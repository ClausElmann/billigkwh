import { Component, HostBinding } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { TranslateService } from "@ngx-translate/core";
import { throwError } from "rxjs";
import { finalize, take } from "rxjs/operators";
import { UserService } from "@core/services/user.service";
import { BiNotificationConfig, BiToastNotificationService, NotificationType } from "@core/utility-services/bi-toast-notification.service";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { isStringNullOrEmpty } from "@globals/helper-functions";
// import { BiCustomAnimations } from "../../../classes/BICustomAnimations";
// import { isStringNullOrEmpty } from "../../../variables-and-functions/helper-functions";


/**
 * Component used when user enters the "create new password" page and an error has happened
 */
@UntilDestroy()
@Component({
  selector: "bi-create-new-pass-modal-content",
  templateUrl: "./bi-create-new-pass-error-modal-content.component.html",
  animations: [BiCustomAnimations.fadeIn],
  styles: [
    `
      a.Button--link {
        vertical-align: text-bottom;
        margin-left: 0.3em;
        margin-top: 1em;
        font-size: 1rem;
      }
    `
  ]
})
export class BiCreateNewPassErrorModalContentComponent {
  public serverErrorMessage: string;

  public title: string;

  public onEmailSent: () => void;

  /**
   * Email of the user for which new password is needed
   */
  public userEmail: string;

  /**
   * This should be false if the error didn't have something to do  with
   * invalid/expired token
   */
  public showResendEmailContent = false;

  public working = false;

  @HostBinding("@fadeIn") public get() {
    return true;
  }

  constructor(private translator: TranslateService, private userService: UserService, private notifier: BiToastNotificationService) {

  }

  public ngOnInit() {
    if (isStringNullOrEmpty(this.userEmail)) throwError("No user email provided!");

    if (!this.onEmailSent) throwError("Please provide callback for 'onEmailSent'");

    // Set title depending on error
    if (this.serverErrorMessage.indexOf("invalid") !== -1) {
      this.title = this.translator.instant("login.NewPassTokenNotVerified");
      this.showResendEmailContent = true;
    } else if (this.serverErrorMessage.indexOf("exp") !== -1) {
      this.title = this.translator.instant("login.NewPassTokenExpired");
      this.showResendEmailContent = true;
    } else if (this.serverErrorMessage.indexOf("User") !== -1) this.title = this.translator.instant("errorMessages.UserNotFound");
    else this.title = `${this.translator.instant("errorMessages.ErrorOccurred")}: ${this.serverErrorMessage}`;
  }

  public resendEmail() {
    this.working = true;
    this.userService
      .resendPasswordEmail(this.userEmail)
      .pipe(
        untilDestroyed(this),
        take(1),
        finalize(() => (this.working = false))
      )
      .subscribe(
        () => {
          this.notifier.createNotification(new BiNotificationConfig(undefined, this.translator.instant("shared.EmailSent")));
          this.onEmailSent();
        },
        (err) => {
          this.notifier.createNotification(new BiNotificationConfig(NotificationType.ERROR, JSON.stringify(err)));
        }
      );
  }
}
