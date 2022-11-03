import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { catchError, map, Observable } from "rxjs";

@Injectable()
export class DeviceService {
  constructor(private http: HttpClient) {}

  public getSmartDevices(countryId?: number, customerId?: number): Observable<SmartDeviceDto[]> {
    const params: { [key: string]: string } = {};

    return this.http
      .get<SmartDeviceDto[]>(ApiRoutes.deviceRoutes.get.getSmartDevices, {
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

    return this.http.get<SmartDeviceDto>(ApiRoutes.deviceRoutes.get.getSmartDevice, {
      params: params
    });
  }

  public updatePrint(print: SmartDeviceDto) {
    return this.http.post<number>(ApiRoutes.deviceRoutes.update.updatePrint, print).pipe(
      catchError(err => {
        throw err;
      })
    );
  }
}
