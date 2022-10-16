import { Component, OnInit, ChangeDetectorRef, ChangeDetectionStrategy, HostBinding } from "@angular/core";
import { FormGroup, Validators, FormControl, ValidationErrors } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";

import { of } from "rxjs";
import { switchMap, take } from "rxjs/operators";

import { CustomerModel } from "@apiModels/customerModel";

import { CustomerService } from "@core/services/customer.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { NumberValidators } from "@globals/classes/number.validator";
import { BiCountryId, BiLanguageId } from "@enums/BiLanguageAndCountryId";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";

interface MainFormValue {
  name: string;
  address: string;
  zipcode: number;
  city: string;
  deleted: boolean;
  publicId: string;
  languageId: number;
  countryId: number;
  companyRegistrationId: number;
  timeZoneId: string;
  hourWage: number;
  coveragePercentage: number;
  economicId: number;
  invoiceMail: string;
  invoiceContactPerson: string;
  invoicePhoneFax: string;
  invoiceMobile: string;
}

@UntilDestroy()
@Component({
  templateUrl: "./superadmin-customer-createedit.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [BiCustomAnimations.fadeInDown, BiCustomAnimations.fadeIn]
})
export class SuperAdminCustomerCreateEditComponent implements OnInit {
  public customer?: CustomerModel;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;
  public pageTitle = "Customer Edit";
  public loading = false;
  public errorMessage = "";

  public deleteOrRecreate = "Slet";
  public createOrSave = "Opret";
  //public updateEconomic = "Opret i e-conomic";

  @HostBinding("@fadeInDown") get fadeInHost() {
    return true;
  }

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private customerService: CustomerService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          if (!params["id"] || isNaN(+params["id"])) return of(this.customerService.initializeCustomerModel());
          if (+params["id"] === 0) return of(this.customerService.initializeCustomerModel());
          this.customerService.currentCustomerId = +params["id"];
          return this.customerService.getCustomer(+params["id"], null);
        })
      )
      .subscribe(data => {
        if (data) {
          this.customer = data;
          if (this.customer.deleted == true) this.deleteOrRecreate = "Genskab";
          this.createOrSave = "Gem";
          //if (this.customer.economicId !== null) this.updateEconomic = "Opdater i e-conomic";
        }
        this.initFormGroup();
        this.cd.detectChanges();
      });
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      name: new FormControl(this.customer?.name, [Validators.required, Validators.minLength(3)]),
      address: new FormControl(this.customer?.address, [Validators.required]),
      languageId: new FormControl(this.customer?.languageId, [Validators.required]),
      countryId: new FormControl(this.customer?.countryId, [Validators.required]),
      companyRegistrationId: new FormControl(this.customer?.companyRegistrationId, [NumberValidators.range(10000000, 99999999)]),
      hourWage: new FormControl(this.customer?.hourWage, [Validators.required, NumberValidators.range(0, 2000)]),
      coveragePercentage: new FormControl(this.customer?.coveragePercentage, [Validators.required, NumberValidators.range(1, 2.0)]),
      invoiceMail: new FormControl(this.customer?.invoiceMail, [Validators.email]),
      invoiceContactPerson: new FormControl(this.customer?.invoiceContactPerson),
      invoicePhoneFax: new FormControl(this.customer?.invoicePhoneFax, [Validators.maxLength(10)]),
      invoiceMobile: new FormControl(this.customer?.invoiceMobile, [Validators.maxLength(50)]),
      zipcode: new FormControl(this.customer?.zipcode, [Validators.required, Validators.maxLength(4), NumberValidators.range(0, 9999)]),
      city: new FormControl(this.customer?.city, [Validators.required])
    });
  }

  public get name() {
    return this.mainForm.get("name");
  }

  public get address() {
    return this.mainForm.get("address");
  }

  public get languageId() {
    return this.mainForm.get("languageId");
  }

  public get countryId() {
    return this.mainForm.get("countryId");
  }

  public get companyRegistrationId() {
    return this.mainForm.get("companyRegistrationId");
  }

  public get hourWage() {
    return this.mainForm.get("hourWage");
  }

  public get coveragePercentage() {
    return this.mainForm.get("coveragePercentage");
  }

  public get invoiceMail() {
    return this.mainForm.get("invoiceMail");
  }

  public get invoiceContactPerson() {
    return this.mainForm.get("invoiceContactPerson");
  }

  public get invoicePhoneFax() {
    return this.mainForm.get("invoicePhoneFax");
  }

  public get invoiceMobile() {
    return this.mainForm.get("invoiceMobile");
  }

  public get zipcode() {
    return this.mainForm.get("zipcode");
  }

  public get city() {
    return this.mainForm.get("city");
  }

  private createObjectFromFormValue(formValue: MainFormValue): CustomerModel {
    const returnObject: Partial<CustomerModel> = {
      ...formValue,
      id: 0
    };

    returnObject.coveragePercentage = +returnObject.coveragePercentage;

    // If editing an existing customer, we can set all other data fields
    if (this.customer) {
      returnObject.id = this.customer.id;
      returnObject.deleted = this.customer.deleted;
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
    this.saveCustomer(false);
  }

  public saveCustomer(toggleDelete: boolean) {
    if (!this.checkAndValidateForm()) return;

    const stay = this.customer.id === 0;

    const formValue = this.mainForm.value as MainFormValue;

    const newOrUpdatedObject = this.createObjectFromFormValue(formValue) as CustomerModel;

    if (toggleDelete) newOrUpdatedObject.deleted = !newOrUpdatedObject.deleted;

    this.customerService.updateCustomer(newOrUpdatedObject).subscribe({
      next: customerId => {
        if (this.customer.id === 0) this.customer.id = customerId;

        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev gemt"
        });

        return this.onSaveComplete(stay);
      },
      error: err => (this.errorMessage = err)
    });
  }

  onSaveComplete(stay: boolean): void {
    // Reset the form to clear the flags
    this.mainForm.reset();
    if (stay) this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/superadmin/customers", this.customer.id, "main"]));
    else this.router.navigate(["/superadmin/customers"]);
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

  deleteRecreate(): void {
    if (this.customer.id === 0) {
      // Don't delete, it was never saved.
      this.onSaveComplete(true);
    } else if (this.customer.id) {
      if (!this.customer.deleted) {
        // if (confirm(`Really delete the customer: ${this.customer.name}?`)) {
        //   this.saveCustomer(true);
        // }

        this.confirmationService.confirm({
          message: `Er du sikker på du ønsker at slette kunden: ${this.customer.name}?`,
          icon: "pi pi-exclamation-triangle",
          accept: () => {
            //confirm action
            this.saveCustomer(true);
          },
          reject: () => {
            //reject action
          }
        });
      } else {
        this.saveCustomer(true);
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
