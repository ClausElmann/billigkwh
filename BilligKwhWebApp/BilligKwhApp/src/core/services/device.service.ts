import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PrintDto } from "@apiModels/printDto";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { catchError, map, Observable } from "rxjs";

@Injectable()
export class DeviceService {
  constructor(private http: HttpClient) {}

  public getPrints(countryId?: number, customerId?: number): Observable<PrintDto[]> {
    const params: { [key: string]: string } = {};

    return this.http
      .get<PrintDto[]>(ApiRoutes.deviceRoutes.get.getPrints, {
        params: params
      })
      .pipe(
        map(prints => {
          return prints;
        })
      );
  }

  public getPrint(id: number): Observable<PrintDto> {
    const params: { [key: string]: string } = {};
    params.id = id.toString();

    return this.http.get<PrintDto>(ApiRoutes.deviceRoutes.get.getPrint, {
      params: params
    });
  }

  public updatePrint(print: PrintDto) {
    return this.http.post<number>(ApiRoutes.deviceRoutes.update.updatePrint, print).pipe(
      catchError(err => {
        throw err;
      })
    );
  }
}
