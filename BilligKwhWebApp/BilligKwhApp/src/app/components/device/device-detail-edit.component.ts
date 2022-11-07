import { Component, OnInit, ChangeDetectorRef, HostBinding, ChangeDetectionStrategy } from "@angular/core";
import { FormGroup, FormControl, ValidationErrors } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { finalize, switchMap, take, tap } from "rxjs/operators";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { CustomerService } from "@core/services/customer.service";
import { CustomerModel } from "@apiModels/customerModel";
import { UserService } from "@core/services/user.service";
import { UserModel } from "@apiModels/UserModel";
import { PrimeNgUtilities } from "@shared/variables-and-functions/primeNg-utilities";
import { SmartDeviceService } from "@core/services/smartdevice.service";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";
import { Observable } from "rxjs";
import { ScheduleDto } from "@apiModels/scheduleDto";
import moment from "moment";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";

export interface ScheduleDtoExt extends ScheduleDto {
  dateForSort?: moment.Moment;
  date?: string;
}

@UntilDestroy()
@Component({
  selector: "app-gruppetavle-edit",
  templateUrl: "./device-detail-edit.component.html",
  styleUrls: ["./device-detail-edit.component.scss"],
  providers: [MessageService, ConfirmationService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [BiCustomAnimations.fadeInDown, BiCustomAnimations.fadeIn]
})
export class DevicedetailEditComponent implements OnInit {
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
  flytKundeDialog = false;

  public src: Blob;
  public pdfViewerVisible = false;
  public pdfSpinner = false;
  public displayPdfDialog: boolean;
  public displayVarmeTabDialog: boolean;

  public runningCss = "running";

  public schedules: Array<ScheduleDtoExt> = [];
  public schedules$: Observable<Array<ScheduleDtoExt>>;

  public zones: Array<SelectItem> = [
    { value: 1, label: "DK1" },
    { value: 2, label: "DK2" }
  ];

  @HostBinding("@fadeInDown") get fadeInHost() {
    return true;
  }

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private deviceService: SmartDeviceService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private userService: UserService,
    private customerService: CustomerService,
    private localizor: BiLocalizationHelperService
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
          this.initializeElectricityPrices();
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

  private initializeElectricityPrices() {
    this.schedules$ = this.deviceService.getSmagetSchedulesForToday(this.smartDevice.id).pipe(
      tap((data: Array<ScheduleDto>) => {
        data.forEach(element => {
          //  element.date = this.localizor.localizeDateTime(element.latestContactUtc);
          //  element.dateForSort = moment(element.latestContactUtc);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
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
    return this.mainForm.get("zoneId");
  }

  public get maxRate() {
    return this.mainForm.get("maxRate");
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

    this.deviceService.updateSmartDevice(this.smartDevice).subscribe({
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
    this.deviceService.updateSmartDevice(this.smartDevice).subscribe({
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
    debugger;
    this.smartDevice.location = this.location.value;
    this.smartDevice.zoneId = this.zoneId.value;
    this.smartDevice.maxRate = +this.maxRate.value;

    this.deviceService.updateSmartDevice(this.smartDevice).subscribe({
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
