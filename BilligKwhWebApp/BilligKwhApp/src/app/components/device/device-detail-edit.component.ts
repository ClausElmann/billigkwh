import { Component, OnInit, ChangeDetectorRef, HostBinding, ChangeDetectionStrategy } from "@angular/core";
import { FormGroup, FormControl, ValidationErrors, Validators } from "@angular/forms";
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
import { CustomValidators } from "@globals/classes/custom-validators";

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

  public statuses: Array<SelectItem> = [
    { value: -1, label: "Efter opsætning (Auto)" },
    { value: 0, label: "Tvangs slukket" },
    { value: 1, label: "Tvangs tændt" }
  ];

  // public editDialog = false;
  // private recipes = new BehaviorSubject<RecipeDto[]>([]);
  // public recipes$: Observable<RecipeDto[]> = this.recipes.asObservable();
  // cols: any[];
  // public selectedItem: RecipeDto;

  // public recipeForm: FormGroup;

  // public dayTypeItems: Array<SelectItem> = [
  //   { value: 0, label: "Alle" },
  //   { value: 1, label: "Hverdage" },
  //   // { value: 2, label: "Lørdag" },
  //   // { value: 3, label: "Søndag" },
  //   { value: 4, label: "Weekend" }
  // ];

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

          // this.cols = [
          //   { field: "priority", header: "priority" },
          //   { field: "dayTypeName", header: "dayTypeName" },
          //   { field: "maxRate", header: "maxRate" },
          //   { field: "fromHour", header: "fromHour" },
          //   { field: "toHour", header: "toHour" },
          //   { field: "minHours", header: "minHours" },
          //   { field: "minTemperature", header: "minTemperature" },
          //   { field: "maxRateAtMinTemperature", header: "maxRateAtMinTemperature" }
          // ];

          //this.initializeRecipes();
          // this.economicManglerDatoOpdatering = this.economicNotUpdatedToday();

          //if (this.print.slettet == true) this.deleteOrRecreate = "Genskab";
          this.createOrSave = "Gem";
          // this.userService
          //   .getUsersByCustomer(this.print.kundeID, false, this.print.oprettetAfBrugerID)
          //   .pipe(take(1))
          //   .subscribe(c => {
          //     this.oprettetAfBrugere = c.map(c => <SelectItem>{ value: c.id, label: c.email });
          this.initFormGroup();

          //this.initRecipeFormGroup();

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
      maxRate: new FormControl(this.smartDevice.maxRate, [Validators.min(0), Validators.required]),
      disableWeekends: new FormControl(this.smartDevice.disableWeekends),
      statusId: new FormControl(this.smartDevice.statusId),
      minTemp: new FormControl(this.smartDevice.minTemp, [Validators.min(5), Validators.max(60), CustomValidators.digitsOnly()]),
      maxRateAtMinTemp: new FormControl(this.smartDevice.maxRateAtMinTemp, [Validators.min(0)]),
      errorMail: new FormControl(this.smartDevice.errorMail, [Validators.email])
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

  public get disableWeekends() {
    return this.mainForm.get("disableWeekends");
  }

  public get statusId() {
    return this.mainForm.get("statusId");
  }

  public get minTemp() {
    return this.mainForm.get("minTemp");
  }

  public get maxRateAtMinTemp() {
    return this.mainForm.get("maxRateAtMinTemp");
  }

  public get errorMail() {
    return this.mainForm.get("errorMail");
  }

  private checkAndValidateForm(formGroup: FormGroup) {
    if (formGroup.invalid) {
      Object.keys(formGroup.controls).forEach(cName => formGroup.controls[cName].markAsTouched());
      this.showFormErrorMessage = true;
      return false;
    }

    this.showFormErrorMessage = false;
    return true;
  }

  getFormValidationErrors(formGroup: FormGroup) {
    console.log("%c ==>> Validation Errors: ", "color: red; font-weight: bold; font-size:25px;");
    let totalErrors = 0;
    Object.keys(formGroup.controls).forEach(key => {
      const controlErrors: ValidationErrors = formGroup.get(key).errors;
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
    if (this.maxRate.value) this.maxRate.setValue(this.maxRate.value.toString().replace(",", "."));
    if (this.maxRateAtMinTemp.value) this.maxRateAtMinTemp.setValue(this.maxRateAtMinTemp.value.toString().replace(",", "."));

    if (!this.checkAndValidateForm(this.mainForm)) return;
    //debugger;
    this.smartDevice.location = this.location.value;
    this.smartDevice.zoneId = this.zoneId.value;
    this.smartDevice.maxRate = +this.maxRate.value;
    this.smartDevice.disableWeekends = this.disableWeekends.value;
    this.smartDevice.statusId = this.statusId.value;
    this.smartDevice.minTemp = this.minTemp.value;
    if (this.maxRateAtMinTemp.value) this.smartDevice.maxRateAtMinTemp = +this.maxRateAtMinTemp.value;
    this.smartDevice.errorMail = this.errorMail.value;

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

  // private initializeRecipes() {
  //   this.recipes$ = this.deviceService.getRecipes(this.activeRoute.parent.snapshot.params.id).pipe(
  //     tap((data: Array<RecipeDto>) => {
  //       data.forEach(element => {
  //         //
  //       });
  //       this.recipes.next(data);
  //     }),
  //     untilDestroyed(this),
  //     finalize(() => (this.loading = false))
  //   );
  // }

  // private initRecipeFormGroup() {
  //   this.recipeForm = new FormGroup({
  //     priority: new FormControl(this.selectedItem.priority, [Validators.required, CustomValidators.digitsOnly()]),
  //     dayTypeId: new FormControl(this.selectedItem.dayTypeId),
  //     maxRate: new FormControl(this.selectedItem.maxRate, [Validators.min(0), Validators.required]),
  //     fromHour: new FormControl(this.selectedItem.fromHour, [Validators.min(0), Validators.max(24), Validators.required]),
  //     toHour: new FormControl(this.selectedItem.toHour, [Validators.min(0), Validators.max(24), Validators.required, CustomValidators.digitsOnly()]),
  //     minHours: new FormControl(this.selectedItem.minHours, [Validators.min(0), Validators.max(24), Validators.required, CustomValidators.digitsOnly()]),
  //     minTemperature: new FormControl(this.selectedItem.minTemperature, [Validators.min(0), Validators.max(100), CustomValidators.digitsOnly()]),
  //     maxRateAtMinTemperature: new FormControl(this.selectedItem.maxRateAtMinTemperature, [Validators.min(0)])
  //   });
  // }

  // public get priority() {
  //   return this.recipeForm.get("priority");
  // }

  // public get dayTypeId() {
  //   return this.recipeForm.get("dayTypeId");
  // }

  // public get maxRate() {
  //   return this.recipeForm.get("maxRate");
  // }

  // public get fromHour() {
  //   return this.recipeForm.get("fromHour");
  // }

  // public get toHour() {
  //   return this.recipeForm.get("toHour");
  // }

  // public get minHours() {
  //   return this.recipeForm.get("minHours");
  // }

  // public get minTemperature() {
  //   return this.recipeForm.get("minTemperature");
  // }

  // public get maxRateAtMinTemperature() {
  //   return this.recipeForm.get("maxRateAtMinTemperature");
  // }

  // onRowSelect() {
  //   this.initRecipeFormGroup();
  //   this.editDialog = true;
  //   //   this.router.navigate(["switchboards", this.activeRoute.parent.snapshot.params.id]);
  // }

  // editItem(item: RecipeDto) {
  //   this.selectedItem = item;
  //   this.onRowSelect();
  // }

  // saveRecipe() {
  //   if (this.maxRate.value) this.maxRate.setValue(this.maxRate.value.toString().replace(",", "."));
  //   if (this.maxRateAtMinTemperature.value) this.maxRateAtMinTemperature.setValue(this.maxRateAtMinTemperature.value.toString().replace(",", "."));

  //   if (!this.checkAndValidateForm(this.recipeForm)) return;

  //   this.selectedItem.priority = this.priority.value;
  //   this.selectedItem.dayTypeId = this.dayTypeId.value;
  //   this.selectedItem.maxRate = +this.maxRate.value;
  //   this.selectedItem.fromHour = this.fromHour.value;
  //   this.selectedItem.toHour = this.toHour.value;
  //   this.selectedItem.minHours = this.minHours.value;
  //   this.selectedItem.minTemperature = this.minTemperature.value;
  //   if (this.maxRateAtMinTemperature.value) this.selectedItem.maxRateAtMinTemperature = +this.maxRateAtMinTemperature.value;

  //   this.deviceService.updateRecipe(this.selectedItem).subscribe({
  //     next: () => {
  //       this.messageService.add({
  //         severity: "success",
  //         summary: "Success",
  //         detail: "Data blev gemt"
  //       });

  //       this.mainForm.reset();
  //       this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/devices", this.smartDevice.id, "edit"]));
  //     },
  //     error: err => (this.errorMessage = err)
  //   });
  // }
}
