import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostBinding, HostListener, Inject, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DefaultUrlSerializer, Router, RouterStateSnapshot } from "@angular/router";
import { GlobalStateAndEventsService } from "@core/global-state-and-events.service";
import { AUTHENTICATION_SERVICE_TOKEN, TokenAuthenticationService } from "@core/security/TokenAuthenticationService";
import { UserService } from "@core/services/user.service";
//import { BiModalService } from "@core/utility-services/bi-modal.service";
import { CustomValidators } from "@globals/classes/custom-validators";
import { isRunningInIE } from "@globals/helper-functions";
import { BiHttpErrorResponse } from "@models/common/BiHttpErrorResponse";
import { TranslateService } from "@ngx-translate/core";
import { BiCustomValidators } from "@shared/classes/BiCustomValidators";
import { RouteNames } from "@shared/classes/RouteNames";
import { WindowSessionStorageNames } from "@shared/variables-and-functions/WindowSessionStorageNames";
import { finalize, map, Observable, of, take } from "rxjs";

@Component({
  selector: "app-login",
  templateUrl: "./el-login.component.html",
  styleUrls: ["./el-login.component.scss"]
})
export class ElLoginComponent implements OnInit, AfterViewInit {

  constructor(private userService: UserService,
    @Inject(AUTHENTICATION_SERVICE_TOKEN)
    private authService: TokenAuthenticationService,
    private router: Router,
    //private modalService: BiModalService,
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

  @HostListener("document:keydown.escape", ["$event"]) escapePressed(event) {
    this.onCloseClicked();
  }

  @ViewChild("loginEmail") private loginEmail: ElementRef<HTMLInputElement>;

  /**
   * Control for the email-input in the Reset Password functionality. Notice: this wont be part of the "loginForm" an so  we must use [formControl] expression in tempalte
   * to make it stand alone and register with the form
   */
  public resetPassEmailControl = new FormControl("", [Validators.required, BiCustomValidators.email()]);
  public twoFactorAuthenticationForm = new FormControl("", [Validators.required, CustomValidators.digitsOnly()]);

  //#endregion

  // @HostBinding("@fadeInOut") get fadeInOutAnim() {
  //   return true;
  // }


  ngOnInit(): void {

    const attemptedRoute = this.eventsManager.getCurrentStateValue()?.routeAfterLogin;

    if (attemptedRoute) {
      const parsedRoute = new DefaultUrlSerializer().parse(attemptedRoute);

      // read route params
      //this.bla = parseInt(parsedRoute.queryParamMap.get("bla"), 10);
    }


    this.loginForm = new FormGroup({
      loginEmail: new FormControl("", [Validators.required, BiCustomValidators.email()]),
      password: new FormControl("", [Validators.required])
    });

    // If running in the shit hole browser, warn user
    if (!window.sessionStorage.getItem(WindowSessionStorageNames.IEWarningSeen) && isRunningInIE()) this.warnIEUser = true;
    //this.loginEmail.nativeElement.focus();
    // this.email.updateValueAndValidity();
    // this.cd.detectChanges();
  }

  ngAfterViewInit() {
    if (this.loginEmail) this.loginEmail.nativeElement.focus();
  }

  public login() {
    if (this.email.valid && this.password.valid) {
      this.failedLoginMessage = undefined;
      this.email.setErrors(undefined);
      this.password.setErrors(undefined);
      this.loginForm.markAsUntouched();

      this._isLoading = true;

      this.userService
        .login(this.email.value, this.password.value)
        .pipe(
          take(1),
          finalize(() => {
            this._isLoading = false;
            this.cd.detectChanges();
          }))
        .subscribe(
          (newUser) => {
            // Successfull login
            this.loginForm.reset();
            this.eventsManager.loginEvent.next(newUser);
            this.hideLoginForm = true;
            this.router.navigate([""]);
          },
          (err: BiHttpErrorResponse) => {
            // Slide in Choose delivery Method
            if (err.status === 300) {
              //this.slideContainerValue = 2;
            }
            // Send Email and go to Input
            else if (err.status === 428) {
              this.sendPinCodeByEmail();
            }
            else {
              this.handleFailedLogin(err);
            }
          });
    }
  }

  public sendPinCodeByEmail() {
    this.userService
      .sendCodeByEmail(this.email.value)
      .subscribe(response => {
        // this.twoFactorMethodUsed = "email";
        // this.slideContainerValue = 3;
        this.cd.detectChanges();
      });
  }

  public onCloseClicked() {

    this.router.navigate([RouteNames.frontPage]);
  }

  public onTwoFactorLogin() {
    this.userService
      .logInTwoFactor(this.email.value, Number(this.twoFactorAuthenticationForm.value))
      .pipe(
        take(1),
        finalize(() => {
          this._isLoading = false;
          this.cd.detectChanges();
        }))
      .subscribe(
        (userToken) => {
          this.loginForm.reset();
          this.eventsManager.loginEvent.next(userToken);
          this.hideLoginForm = true;
        },
        (error: BiHttpErrorResponse) => {
          this.handleFailedLogin(error);
        });
  }

  public onDismissIEWarning() {
    // Use localstorage to store the info that user has seen and dismissed the IE warning
    window.sessionStorage.setItem(WindowSessionStorageNames.IEWarningSeen, "1");
    this.warnIEUser = false;
  }

  //==== Getters for easy access to form control's data from the view
  public get email() {
    return this.loginForm.get("loginEmail");
  }
  public get password() {
    return this.loginForm.get("password");
  }
  //==================

  private handleFailedLogin(err: BiHttpErrorResponse) {
    this._isLoading = false;
    // When forbidden it means that user account is locked
    if (err.status === 403) {
      //this.generateLoginLockedModalContent().subscribe((content) => this.modalService.open(content));
    } else {
      this.failedLoginMessage = err.error.errorMessage;
    }
  }

  /**
   * Implementation of custom CanDeactivate interface CanDeactivateComponent.
   */
  canIDeactivate(nextState: RouterStateSnapshot): Observable<boolean> {
    // Only if navigating to frontpage, it's okay to leave.
    return of(nextState.url.indexOf(RouteNames.frontPage) !== -1);
  }

  private generateLoginLockedModalContent() {
    return this.translator.get(["errorMessages.LoginLocked", "login.ResetPassword", "shared.Close"]).pipe(
      take(1),
      map((translations) => {
        const modalWrapperDiv = $(`<div style="padding-top: 2em;"></div>`);
        modalWrapperDiv.append(`<h3 style="margin-bottom: 2em">${translations["errorMessages.LoginLocked"]}</h3>`);

        const okButton = $(`<button class="p-button-secondary p-button margin-right-1">${translations["shared.Close"]}</button>`).click(() => {
          //   this.modalService.close();

        });
        const resetPasswordButton = $(`<button class="p-button-primary p-button type="submit">${translations["login.ResetPassword"]}</button>`).click(() => {
          //   this.modalService.close();
          this.showResetPassContent = true;
          //this.slideContainerValue = 1;
          this.cd.detectChanges();
        });

        modalWrapperDiv.append(okButton);
        modalWrapperDiv.append(resetPasswordButton);

        return modalWrapperDiv;
      })
    );
  }
}
