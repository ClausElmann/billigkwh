import { Component, OnInit } from "@angular/core";
import { CustomerService } from "@core/services/customer.service";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { CustomerModel } from "@apiModels/CustomerModel";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem, SortEvent } from "primeng/api";
import { finalize, map, Observable, ReplaySubject, take, tap } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { TableColumnPrimeNg } from "@shared/interfaces-and-enums/TableColumnPrimeNg";
import moment from "moment";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";

export interface CustomerModelExt extends CustomerModel {
  dateForSort?: moment.Moment;
  date?: string;
}

export interface TableColumnPrimeNgExt extends TableColumnPrimeNg {
  sortField?: string;
}

@UntilDestroy()
@Component({
  templateUrl: "./superadmin-customerlist.component.html",
  providers: [MessageService, ConfirmationService]
})
export class SuperAdminCustomerListComponent implements OnInit {
  public loading = true;

  public customers: Array<CustomerModel> = [];
  public customers$: Observable<Array<CustomerModel>>;

  private columns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public columns$ = this.columns.asObservable();

  private globalFilterFields = new ReplaySubject<Array<string>>(1);
  public globalFilterFields$ = this.globalFilterFields.asObservable();

  public countries: Array<SelectItem> = [
    { value: BiCountryId.DK, label: "Danmark" },
    { value: BiCountryId.SE, label: "Sverige" },
    { value: BiCountryId.FI, label: "Finland" },
    { value: BiCountryId.NO, label: "Norge" }
  ];

  public selectedCountryCode: string;

  selectedValue: string;

  private countryId = BiCountryId.DK;

  selectedCustomer: CustomerModel;

  cols: any[];

  showDeleted: boolean;

  constructor(private customerService: CustomerService, private activeRoute: ActivatedRoute, private router: Router, private localizor: BiLocalizationHelperService) {}

  ngOnInit() {
    const currentCustomer = this.customerService.getCurrentStateValue().currentCustomer;
    this.countryId = currentCustomer.countryId;
    this.showDeleted = false;

    this.initializeCustomers();
    this.initColumns();
    // this.cols = [
    //   { field: "id", header: "Id" },
    //   { field: "name", header: "Navn" },
    //   { field: "address", header: "Adresse" },
    //   { field: "companyRegistrationId", header: "Cvr" },
    //   { field: "date", header: "Oprettet", sortField: "dateForSort" }
    // ];
  }

  private initColumns() {
    this.globalFilterFields.next(["name", "address", "companyRegistrationId"]);
    this.columns.next([
      { field: "id", header: "Id" },
      { field: "name", header: "Navn" },
      { field: "address", header: "Adresse" },
      { field: "companyRegistrationId", header: "Cvr" },
      { field: "date", header: "Oprettet", sortField: "dateForSort" }
    ]);
  }

  onRowSelect(event) {
    this.router.navigate([event.data.id, "main"], { relativeTo: this.activeRoute });
  }

  public onCreateNewCustomer() {
    this.router.navigate([0, "main"], { relativeTo: this.activeRoute });
  }

  public selectedCountryCodeChange(item: SelectItem) {
    this.countryId = +item.value;
    this.initializeCustomers();
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeCustomers();
    } else {
      this.showDeleted = false;
      this.initializeCustomers();
    }
  }

  // private initializeCustomers() {
  //   this.customers$ = this.customerService.getCustomers(this.countryId, this.showDeleted).pipe(
  //     untilDestroyed(this),
  //     finalize(() => (this.loading = false))
  //   );
  // }

  private initializeCustomers() {
    this.customers$ = this.customerService.getCustomers(this.countryId, this.showDeleted).pipe(
      tap((data: Array<CustomerModelExt>) => {
        data.forEach(element => {
          element.date = this.localizor.localizeDateTime(element.dateCreatedUtc);
          element.dateForSort = moment(element.dateCreatedUtc);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
  }

  sendMail(messageId: string) {
    if (messageId) {
      this.customerService
        .sendMail(+messageId)
        .pipe(take(1))
        .subscribe(c => {
          console.log(c);
        });
    }
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
