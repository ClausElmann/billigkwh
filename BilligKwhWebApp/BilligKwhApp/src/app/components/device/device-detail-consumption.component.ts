import { Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SortEvent } from "primeng/api";
import { finalize, map, Observable, ReplaySubject, tap } from "rxjs";
import { ActivatedRoute } from "@angular/router";
import { ConsumptionDto } from "@apiModels/consumptionDto";
import { SmartDeviceService } from "@core/services/smartdevice.service";
import moment from "moment";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import { TableColumnPrimeNgExt } from "./devicelist.component";

export interface ConsumptionDtoExt extends ConsumptionDto {
  dateSentForSort?: moment.Moment;
  dateSent?: string;
}

@UntilDestroy()
@Component({
  selector: "app-device-detail-consumption",
  templateUrl: "./device-detail-consumption.component.html",
  providers: [MessageService, ConfirmationService]
})
export class DeviceDetailConsumptionComponent implements OnInit {
  public loading = true;

  public imageSpinner = false;
  public displayImageDialog: boolean;

  //customer: CustomerModel;
  public consumptions: Array<ConsumptionDto> = [];
  public consumptions$: Observable<Array<ConsumptionDto>>;

  public fromDate: Date = new Date();
  public toDate: Date = new Date();

  selectedConsumption: ConsumptionDto;

  private columns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public columns$ = this.columns.asObservable();

  private globalFilterFields = new ReplaySubject<Array<string>>(1);
  public globalFilterFields$ = this.globalFilterFields.asObservable();

  showDeleted: boolean;

  constructor(private smartDeviceService: SmartDeviceService, private localizor: BiLocalizationHelperService, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    this.fromDate.setDate(new Date().getDate() - 30);

    // this.route.parent.data.subscribe(data => {
    //   this.customer = data["resolvedData"].customer;
    // });
    this.showDeleted = false;

    this.initializeConsumptions();

    this.initColumns();
  }

  private initColumns() {
    this.globalFilterFields.next(["subject", "body", "toName", "toEmail"]);
    this.columns.next([
      { field: "dateSent", header: "Sendt", sortField: "dateSentForSort" },
      { field: "h00", header: "00" },
      { field: "h01", header: "01" },
      { field: "h02", header: "02" },
      { field: "h03", header: "03" },
      { field: "h04", header: "04" },
      { field: "h05", header: "05" },
      { field: "h06", header: "06" },
      { field: "h07", header: "07" },
      { field: "h08", header: "08" },
      { field: "h09", header: "09" },
      { field: "h10", header: "10" },
      { field: "h11", header: "11" },
      { field: "h12", header: "12" },
      { field: "h13", header: "13" },
      { field: "h14", header: "14" },
      { field: "h15", header: "15" },
      { field: "h16", header: "16" },
      { field: "h17", header: "17" },
      { field: "h18", header: "18" },
      { field: "h19", header: "19" },
      { field: "h20", header: "20" },
      { field: "h21", header: "21" },
      { field: "h22", header: "22" },
      { field: "h23", header: "23" }
    ]);
  }

  onRowSelect(event) {
    this.displayImageDialog = true;
    //this.router.navigate(["users", event.data.id, "edit"]);
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeConsumptions();
    } else {
      this.showDeleted = false;
      this.initializeConsumptions();
    }
  }

  private initializeConsumptions() {
    this.consumptions$ = this.smartDeviceService.getConsumptionsForPeriod(this.activeRoute.parent.snapshot.params.id, this.fromDate, this.toDate).pipe(
      tap((data: Array<ConsumptionDtoExt>) => {
        data.forEach(element => {
          element.dateSent = this.localizor.localizeDateTime(element.date);
          element.dateSentForSort = moment(element.date);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
  }

  // private initializeEmails() {
  //   this.emails$ = this.customerService.getEmails(this.fromDate, this.toDate, null).pipe(
  //     tap((data: Array<EmailModelExt>) => {
  //       data.forEach(element => {
  //         element.dateSent = this.localizor.localizeDateTime(element.dateSentUtc);
  //         element.dateSentForSort = moment(element.dateSentUtc);
  //       });
  //     }),
  //     untilDestroyed(this),
  //     finalize(() => {
  //       this.loading = false;
  //     })
  //   );
  // }

  public onDateFilterChanged() {
    this.initializeConsumptions();
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
