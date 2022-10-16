import { HttpErrorResponse } from "@angular/common/http";
import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { UserService } from "@core/services/user.service";
import { BiNotificationConfig, BiToastNotificationService } from "@core/utility-services/bi-toast-notification.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { TranslateService } from "@ngx-translate/core";
import { BiCustomValidators } from "@shared/classes/BiCustomValidators";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { throwError as observableThrowError } from "rxjs";
import { take } from "rxjs/operators";

@UntilDestroy()
@Component({
  selector: "user-security",
  templateUrl: "./user-security.component.html",
  styleUrls: ["./user-security.component.scss"],
  animations: [BiCustomAnimations.fadeInOutInlineBlock, BiCustomAnimations.fadeInDown],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserSecurityComponent implements OnInit {
  public passwordForm: FormGroup;

  public termsConditionsCtrl: FormControl; // standalone control for the app's terms and conditions. Super Admin

  public isSuperAdmin = this.userService.getCurrentStateValue().currentUser.isSuperAdmin;

  constructor(
    private translator: TranslateService,
    private notifier: BiToastNotificationService,
    private userService: UserService
  ) {

  }

  public ngOnInit() {
    this.passwordForm = new FormGroup({
      currentPass: new FormControl({ value: this.isSuperAdmin ? this.translator.instant("shared.NotRelevantForSuperAdmin") : "", disabled: this.isSuperAdmin }, [Validators.required, Validators.minLength(8)]),
      newPass: new FormControl("", [Validators.required, BiCustomValidators.password()]),
      newPassConfirm: new FormControl("")
    });

    // We must set validators for the newPassConfirm control here as the Form controls must exist first
    this.newPassConfirm.setValidators([Validators.required, (c) => BiCustomValidators.equalToOther(this.newPass)(c)]);

  }

  //====== GETTERS FOR EASY ACCESS TO FORMCONTROLS =============
  public get currentPass() {
    return this.passwordForm.get("currentPass");
  }
  public get newPass() {
    return this.passwordForm.get("newPass");
  }
  public get newPassConfirm() {
    return this.passwordForm.get("newPassConfirm");
  }
  //===========================================================

  /**
   * Handler for when user clicks save after finished editing password
   */
  public updatePassword() {
    if (this.passwordForm.dirty && this.passwordForm.valid) {
      const formValue = this.passwordForm.value;
      this.userService
        .changePassword(formValue.newPass, formValue.currentPass)
        .pipe(untilDestroyed(this), take(1))
        .subscribe(
          () => {
            this.passwordForm.reset();
            this.notifier.createNotification(new BiNotificationConfig(undefined, this.translator.instant("settings.myUser.PasswordUpdated")));
            this.passwordForm.reset();
          },
          (err: HttpErrorResponse) => {
            if (err.error.indexOf("Current") !== -1) {
              // entered value for current password invalid!
              this.currentPass.setErrors({
                noMatch: true
              });
            } else return observableThrowError(err);
          }
        );
    }
  }
}
