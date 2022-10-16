import { HttpErrorResponse } from "@angular/common/http";
import { ChangeDetectorRef, Component, HostBinding, Inject, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { GlobalStateAndEventsService } from "@core/global-state-and-events.service";
import { AUTHENTICATION_SERVICE_TOKEN, TokenAuthenticationService } from "@core/security/TokenAuthenticationService";
import { UserService } from "@core/services/user.service";
//import { BiModalService } from "@core/utility-services/bi-modal.service";
import { isStringNullOrEmpty } from "@globals/helper-functions";
import { BiHttpErrorResponse } from "@models/common/BiHttpErrorResponse";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { TranslateService } from "@ngx-translate/core";
import { RouteNames } from "@shared/classes/RouteNames";
import { finalize, take } from "rxjs";

@UntilDestroy()
@Component({
  selector: "app-login",
  templateUrl: "./password-reset-create.component.html",
  styleUrls: ["./password-reset-create.component.scss"]
})
export class PasswordResetCreateComponent implements OnInit {

  constructor(private userService: UserService,
    @Inject(AUTHENTICATION_SERVICE_TOKEN)
    private authService: TokenAuthenticationService,
    private router: Router,
    private currentRoute: ActivatedRoute,
    private eventsManager: GlobalStateAndEventsService,
    private translator: TranslateService,
    private cd: ChangeDetectorRef) { }

  public loginForm: FormGroup;
  public hideLoginForm = false;
  public warnIEUser: boolean;
  public failedLoginMessage: string;
  public _isLoading: boolean;
  public showResetPassContent = false;
  public emailIsReadonly = false;

  @HostBinding("class.is-loading") get isLoading() {
    return this._isLoading;
  }

  /**
    * The email received in url param
    */
  public userEmail: string;

  /**
   * The unique "reset-password-token" received in url param
   */
  public resetPassToken: string;

  public isResetPass = this.router.url.indexOf(RouteNames.resetPassword) !== -1;

  public passwordFormatHelperHtml: string;
  public tooltipPosition: "top" | "right" = "top";
  public invalidLogin = "";

  ngOnInit(): void {

    this.setPasswordFormatHelperHtml();

    this.loginForm = new FormGroup({
      newPassword: new FormControl("", [Validators.required]),
      confirmPassword: new FormControl("", [Validators.required])
    });

    //this.setTextStrings();

    this.currentRoute.queryParams.pipe(untilDestroyed(this)).subscribe((params) => {
      this.userEmail = params["email"];

      if (isStringNullOrEmpty(this.userEmail) || isStringNullOrEmpty(params["token"])) {
        this.loginForm.disable();
        this.router.navigate([RouteNames.frontPage]);
      } else {
        // email and token received - verify on server
        this.resetPassToken = params["token"]; // save the token locally first
        this.userService
          .verifyResetPasswordToken(this.resetPassToken, this.userEmail)
          .subscribe(null, (err: HttpErrorResponse) => this.handleTokenVerifyError(err));
      }
    });

    // const attemptedRoute = this.eventsManager.getCurrentStateValue()?.routeAfterLogin;

    // if (attemptedRoute) {
    //   const parsedRoute = new DefaultUrlSerializer().parse(attemptedRoute);

    //   // read route params
    //   //this.bla = parseInt(parsedRoute.queryParamMap.get("bla"), 10);
    // }


    // this.loginForm = new FormGroup({
    //   newPassword: new FormControl("", [Validators.required]),
    //   confirmPassword: new FormControl("", [Validators.required])
    // });

    // // If running in the shit hole browser, warn user
    // if (!window.sessionStorage.getItem(WindowSessionStorageNames.IEWarningSeen) && isRunningInIE()) this.warnIEUser = true;
    // //this.loginEmail.nativeElement.focus();
    // // this.email.updateValueAndValidity();
    // // this.cd.detectChanges();
  }



  public login() {
    if (this.newPassword.valid && this.confirmPassword.valid) {
      this.failedLoginMessage = undefined;
      this.newPassword.setErrors(undefined);
      this.confirmPassword.setErrors(undefined);
      this.loginForm.markAsUntouched();

      this._isLoading = true;

      this.userService
        //.login(this.email.value, this.password.value)

        .resetPassword(this.resetPassToken, this.userEmail, this.newPassword.value, this.confirmPassword.value)

        .pipe(
          take(1),
          finalize(() => {
            this._isLoading = false;
            this.cd.detectChanges();
          }))
        .subscribe(
          (token) => {
            console.log(token);
            // Successfull login
            this.loginForm.reset();
            this.hideLoginForm = true;
            this.router.navigate([RouteNames.login]);
          },
          (err: BiHttpErrorResponse) => {
            // Slide in Choose delivery Method
            if (err.status === 300) {
              //this.slideContainerValue = 2;
            }
            this.invalidLogin = this.passwordFormatHelperHtml;
          });
    }
    else this.loginForm.markAsTouched(); // for showing errors not visible because form hasn't been touched
  }


  private setPasswordFormatHelperHtml() {
    this.translator
      .get([
        "settings.myUser.PasswordCriteriaTitle",
        "settings.myUser.aDigit",
        "settings.myUser.aLowerCaseLetter",
        "settings.myUser.anUpperCaseLetter",
        "settings.myUser.1OfCharacters"
      ])
      .pipe(take(1))
      .subscribe((translations) => {
        this.passwordFormatHelperHtml = `<p style="margin: 0">${translations["settings.myUser.PasswordCriteriaTitle"]}</p><ul style="list-style-type: bullet;"><li>${translations["settings.myUser.aDigit"]}</li><li>${translations["settings.myUser.aLowerCaseLetter"]}</li><li>${translations["settings.myUser.anUpperCaseLetter"]}</li><li>${translations["settings.myUser.1OfCharacters"]}</li></ul>`;
      });
  }

  /**
   * Helper for handling error at token verification process.
   * @param error The error message returned from server
   */
  private handleTokenVerifyError(error: BiHttpErrorResponse) {
    const callback = () => {
      this.router.navigate([RouteNames.landing]);
    };
    this.loginForm.disable();
  }

  //==== Getters for easy access to form control's data from the view
  public get newPassword() {
    return this.loginForm.get("newPassword");
  }

  public get confirmPassword() {
    return this.loginForm.get("confirmPassword");
  }
  //==================
}
