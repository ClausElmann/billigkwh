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
import { TemperatureReadingDto } from "@apiModels/temperatureReadingDto";

export interface ConsumptionDtoExt extends ConsumptionDto {
  dateSentForSort?: moment.Moment;
  dateSent?: string;
  counter00: number;
  counter23: number;
  consumption: number;
}

export interface TemperatureReadingDtoExt extends TemperatureReadingDto {
  datetimeForSort?: moment.Moment;
  datetime?: string;
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

  public temperatureReadings: Array<TemperatureReadingDto> = [];
  public temperatureReadings$: Observable<Array<TemperatureReadingDto>>;

  public fromDate: Date = new Date();
  public toDate: Date = new Date();

  selectedConsumption: ConsumptionDto;

  private consumptionsColumns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public consumptionsColumns$ = this.consumptionsColumns.asObservable();

  private consumptionsGlobalFilterFields = new ReplaySubject<Array<string>>(1);
  public consumptionsGlobalFilterFields$ = this.consumptionsGlobalFilterFields.asObservable();

  private temperatureReadingsColumns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public temperatureReadingsColumns$ = this.temperatureReadingsColumns.asObservable();

  private temperatureReadingsGlobalFilterFields = new ReplaySubject<Array<string>>(1);
  public temperatureReadingsGlobalFilterFields$ = this.temperatureReadingsGlobalFilterFields.asObservable();

  showDeleted: boolean;

  constructor(private smartDeviceService: SmartDeviceService, private localizor: BiLocalizationHelperService, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    this.fromDate.setDate(new Date().getDate() - 30);

    // this.route.parent.data.subscribe(data => {
    //   this.customer = data["resolvedData"].customer;
    // });
    this.showDeleted = false;

    this.initializeConsumptions();
    this.initializeTemperatureReadings();

    this.initColumns();
  }

  private initColumns() {
    this.consumptionsGlobalFilterFields.next(["subject", "body", "toName", "toEmail"]);
    this.consumptionsColumns.next([
      { field: "dateSent", header: "Dato", sortField: "dateSentForSort" },
      { field: "counter00", header: "Primo" },
      { field: "counter23", header: "Ultimo" },
      { field: "consumption", header: "Forbrug" },
      { field: "c00", header: "00" },
      { field: "c01", header: "01" },
      { field: "c02", header: "02" },
      { field: "c03", header: "03" },
      { field: "c04", header: "04" },
      { field: "c05", header: "05" },
      { field: "c06", header: "06" },
      { field: "c07", header: "07" },
      { field: "c08", header: "08" },
      { field: "c09", header: "09" },
      { field: "c10", header: "10" },
      { field: "c11", header: "11" },
      { field: "c12", header: "12" },
      { field: "c13", header: "13" },
      { field: "c14", header: "14" },
      { field: "c15", header: "15" },
      { field: "c16", header: "16" },
      { field: "c17", header: "17" },
      { field: "c18", header: "18" },
      { field: "c19", header: "19" },
      { field: "c20", header: "20" },
      { field: "c21", header: "21" },
      { field: "c22", header: "22" },
      { field: "c23", header: "23" }
    ]);

    this.temperatureReadingsGlobalFilterFields.next(["datetime", "temperature"]);
    this.temperatureReadingsColumns.next([
      { field: "datetime", header: "Dato", sortField: "datetimeForSort" },
      { field: "temperature", header: "Temperatur" }
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
      this.initializeTemperatureReadings();
    } else {
      this.showDeleted = false;
      this.initializeConsumptions();
      this.initializeTemperatureReadings();
    }
  }

  private initializeTemperatureReadings() {
    this.temperatureReadings$ = this.smartDeviceService.getTemperatureReadingsPeriod(this.activeRoute.parent.snapshot.params.id, this.fromDate, this.toDate).pipe(
      tap((data: Array<TemperatureReadingDtoExt>) => {
        data.forEach(element => {
          element.datetime = this.localizor.localizeDateTime(element.datetimeUtc);
          element.datetimeForSort = moment(element.datetimeUtc);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
  }

  private initializeConsumptions() {
    this.consumptions$ = this.smartDeviceService.getConsumptionsForPeriod(this.activeRoute.parent.snapshot.params.id, this.fromDate, this.toDate).pipe(
      tap((data: Array<ConsumptionDtoExt>) => {
        data.forEach(element => {
          element.dateSent = this.localizor.formatUtcDateTime(element.date, false, true);
          element.dateSentForSort = moment(element.date);
          element.counter00 = element.h00 / 10;
          element.counter23 = element.h23 / 10;

          if (element.h00 > 0 && element.h23 > 0) element.consumption = Math.round(((element.h23 - element.h00) / 10) * 10) / 10;
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
    this.initializeTemperatureReadings();
  }

  customSort(event: SortEvent) {
    this.consumptionsColumns$.pipe(map(columns => columns.find(f => f.field === event.field))).subscribe(col => {
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
