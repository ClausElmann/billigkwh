import { TranslateService } from "@ngx-translate/core";
import { Observable, of } from "rxjs";
import { finalize, map, retry, switchMap, take } from "rxjs/operators";
import { CustomerService } from "./core/services/customer.service";
import { UserService } from "./core/services/user.service";
import { getLanguageCodeByHostName, isInsideIFrameOrPopup } from "./shared/variables-and-functions/helper-functions";
import { TokenAuthenticationService } from "@core/security/TokenAuthenticationService";
import { getLanguageCodeByLanguageId } from "@globals/helper-functions";
import moment from "moment";

// App initializer factory used for hooking up on Angular's app initialization process (APP_INITIALIZER).
// We use this for making sure that correct language is set BEFORE app shows anything. Also checks if a user
// is already logged in and in that case ensures that the customer is loaded before proceeding (as customer has info about country).
export function billigkwhAppInitializer(translate: TranslateService, userService: UserService, customerService: CustomerService, authService: TokenAuthenticationService) {
  return () =>
    new Promise<any>(resolve => {
      let observable: Observable<boolean>;
      if (!isInsideIFrameOrPopup() && authService.refreshToken()) {
        // If user access token expired but we have a refresh token
        if (!authService.accessTokenModel() || moment(authService.accessTokenModel().expiresAt).isBefore(moment())) {
          observable = authService.refresh().pipe(
            switchMap(() => userService.refreshUserState()),
            map(() => true)
          );
        } else {
          // User exists. Make user service do what it needs (e.g. get info) and get customer info (will set info like country id)
          observable = userService.handleUserExists().pipe(map(() => true));
        }
      } else observable = of(false);

      observable.subscribe(userExists => {
        if (!userExists) {
          // Check domain - if we can determine language by that, do that. Otherwise, we must call server
          const lang = getLanguageCodeByHostName();
          if (lang) {
            translate.use(lang);
            document.documentElement.lang = lang;
            return resolve(true);
          } else {
            userService
              .getWorkingLanguage()
              .pipe(
                retry(3),
                finalize(() => resolve(true)),
                switchMap(lang => {
                  // If something went wrong when getting the language, just move on with app initialization
                  if (!lang) return translate.use("da");
                  // Now we set the language no matter browser locale
                  const langToSet = lang.uniqueSeoCode ? lang.uniqueSeoCode : "da";
                  document.documentElement.lang = langToSet;

                  return translate.use(langToSet);
                })
              )
              .subscribe();
          }
        } else {
          customerService
            .getCustomer()
            .pipe(take(1))
            .subscribe(() => {
              const lang = getLanguageCodeByLanguageId(userService.getCurrentStateValue().currentUser?.languageId);
              translate.use(lang);
              document.documentElement.lang = lang;
              resolve(true);
            });
        }
      });
    });
}
