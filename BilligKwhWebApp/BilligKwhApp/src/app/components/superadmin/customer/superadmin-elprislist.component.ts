import { Component, OnInit } from "@angular/core";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem, SortEvent } from "primeng/api";
import { finalize, map, Observable, ReplaySubject, tap } from "rxjs";
import { CustomerService } from "@core/services/customer.service";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import moment from "moment";
import { TableColumnPrimeNg } from "@shared/interfaces-and-enums/TableColumnPrimeNg";
import { ElectricityPriceModel } from "@apiModels/electricityPriceModel";
import { UserService } from "@core/services/user.service";

export interface ElectricityPriceModelExt extends ElectricityPriceModel {
  dateForSort?: moment.Moment;
  date?: string;
}

export interface TableColumnPrimeNgExt extends TableColumnPrimeNg {
  sortField?: string;
}

@UntilDestroy()
@Component({
  templateUrl: "superadmin-elprislist.component.html",
  styleUrls: ["superadmin-elprislist.component.scss"],
  providers: [MessageService, ConfirmationService]
})
export class SuperAdminElectricityPriceListComponent implements OnInit {
  public loading = true;

  public elpriser: Array<ElectricityPriceModel> = [];
  public elpriser$: Observable<Array<ElectricityPriceModel>>;
  private columns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public columns$ = this.columns.asObservable();
  private globalFilterFields = new ReplaySubject<Array<string>>(1);
  public globalFilterFields$ = this.globalFilterFields.asObservable();
  public errorMessage = "";

  public countries: Array<SelectItem> = [
    { value: BiCountryId.DK, label: "Danmark" },
    { value: BiCountryId.SE, label: "Sverige" },
    { value: BiCountryId.FI, label: "Finland" },
    { value: BiCountryId.NO, label: "Norge" }
  ];

  public selectedCountryCode: string;
  public fromDate: Date = new Date();
  public toDate: Date = new Date();

  public nordnetDate: Date = new Date();
  public nordnetData = "";

  indsaetDataDialog = false;

  selectedValue: string;

  private countryId = BiCountryId.DK;

  cols: any[];

  showDeleted: boolean;

  text: string;

  public isSuperAdmin = this.userService.getCurrentStateValue().currentUser.isSuperAdmin;

  constructor(private customerService: CustomerService, private userService: UserService, private localizor: BiLocalizationHelperService, private messageService: MessageService) {}

  ngOnInit() {
    this.text = "<h2>Vælg en email i listen for at se indholdet</h2>";

    this.fromDate.setDate(new Date().getDate());
    this.toDate.setDate(new Date().getDate() + 1);

    this.initializeElectricityPrices();

    this.initColumns();
  }

  public selectedCountryCodeChange(item: SelectItem) {
    this.countryId = +item.value;
    this.initializeElectricityPrices();
  }

  public onDateFilterChanged() {
    this.initializeElectricityPrices();
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeElectricityPrices();
    } else {
      this.showDeleted = false;
      this.initializeElectricityPrices();
    }
  }

  private initColumns() {
    this.globalFilterFields.next(["timeDk", "dk1", "dk2"]);
    this.columns.next([
      { field: "date", header: "Dato", sortField: "dateForSort" },
      { field: "hourDKNo", header: "Time" },
      { field: "dk1", header: "Dk1" },
      { field: "dk2", header: "Dk2" }
    ]);
  }

  private initializeElectricityPrices() {
    this.elpriser$ = this.customerService.getElectricityPrices(this.fromDate, this.toDate).pipe(
      tap((data: Array<ElectricityPriceModelExt>) => {
        data.forEach(element => {
          element.date = this.localizor.localizeDateTime(element.hourUTC);
          element.dateForSort = moment(element.hourUTC);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
  }

  importData() {
    this.customerService.importData(this.nordnetDate, encodeURIComponent(this.nordnetData)).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev importeret"
        });

        this.indsaetDataDialog = false;
      },
      error: err => (this.errorMessage = err)
    });
  }

  customSort(event: SortEvent) {
    this.columns$.pipe(map(columns => columns.find(f => f.field === event.field))).subscribe(col => {
      let value1: any, value2: any;
      event.data.sort((data1, data2) => {
        if (col && col.sortField) {
          value1 = +data1[col.sortField];
          value2 = +data2[col.sortField];
        } else {
          value1 = data1[event.field];
          value2 = data2[event.field];
        }

        let result = null;
        if (value1 == null && value2 != null) {
          result = -1;
        } else if (value1 != null && value2 == null) {
          result = 1;
        } else if (value1 == null && value2 == null) {
          result = 0;
        } else if (typeof value1 === "string" && typeof value2 === "string") {
          result = value1.localeCompare(value2);
        } else {
          result = value1 < value2 ? -1 : value1 > value2 ? 1 : 0;
        }
        return event.order * result;
      });
    });
  }
}
