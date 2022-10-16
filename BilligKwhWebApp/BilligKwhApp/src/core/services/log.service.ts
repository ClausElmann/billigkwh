import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LogEvent } from "@models/log/Log";
import { LogLevel } from "@models/log/LogLevel";
import { LogSearchModel } from "@models/log/LogSearchModel";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { Observable } from "rxjs";

@Injectable()
export class LogService {
  constructor(private httpClient: HttpClient) {}

  /**
   * For logging an error in server database
   * @param err Either EttpErrorReponse or a default js error with stack and message
   */
  public logJavascriptException(err: HttpErrorResponse | { message: string; stack?: string }) {
    let errorString: string;

    if (err instanceof HttpErrorResponse) {
      errorString += `${(err ).name}: ${err.message}`;
    } else {
      errorString = err.message;
      if (err.stack) errorString += "\n" + err.stack;
    }
    const headers = new HttpHeaders({ "Content-Type": "application/json" }); // IMPORTANT! Angular will set "plain text" as Content Type, but it MUST be json!

    return this.httpClient.post(ApiRoutes.logRoutes.logJavascriptException, JSON.stringify(errorString), {
      headers: headers
    });
  }

  /**
   * Returns all saved logs from db
   */
  public getAllLogs(searchModel: LogSearchModel) {
    return this.httpClient.post<LogEvent[]>(ApiRoutes.logRoutes.getAllLogs, searchModel);
  }

  /**
   * Returns all the different log levels (types)
   */
  public getLogLevels() {
    return this.httpClient.get<LogLevel[]>(ApiRoutes.logRoutes.getLogLevels);
  }

  /**
   * Clears/Deletes all logs
   */
  public clearAllLogs() {
    return this.httpClient.post(ApiRoutes.logRoutes.clearAllLogs, {});
  }
  //---------------------------------------------------------------------------

  public get(id: number): Observable<LogEvent> {
    return this.httpClient.get<LogEvent>(ApiRoutes.logRoutes.getById, {
      params: { id: id.toString() }
    });
  }
  public getAllByDate(fromDate: string, toDate: string): Observable<Array<LogEvent>> {
    return this.httpClient.get<Array<LogEvent>>(ApiRoutes.logRoutes.getAllByDate, {
      params: { fromDate: fromDate, toDate: toDate }
    });
  }
  public getAllByLoglevel(logLevelType: number): Observable<Array<LogEvent>> {
    return this.httpClient.get<Array<LogEvent>>(ApiRoutes.logRoutes.getAllByLoglevel, {
      params: { logLevelType: logLevelType.toString() }
    });
  }
}
