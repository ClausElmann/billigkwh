import { Injectable } from "@angular/core";
import moment from "moment";
@Injectable({
  providedIn: "root"
})
export class BiDateTimeService {
  public isPastSmsLogArchivingLimit(date: moment.Moment): boolean {
    return date.isBefore(moment().startOf("day").subtract(3, "month").utc());
  }
}

