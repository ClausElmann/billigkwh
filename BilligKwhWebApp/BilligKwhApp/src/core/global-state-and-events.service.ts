import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { BehaviorSubject, Subject } from "rxjs";
import { BiStore } from "@globals/classes/BiStore";
import { BiCountryId } from "@globals/enums/BiLanguageAndCountryId";
//import { GlobalState } from "./GlobalState";
import { BiNotificationConfig } from "./utility-services/bi-toast-notification.service";
import { take } from "rxjs/operators";
import { UntilDestroy } from "@ngneat/until-destroy";
import { GlobalState } from "./services/GlobalState";
import { UserModel } from "@apiModels/UserModel";

/**
 * Class for defining global events used in whole application. Using the power of RxJs!
 */
@UntilDestroy({ checkProperties: true })
@Injectable()
export class GlobalStateAndEventsService extends BiStore<GlobalState> {
  public constructor(private translator: TranslateService) {
    super(new GlobalState([]));

    // When translations are ready, we can set the country names
    translator.onLangChange.pipe(take(1)).subscribe(() => {
      this.setState({
        ...this.state.value,
        countries: [
          {
            name: translator.instant("shared.Denmark"),
            countryId: BiCountryId.DK
          },
          {
            name: translator.instant("shared.Sweden"),
            countryId: BiCountryId.SE
          },
          {
            name: translator.instant("shared.Finland"),
            countryId: BiCountryId.FI
          },
          {
            name: translator.instant("shared.Norway"),
            countryId: BiCountryId.NO
          }
        ]
      });
    });
  }

  //#region Events
  public failedAccessTokenRefresh = new Subject<void>(); // called when server fails to either create or refresh access token

  /**
  * fired when accesstoken nearly expires. event value number of seconds left
  */
  public refreshTokenNearlyExpired = new Subject<number>();
  /**
     * Subject for controlling refreshTokenExpireDate
     */
  public refreshTokenExpireDate = new BehaviorSubject<Date>(undefined);
  /**
     * Subject for controlling visiblity of the page overlay. Subscriber will set visiblity depending on the emitted value - true/false
     */
  public togglePageOverlayVisibilitySubject = new Subject<boolean>();

  /**
   * Subject for controlling visiblity of the "content loading" spinner and overlay. Subscriber will set visiblity depending on the emitted value - true/false
   */
  public toggleContentLoadingSpinnerVisibilitySubject = new Subject<boolean>();

  /**
   * Subject for showing the global app event notification. Usefull every time some event occurs
   */
  public showAppEventNotification = new Subject<BiNotificationConfig>();

  /**
   * To be fired when a new login has been performed. Eventvalue is the user who logged in.
   */
  public loginEvent = new Subject<UserModel>();

  public loggedOut = new Subject<void>();

  private profileChanged = new Subject<number>();

  public profileChanged$ = this.profileChanged.asObservable();

  private customerChanged = new Subject<number>();

  public customerChanged$ = this.customerChanged.asObservable();


  private impersonateFromUserChanged = new Subject<number>();

  public impersonateFromUserChanged$ = this.impersonateFromUserChanged.asObservable();

  public fireCustomerChanged(customerId: number) {
    this.customerChanged.next(customerId);
  }

  public fireProfileChanged(profileId: number) {
    this.profileChanged.next(profileId);
  }


  public fireimpersonateFromUserChanged(userId: number) {
    this.impersonateFromUserChanged.next(userId);
  }

  /**
   * To be fired when an email for resetting a user's password has been sent. Not a "global event" but easiest to define here so that AppComponent can listen for it
   */
  public resetPassEmailSent = new Subject<void>();

  //#endregion
}
