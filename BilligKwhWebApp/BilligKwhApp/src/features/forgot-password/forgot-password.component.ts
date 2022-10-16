import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { UserService } from "@core/services/user.service";
import { BiHttpErrorResponse } from "@models/common/BiHttpErrorResponse";
import { TranslateService } from "@ngx-translate/core";
import { BiCustomValidators } from "@shared/classes/BiCustomValidators";
import { finalize, take } from "rxjs";

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html",
  styleUrls: ["./forgot-password.component.scss"]
})
export class ForgotPasswordComponent implements OnInit, AfterViewInit {

  constructor(private userService: UserService,
    private router: Router,
    private translator: TranslateService,
    private cd: ChangeDetectorRef) { }

  public mainForm: FormGroup;

  @ViewChild("loginEmail") private loginEmail: ElementRef<HTMLInputElement>;

  ngOnInit(): void {

    this.mainForm = new FormGroup({
      loginEmail: new FormControl("", [Validators.required, BiCustomValidators.email()])
    });
  }

  ngAfterViewInit() {
    if (this.loginEmail) this.loginEmail.nativeElement.focus();
  }

  public login() {
    if (this.email.valid) {
      this.email.setErrors(undefined);
      this.mainForm.markAsUntouched();

      this.userService
        .requestResetPasswordToken(this.email.value)
        .pipe(
          take(1),
          finalize(() => {
            this.cd.detectChanges();
          }))
        .subscribe(
          () => {
            this.mainForm.reset();
            this.router.navigate(["/login"]);
          },
          (err: BiHttpErrorResponse) => {
            console.log(err)
          });
    }
  }

  public get email() {
    return this.mainForm.get("loginEmail");
  }

}
