import { Component, OnInit, ChangeDetectorRef, HostBinding } from "@angular/core";
import { FormGroup, Validators, ValidationErrors, FormControl } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";

import { of } from "rxjs";
import { finalize, switchMap, take } from "rxjs/operators";

import { UserService } from "@core/services/user.service";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BiCountryId, BiLanguageId } from "@enums/BiLanguageAndCountryId";
import { UserEditModel } from "@apiModels/userEditModel";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { UserModel } from "@apiModels/UserModel";
import { CustomerService } from "@core/services/customer.service";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { HttpErrorResponse } from "@angular/common/http";
import { BiNotificationConfig, BiToastNotificationService, NotificationType } from "@core/utility-services/bi-toast-notification.service";
import { TranslateService } from "@ngx-translate/core";

interface MainFormValue {
  email: string;
  name: string;
  languageId: number;
  countryId: number;
  timeZoneId: string;
  administrator: boolean;
  newPassword: string;
}

@UntilDestroy()
@Component({
  templateUrl: "./user-edit-main.component.html",
  animations: [BiCustomAnimations.fadeInDown, BiCustomAnimations.fadeIn]
})
export class UserEditMainComponent implements OnInit {
  public user?: UserEditModel;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;
  public pageTitle = "User Edit";
  public loading = false;
  public errorMessage = "";

  public deleteOrRecreate = "Slet";
  public createOrSave = "Opret";
  public updateEconomic = "Opret i e-conomic";

  @HostBinding("@fadeInDown") get fadeInHost() {
    return true;
  }

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private customerService: CustomerService,
    private notifier: BiToastNotificationService,
    private translator: TranslateService
  ) {}

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          if (!params["id"] || isNaN(+params["id"])) return of(this.userService.initializeUserModel());
          if (+params["id"] === 0) return of(this.userService.initializeUserModel());
          return this.userService.getUser(+params["id"], null);
        })
      )
      .subscribe(data => {
        if (data) {
          this.user = data;
          //debugger;
          if (this.user.deleted == true) this.deleteOrRecreate = "Genskab";
          this.createOrSave = "Gem";
        }
        this.initFormGroup();
        this.cd.detectChanges();
      });
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      email: new FormControl(this.user?.email, [Validators.required, Validators.email]),
      name: new FormControl(this.user?.name, [Validators.required, Validators.minLength(3)]),
      phone: new FormControl(this.user?.phone, [Validators.maxLength(50)]),
      languageId: new FormControl(this.user?.languageId, [Validators.required]),
      countryId: new FormControl(this.user?.countryId, [Validators.required]),
      newPassword: new FormControl(this.user?.newPassword, [Validators.required, Validators.minLength(4)]),
      administrator: new FormControl(this.user?.administrator)
    });
  }

  public get email() {
    return this.mainForm.get("email");
  }

  public get name() {
    return this.mainForm.get("name");
  }

  public get phone() {
    return this.mainForm.get("phone");
  }

  public get languageId() {
    return this.mainForm.get("languageId");
  }

  public get countryId() {
    return this.mainForm.get("countryId");
  }

  public get newPassword() {
    return this.mainForm.get("newPassword");
  }

  public get administrator() {
    return this.mainForm.get("administrator");
  }

  private createObjectFromFormValue(formValue: MainFormValue): UserModel {
    const returnObject: Partial<UserModel> = {
      ...formValue,
      id: 0
    };
    // If editing an existing user, we can set all other data fields

    if (this.user.id !== 0) {
      returnObject.id = this.user.id;
      returnObject.deleted = this.user.deleted;
    } else {
      returnObject.customerId = this.customerService.CustomerId;
    }

    return returnObject;
  }

  private checkAndValidateForm() {
    if (this.mainForm.invalid) {
      Object.keys(this.mainForm.controls).forEach(cName => this.mainForm.controls[cName].markAsTouched());
      this.showFormErrorMessage = true;
      return false;
    }

    this.showFormErrorMessage = false;
    return true;
  }

  public onSaveClicked() {
    this.saveUser(false);
  }

  public saveUser(toggleDelete: boolean) {
    //debugger;

    if (!this.checkAndValidateForm()) return;

    const stay = this.user.id === 0;

    const formValue = this.mainForm.value as MainFormValue;

    const newOrUpdatedObject = this.createObjectFromFormValue(formValue) as UserModel;

    if (toggleDelete) newOrUpdatedObject.deleted = !newOrUpdatedObject.deleted;

    this.userService.updateUser(newOrUpdatedObject).subscribe({
      next: userId => {
        if (this.user.id === 0) this.user.id = userId;

        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev gemt"
        });

        return this.onSaveComplete(stay);
      },
      //error: err => (this.errorMessage = err)

      error: (err: HttpErrorResponse) => {
        // Notify User

        this.messageService.add({
          life: 5000,
          severity: "error",
          summary: "Der skete en fejl",
          detail: err.error.errorMessage
        });
      }
    });
  }

  onSaveComplete(stay: boolean): void {
    // Reset the form to clear the flags
    this.mainForm.reset();
    // if (stay) this.router.navigate(["/superadmin/users", this.user.id, "main"]);
    // else this.router.navigate(["/superadmin/users"]);
    //debugger;
    this.router.navigate(["/superadmin/customers", this.user.customerId, "users"]);
  }

  confirmBrugerMail(event: Event) {
    this.confirmationService.confirm({
      target: event.target,
      message: "Er du sikker på du vil sende en oprettelses mail?",
      icon: "pi pi-exclamation-triangle",
      accept: () => {
        this.brugerMail();
        //confirm action
      },
      reject: () => {
        //reject action
      }
    });
  }

  public brugerMail() {
    this.userService
      .sendNewUserEmail(this.user.id)
      .pipe(
        untilDestroyed(this),
        take(1),
        finalize(() => console.log("sent"))
      )
      .subscribe(
        () => {
          this.notifier.createNotification(new BiNotificationConfig(undefined, this.translator.instant("shared.EmailSent")));
          // this.onEmailSent();
        },
        err => {
          this.notifier.createNotification(new BiNotificationConfig(NotificationType.ERROR, JSON.stringify(err)));
        }
      );
  }

  public languages: Array<SelectItem> = [
    // { value: null, label: "Vælg sprog" },
    { value: BiLanguageId.DK, label: "Dansk" },
    { value: BiLanguageId.SE, label: "Svensk" },
    { value: BiLanguageId.FI, label: "Finsk" },
    { value: BiLanguageId.NO, label: "Norsk" }
  ];

  public countries: Array<SelectItem> = [
    // { value: null, label: "Vælg land" },
    { value: BiCountryId.DK, label: "Danmark" },
    { value: BiCountryId.SE, label: "Sverige" },
    { value: BiCountryId.FI, label: "Finland" },
    { value: BiCountryId.NO, label: "Norge" }
  ];

  public janej: Array<SelectItem> = [
    { value: true, label: "Ja" },
    { value: false, label: "Nej" }
  ];

  deleteRecreate(): void {
    if (this.user.id === 0) {
      // Don't delete, it was never saved.
      alert("ups");
      this.onSaveComplete(true);
    } else if (this.user.id) {
      if (!this.user.deleted) {
        // if (confirm(`Really delete the user: ${this.user.name}?`)) {
        //   this.saveUser(true);
        // }

        this.confirmationService.confirm({
          message: `Er du sikker på du ønsker at slette brugeren: ${this.user.name}?`,
          icon: "pi pi-exclamation-triangle",
          accept: () => {
            //confirm action
            this.saveUser(true);
          },
          reject: () => {
            //reject action
          }
        });
      } else {
        this.saveUser(true);
      }
    }
  }

  getFormValidationErrors() {
    console.log("%c ==>> Validation Errors: ", "color: red; font-weight: bold; font-size:25px;");
    let totalErrors = 0;
    Object.keys(this.mainForm.controls).forEach(key => {
      const controlErrors: ValidationErrors = this.mainForm.get(key).errors;
      if (controlErrors != null) {
        totalErrors++;
        Object.keys(controlErrors).forEach(keyError => {
          console.log("Key control: " + key + ", keyError: " + keyError + ", err value: ", controlErrors[keyError]);
        });
      }
    });
    console.log("Number of errors: ", totalErrors);
  }
}
