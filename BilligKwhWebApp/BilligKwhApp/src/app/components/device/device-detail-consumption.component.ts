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
  price00: number;
  price01: number;
  price02: number;
  price03: number;
  price04: number;
  price05: number;
  price06: number;
  price07: number;
  price08: number;
  price09: number;
  price10: number;
  price11: number;
  price12: number;
  price13: number;
  price14: number;
  price15: number;
  price16: number;
  price17: number;
  price18: number;
  price19: number;
  price20: number;
  price21: number;
  price22: number;
  price23: number;
}

export interface TemperatureReadingDtoExt extends TemperatureReadingDto {
  datetimeForSort?: moment.Moment;
  datetime?: string;
  running?: string;
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
      { field: "c23", header: "23" },
      { field: "p00", header: "P00" },
      { field: "p01", header: "P01" },
      { field: "p02", header: "P02" },
      { field: "p03", header: "P03" },
      { field: "p04", header: "P04" },
      { field: "p05", header: "P05" },
      { field: "p06", header: "P06" },
      { field: "p07", header: "P07" },
      { field: "p08", header: "P08" },
      { field: "p09", header: "P09" },
      { field: "p10", header: "P10" },
      { field: "p11", header: "P11" },
      { field: "p12", header: "P12" },
      { field: "p13", header: "P13" },
      { field: "p14", header: "P14" },
      { field: "p15", header: "P15" },
      { field: "p16", header: "P16" },
      { field: "p17", header: "P17" },
      { field: "p18", header: "P18" },
      { field: "p19", header: "P19" },
      { field: "p20", header: "P20" },
      { field: "p21", header: "P21" },
      { field: "p22", header: "P22" },
      { field: "p23", header: "P23" }
    ]);

    this.temperatureReadingsGlobalFilterFields.next(["datetime", "temperature"]);
    this.temperatureReadingsColumns.next([
      { field: "datetime", header: "Dato", sortField: "datetimeForSort" },
      { field: "temperature", header: "Temperatur" },
      { field: "running", header: "Kører nu" }
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
          element.running = element.isRunning ? "Ja" : "Nej";
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

        //  element.price00 = Math.round((element.c00 * element.p00 + Number.EPSILON) * 100) / 100;
        //  element.price01 = Math.round((element.c01 * element.p01 + Number.EPSILON) * 100) / 100;
        //  element.price02 = Math.round((element.c02 * element.p02 + Number.EPSILON) * 100) / 100;
        //  element.price03 = Math.round((element.c03 * element.p03 + Number.EPSILON) * 100) / 100;
        //  element.price04 = Math.round((element.c04 * element.p04 + Number.EPSILON) * 100) / 100;
        //  element.price05 = Math.round((element.c05 * element.p05 + Number.EPSILON) * 100) / 100;
        //  element.price06 = Math.round((element.c06 * element.p06 + Number.EPSILON) * 100) / 100;
        //  element.price07 = Math.round((element.c07 * element.p07 + Number.EPSILON) * 100) / 100;
        //  element.price08 = Math.round((element.c08 * element.p08 + Number.EPSILON) * 100) / 100;
        //  element.price09 = Math.round((element.c09 * element.p09 + Number.EPSILON) * 100) / 100;
        //  element.price10 = Math.round((element.c10 * element.p10 + Number.EPSILON) * 100) / 100;
        //  element.price11 = Math.round((element.c11 * element.p11 + Number.EPSILON) * 100) / 100;
        //  element.price12 = Math.round((element.c12 * element.p12 + Number.EPSILON) * 100) / 100;
        //  element.price13 = Math.round((element.c13 * element.p13 + Number.EPSILON) * 100) / 100;
        //  element.price14 = Math.round((element.c14 * element.p14 + Number.EPSILON) * 100) / 100;
        //  element.price15 = Math.round((element.c15 * element.p15 + Number.EPSILON) * 100) / 100;
        //  element.price16 = Math.round((element.c16 * element.p16 + Number.EPSILON) * 100) / 100;
        //  element.price17 = Math.round((element.c17 * element.p17 + Number.EPSILON) * 100) / 100;
        //  element.price18 = Math.round((element.c18 * element.p18 + Number.EPSILON) * 100) / 100;
        //  element.price19 = Math.round((element.c19 * element.p19 + Number.EPSILON) * 100) / 100;
        //  element.price20 = Math.round((element.c20 * element.p20 + Number.EPSILON) * 100) / 100;
        //  element.price21 = Math.round((element.c21 * element.p21 + Number.EPSILON) * 100) / 100;
        //  element.price22 = Math.round((element.c22 * element.p22 + Number.EPSILON) * 100) / 100;
        //  element.price23 = Math.round((element.c23 * element.p23 + Number.EPSILON) * 100) / 100;

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
