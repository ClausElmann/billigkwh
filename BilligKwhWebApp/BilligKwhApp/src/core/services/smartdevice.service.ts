import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ScheduleDto } from "@apiModels/scheduleDto";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { catchError, map, Observable } from "rxjs";

@Injectable()
export class SmartDeviceService {
  constructor(private http: HttpClient) {}

  public getSmartDevices(countryId?: number, customerId?: number): Observable<SmartDeviceDto[]> {
    const params: { [key: string]: string } = {};

    return this.http
      .get<SmartDeviceDto[]>(ApiRoutes.smartDeviceRoutes.get.getSmartDevices, {
        params: params
      })
      .pipe(
        map(prints => {
          return prints;
        })
      );
  }

  public getSmartDevice(id: number): Observable<SmartDeviceDto> {
    const params: { [key: string]: string } = {};
    params.id = id.toString();

    return this.http.get<SmartDeviceDto>(ApiRoutes.smartDeviceRoutes.get.getSmartDevice, {
      params: params
    });
  }

  public updateSmartDevice(print: SmartDeviceDto) {
    return this.http.post<number>(ApiRoutes.smartDeviceRoutes.update.updateSmartDevice, print).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public getSmagetSchedulesForToday(id: number): Observable<ScheduleDto[]> {
    const params: { [key: string]: string } = {};
    params.deviceId = id.toString();

    return this.http
      .get<ScheduleDto[]>(ApiRoutes.smartDeviceRoutes.get.getSchedulesForToday, {
        params: params
      })
      .pipe(
        map(prints => {
          return prints;
        })
      );
  }
}
