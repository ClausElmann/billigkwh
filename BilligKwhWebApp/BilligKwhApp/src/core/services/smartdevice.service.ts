import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ConsumptionDto } from "@apiModels/consumptionDto";
import { ScheduleDto } from "@apiModels/scheduleDto";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";
import { TemperatureReadingDto } from "@apiModels/temperatureReadingDto";
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

  public getConsumptionsForPeriod(deviceId: number, fromDateUtc: Date, toDateUtc: Date): Observable<ConsumptionDto[]> {
    const params: { [key: string]: string } = {
      deviceId: deviceId.toString(),
      fromDateUtc: fromDateUtc.toDateString(),
      toDateUtc: toDateUtc.toDateString()
    };

    //if (customerId) params["customerId"] = customerId.toString();

    return this.http
      .get<ConsumptionDto[]>(ApiRoutes.smartDeviceRoutes.get.getConsumptionsPeriod, {
        params: params
      })
      .pipe(
        map(consumptions => {
          return consumptions;
        })
      );
  }

  public getTemperatureReadingsPeriod(deviceId: number, fromDateUtc: Date, toDateUtc: Date): Observable<TemperatureReadingDto[]> {
    const params: { [key: string]: string } = {
      deviceId: deviceId.toString(),
      fromDateUtc: fromDateUtc.toDateString(),
      toDateUtc: toDateUtc.toDateString()
    };

    //if (customerId) params["customerId"] = customerId.toString();

    return this.http
      .get<TemperatureReadingDto[]>(ApiRoutes.smartDeviceRoutes.get.getTemperatureReadingsPeriod, {
        params: params
      })
      .pipe(
        map(consumptions => {
          return consumptions;
        })
      );
  }

  // public getRecipes(deviceId: number): Observable<RecipeDto[]> {
  //   const params: { [key: string]: string } = {};
  //   params.deviceId = deviceId.toString();

  //   return this.http
  //     .get<RecipeDto[]>(ApiRoutes.smartDeviceRoutes.get.getRecipes, {
  //       params: params
  //     })
  //     .pipe(
  //       map(recipes => {
  //         return recipes;
  //       })
  //     );
  // }

  // public updateRecipe(recipe: RecipeDto) {
  //   return this.http.post<number>(ApiRoutes.smartDeviceRoutes.update.updateRecipe, recipe).pipe(
  //     catchError(err => {
  //       throw err;
  //     })
  //   );
  // }
}
