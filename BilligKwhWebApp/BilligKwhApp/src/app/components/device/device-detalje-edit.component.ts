import { Component, OnInit, ChangeDetectorRef, HostBinding, ChangeDetectionStrategy } from "@angular/core";
import { FormGroup, FormControl, ValidationErrors } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap, take } from "rxjs/operators";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { CustomerService } from "@core/services/customer.service";
import { CustomerModel } from "@apiModels/customerModel";
import { UserService } from "@core/services/user.service";
import { UserModel } from "@apiModels/UserModel";
import { PrimeNgUtilities } from "@shared/variables-and-functions/primeNg-utilities";
import { DeviceService } from "@core/services/device.service";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";

@UntilDestroy()
@Component({
  selector: "app-gruppetavle-edit",
  templateUrl: "./device-detalje-edit.component.html",
  styleUrls: ["./device-detalje-edit.component.scss"],
  providers: [MessageService, ConfirmationService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [BiCustomAnimations.fadeInDown, BiCustomAnimations.fadeIn]
})
export class DeviceDetaljeEditComponent implements OnInit {
  public smartDevice?: SmartDeviceDto;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;
  public pageTitle = "Enheds redigering";
  public loading = false;
  public errorMessage = "";
  public customers: Array<CustomerModel>;
  public users: Array<UserModel>;
  public oprettetAfBrugere: Array<SelectItem>;

  public valgtKunde: CustomerModel;
  public valgtBruger: UserModel;

  public deleteOrRecreate = "Slet";
  public createOrSave = "Opret";
  public updateEconomic = "Opdater i e-conomic";
  public exportCSV = PrimeNgUtilities.exportCSV;

  deleteDialog = false;
  takForBestillingDialog = false;
  viHarNuSendtDinTavleDialog = false;
  bookInvoiceDialog = false;
  sendFakturaMailDialog = false;
  economicManglerDatoOpdatering = false;

  flytKundeDialog = false;

  public src: Blob;
  public pdfViewerVisible = false;
  public pdfSpinner = false;
  public displayPdfDialog: boolean;
  public displayVarmeTabDialog: boolean;

  varmeberegning: string;

  @HostBinding("@fadeInDown") get fadeInHost() {
    return true;
  }

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private userService: UserService,
    private customerService: CustomerService
  ) {}

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          return this.deviceService.getSmartDevice(+params["id"]);
        })
      )
      .subscribe(data => {
        if (data) {
          this.smartDevice = data;

          // this.economicManglerDatoOpdatering = this.economicNotUpdatedToday();

          //if (this.print.slettet == true) this.deleteOrRecreate = "Genskab";
          this.createOrSave = "Gem";
          // this.userService
          //   .getUsersByCustomer(this.print.kundeID, false, this.print.oprettetAfBrugerID)
          //   .pipe(take(1))
          //   .subscribe(c => {
          //     this.oprettetAfBrugere = c.map(c => <SelectItem>{ value: c.id, label: c.email });
          this.initFormGroup();
          this.cd.detectChanges();
          //   });
        }
      });
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      location: new FormControl(this.smartDevice.location),
      zoneId: new FormControl(this.smartDevice.zoneId),
      maxRate: new FormControl(this.smartDevice.maxRate)
    });
  }

  public get location() {
    return this.mainForm.get("location");
  }

  public get zoneId() {
    return this.mainForm.get("location");
  }

  public get maxRate() {
    return this.mainForm.get("location");
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

  deleteItem() {
    this.deleteDialog = true;
  }

  deleteRecreate(): void {
    if (this.smartDevice.deleted) {
      this.recreateItem();
    } else {
      this.deleteDialog = true;
    }
  }

  recreateItem() {
    this.smartDevice.deleted = null;

    this.deviceService.updatePrint(this.smartDevice).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev genskabt"
        });

        this.mainForm.reset();

        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/devices", this.smartDevice.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  confirmDelete() {
    this.smartDevice.deleted = new Date().toString();
    this.deviceService.updatePrint(this.smartDevice).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev slettet"
        });

        this.mainForm.reset();

        this.router.navigate(["/devices"]);
      },
      error: err => (this.errorMessage = err)
    });
  }

  saveItem() {
    if (!this.checkAndValidateForm()) return;

    this.smartDevice.location = this.location.value;
    this.smartDevice.zoneId = this.zoneId.value;
    this.smartDevice.maxRate = this.maxRate.value;

    this.deviceService.updatePrint(this.smartDevice).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev gemt"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/devices", this.smartDevice.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  public flytKunde() {
    this.customerService
      .getCustomers(1, false)
      .pipe(take(1))
      .subscribe(c => {
        this.customers = c;
        this.flytKundeDialog = true;
        this.cd.detectChanges();
      });
  }

  onKundeChange(event: CustomerModel) {
    this.userService
      .getUsersByCustomer(this.valgtKunde.id, false)
      .pipe(take(1))
      .subscribe(c => {
        console.log(c);
        this.users = c;
        this.cd.detectChanges();
      });
  }
}
