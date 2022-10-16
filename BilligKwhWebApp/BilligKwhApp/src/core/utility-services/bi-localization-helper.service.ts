import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import moment from "moment-timezone";
import { Moment as MomentNormal } from "moment";

import { filter, map } from "rxjs/operators";
import { BiCountryId, BiLanguageId } from "@globals/enums/BiLanguageAndCountryId";
import {
  landLineLength_SE,
  phoneLength_DK,
  phoneLength_FI,
  phoneLength_SE,
  landLineLength_FI,
  phoneLength_NO
} from "@globals/classes/global-constants";
import { isInsideIFrameOrPopup, isStringNullOrEmpty } from "@shared/variables-and-functions/helper-functions";
import { convertWindowsTimeZoneToIANA } from "@shared/variables-and-functions/timezoneTypeConversion";
import { CustomerService } from "../services/customer.service";
import { UserService } from "../services/user.service";

export interface LanguageConfig {
  dateFormat: string;
  dateTimeFormat: string;
  dateFormatNoYear: string;
  phoneCountryCode: number;
  phoneLength: number;

  /**
   * Min length of landline numbers
   */
  phoneLengthLandlineMin: number;
  /**
   * Max length of land line numbers
   */
  phoneLengthLandlineMax: number;
  minimumPhoneLength?: number;

  /**
   * Max. zip code length. For instance, Some can be up to 5
   */
  zipcodeLength: number;
  defaultMapCenterLatitude: number;
  defaultMapCenterLongitude: number;
  countryCode: string;
}

/**
 * Helper service with methods for localization stuff. This includes formatting of date times based on customer/profile timezone,
 * phone number country prefix and phone number length requirement (depending on country and language).
 * NOTE: This class uses Moment Timezone in order to use specific time zone depending on user or customer.
 *
 */
@Injectable()
export class BiLocalizationHelperService {
  /**
   * Whether the current profile has a time zone id specified. To know if we should use customer's instead.
   */
  private profileHasTimeZone = false;

  ////======  Public properties with language and country specific information to be used all over web app ============
  public dateFormat = "DD/MM/YYYY";
  public dateFormatNoYear = "DD/MM";
  public dateTimeFormat = "DD/MM/YYYY HH:mm";
  public dateTimeSecondsFormat = this.dateTimeFormat + ":ss";

  /**
   * Phone number prefix determined by the country of the customer
   */
  public phoneCountryCode = 45;

  /**
   * How long phone numbers should be. Default 8 (DK)
   */
  public phoneLength = 8;
  /**
   * How long zipcodes should be
   */
  public zipcodeLength = 4;
  /**
   * Is language Danish? This is based on current USER.
   */
  public isDanishLang = true;

  /**
   * This is based on current USER language Setting.
   */
  public currentUserLanguageId = 1;

  /**
   * Country id of the customer. Default DK
   */
  public customerCountryId: BiCountryId = BiCountryId.DK;
  public customerCountryCode = "da";

  public defaultMapCenterLatitude = 56.2639;
  public defaultMapCenterLongitude = 9.5018;
  public defaultZoomLevel = 7;
  ////==========================================

  constructor(
    private customerService: CustomerService,
    private userService: UserService,
    private translator: TranslateService
  ) {
    moment.tz.setDefault("Europe/Copenhagen"); // now everytime we create moment date, it's treated with this timezone

    if (isInsideIFrameOrPopup()) return;

     this.initCurrentCustomerChangeSubscription();

    this.initUserLanguageUpdateSubscription();
  }

  private initCurrentCustomerChangeSubscription() {
    // when customer changes, so too can the time zone id and country id
    this.customerService.state$
      .pipe(
        map((state) => state.currentCustomer),
        filter((c) => c != null)
      )
      .subscribe((customer) => {
        // For time zone, profile comes first - check that it doesn't have a time zone set
        if (!this.profileHasTimeZone) moment.tz.setDefault(convertWindowsTimeZoneToIANA(customer.timeZoneId));

        // if country id has changed
        if (this.customerCountryId !== customer.countryId) {
          this.customerCountryId = customer.countryId;

          switch (customer.countryId) {
            case BiCountryId.DK:
              this.customerCountryCode = "da";
              this.phoneCountryCode = 45;
              this.phoneLength = 8;
              this.zipcodeLength = 4;
              this.defaultMapCenterLatitude = 56.2639;
              this.defaultMapCenterLongitude = 9.5018;
              break;

            case BiCountryId.SE:
              this.phoneCountryCode = 46;
              this.customerCountryCode = "sv";
              this.phoneLength = 7; // minimum length in Sweden
              this.zipcodeLength = 5;
              this.defaultMapCenterLatitude = 60.12816;
              this.defaultMapCenterLongitude = 18.6435;
              break;

            case BiCountryId.FI:
              this.phoneCountryCode = 358;
              this.customerCountryCode = "fi";
              this.phoneLength = 9; // without country code and "9" in front (used in Finland)
              this.zipcodeLength = 5;
              this.defaultMapCenterLatitude = 60.12816;
              this.defaultMapCenterLongitude = 18.6435;
              break;

            case BiCountryId.NO:
              this.customerCountryCode = "no";
              this.phoneCountryCode = 47;
              this.phoneLength = 8;
              this.zipcodeLength = 4;
              this.defaultMapCenterLatitude = 65.4465118;
              this.defaultMapCenterLongitude = 16.7209846;
              break;
          }
        }
      });
  }

  private initUserLanguageUpdateSubscription() {
    // Subscribe to changes in user current language
    this.userService.state$.pipe(map((s) => s.currentLanguageId)).subscribe((langId) => {
      switch (langId) {
        case BiLanguageId.DK:
          this.translator.use("da");
          this.dateFormat = "DD/MM/YYYY";
          this.dateFormatNoYear = "DD/MM";
          this.dateTimeFormat = "DD/MM/YYYY HH:mm";
          this.isDanishLang = true;
          this.currentUserLanguageId = BiLanguageId.DK;
          moment.locale("da");
          document.documentElement.lang = "da";

          break;

        case BiLanguageId.SE:
          this.translator.use("se");
          this.dateFormat = "YYYY-MM-DD";
          this.dateFormatNoYear = "MM-DD";
          this.dateTimeFormat = "YYYY-MM-DD HH:mm";
          this.isDanishLang = false;
          this.currentUserLanguageId = BiLanguageId.SE;
          moment.locale("sv");
          document.documentElement.lang = "se";

          break;

        case BiLanguageId.EN:
          this.translator.use("en");
          this.dateFormat = "DD/MM/YYYY";
          this.dateFormatNoYear = "DD/MM";
          this.dateTimeFormat = "DD/MM/YYYY HH:mm";
          this.isDanishLang = false;
          this.currentUserLanguageId = BiLanguageId.EN;
          moment.locale("en");
          document.documentElement.lang = "en";
          break;

        case BiLanguageId.FI:
          this.translator.use("fi");
          this.dateFormat = "DD/MM/YYYY";
          this.dateFormatNoYear = "DD/MM";
          this.dateTimeFormat = "DD/MM/YYYY HH:mm";
          this.isDanishLang = false;
          this.currentUserLanguageId = BiLanguageId.FI;
          moment.locale("fi");
          document.documentElement.lang = "fi";
          break;

        case BiLanguageId.NO:
          this.translator.use("no");
          this.dateFormat = "DD/MM/YYYY";
          this.dateFormatNoYear = "DD/MM";
          this.dateTimeFormat = "DD/MM/YYYY HH:mm";
          this.isDanishLang = false;
          this.currentUserLanguageId = BiLanguageId.NO;
          moment.locale("no");
          document.documentElement.lang = "no";
          break;
      }
    });
  }

  public setLanguage(language: string) {
    this.translator.use(language);
  }
  //#region DATE TIME LOCALIZATION (note: all moment objects will be treated with correct time zone as we use the moment.tz.setDefault above)
  /**
   * Translates a native JS Date or Moment object to a formatted string based on language
   */
  public localizeDateTime(theDate: string | MomentNormal, showSeconds = false): string {
    const format = this.dateTimeFormat + (showSeconds ? ":ss" : "");
    if (theDate) {
      // check if already a moment object
      if (moment.isMoment(theDate)) return theDate.format(format); // we call "toUpper()" as the format is in lowercase (for the datepicker) but Moment Js expects upper case

      return moment(theDate).format(format);
    }
    return "";
  }

  /**
   * Takes UTC string or Moment UTC instance and formats it for display.
   * @param dateTimeStringUtc Date string in UTC format OR a Moment Js UTC instance
   * @param toDefaultDate Set true if you ONLY want a date output and use the default value (depending on customer or profile country).
   * @param toDefaultTime Set true if you ONLY want a time output and use the default value "HH:mm"
   * @param outputFormat Use this if want full control of the output format. Must be a Moment Js compatible format. Examples:
   * "DD/MM/YYYY", "HH:mm", "DD MM YY" etc.
   */
  public formatUtcDateTime(dateTimeStringUtc: string, toDefaultTime?: boolean, toDefaultDate?: boolean, outputFormat?: string): string {
    if (dateTimeStringUtc) {
      if (toDefaultTime) return moment(dateTimeStringUtc).format("HH:mm");
      if (toDefaultDate) return moment(dateTimeStringUtc).format(this.dateFormat);
      if (outputFormat) return moment(dateTimeStringUtc).format(outputFormat);
      return moment(dateTimeStringUtc).format();
    }
    return "";
  }

  /**
   * Converts a formated date time string into a correct ISO 8601 UTC date time string - ready to send to server.
   * @param dateTimeString A date time string in the format DD/MM/YYYY HH:mm (or YYYY-MM-DD HH:mm if Swedish).
   * @param inputFormat The date time format. This string must be a format conforming to Moment Js compatible date times like "DD/MM/YYYY HH:mm" or just
   * the date part. Defaults to the general format in customer's country
   */
  public formatedDateTimeStringToUtcString(dateTimeString: string, inputFormat?: string): string {
    if (isStringNullOrEmpty(dateTimeString)) return null;
    const momentDateTime = moment(dateTimeString, inputFormat ? inputFormat : this.dateTimeFormat);
    if (momentDateTime.isValid()) {
      return momentDateTime.toISOString();
    } else return null;
  }

  /**
   * Converts a number of milliseconds since UTC/UNIX epoch to a date string in either DK or SE form. We always convert using the
   *  time zone id of current customer (which is first converted to the IANA time zone equivalent).
   */
  public numberToLocalizedDate(msSinceEpoch: number): string {
    return moment(msSinceEpoch).format(this.dateFormat);
  }

  //#endregion

  //#region HELPERS
  /**
   * Returns the default lat long coordinates based on country. Returned as a tuple like this [lat, long]
   */
  public getlatLongByCountry(countryId: BiCountryId): [number, number] {
    switch (countryId) {
      case BiCountryId.DK:
        return [56.2639, 9.5018];

      case BiCountryId.SE:
        return [57.21961, 12.12891];

      case BiCountryId.FI:
        return [65.9459988, 21.551244];
      case BiCountryId.NO:
        return [65.4465118, 16.7209846];
    }
  }

  /**
   * Returns language configurations based on a language id. By default, Danish countryId is used (if null or undefined
   * is passed).
   */
  public getLanguageConfigByCountry(countryId: BiCountryId): LanguageConfig {
    if (!countryId) countryId = BiCountryId.DK;

    switch (countryId) {
      case BiCountryId.DK:
        return {
          dateFormat: "DD/MM/YYYY",
          dateFormatNoYear: "DD/MM",
          dateTimeFormat: "DD/MM/YYYY HH:mm",
          phoneCountryCode: 45,
          minimumPhoneLength: 8,
          phoneLength: 8,
          phoneLengthLandlineMin: phoneLength_DK,
          phoneLengthLandlineMax: phoneLength_DK,
          zipcodeLength: 4,
          defaultMapCenterLatitude: 56.2639,
          defaultMapCenterLongitude: 9.5018,
          countryCode: "dk"
        };
      case BiCountryId.SE:
        return {
          dateFormat: "YYYY-MM-DD",
          dateFormatNoYear: "MM-DD",
          dateTimeFormat: "YYYY-MM-DD HH:mm",
          phoneCountryCode: 46,
          minimumPhoneLength: phoneLength_SE[0],
          phoneLength: phoneLength_SE[1],
          phoneLengthLandlineMin: landLineLength_SE[0],
          phoneLengthLandlineMax: landLineLength_SE[1],
          zipcodeLength: 5,
          defaultMapCenterLatitude: 60.12816,
          defaultMapCenterLongitude: 18.6435,
          countryCode: "se"
        };
      case BiCountryId.EN:
        return {
          dateFormat: "DD/MM/YYYY",
          dateFormatNoYear: "DD/MM",
          dateTimeFormat: "DD/MM/YYYY HH:mm",
          phoneCountryCode: 44, // Change to England
          phoneLength: 8, // Change to England
          phoneLengthLandlineMin: 8, // Change to England
          phoneLengthLandlineMax: 10, // Change to England
          zipcodeLength: 4, // Change to England
          defaultMapCenterLatitude: 56.2639, // Change to England
          defaultMapCenterLongitude: 9.5018, // Change to England
          countryCode: "en"
        };
      case BiCountryId.FI:
        return {
          dateFormat: "DD/MM/YYYY",
          dateFormatNoYear: "DD/MM",
          dateTimeFormat: "DD/MM/YYYY HH:mm",
          phoneCountryCode: 358,
          minimumPhoneLength: phoneLength_FI[0],
          phoneLength: phoneLength_FI[1],
          phoneLengthLandlineMin: landLineLength_FI[0],
          phoneLengthLandlineMax: landLineLength_FI[1],
          zipcodeLength: 5,
          defaultMapCenterLatitude: 64.6233799,
          defaultMapCenterLongitude: 23.8364001,
          countryCode: "fi"
        };
      case BiCountryId.NO:
        return {
          dateFormat: "YYYY-MM-DD",
          dateFormatNoYear: "MM-DD",
          dateTimeFormat: "YYYY-MM-DD HH:mm",
          phoneCountryCode: 47,
          minimumPhoneLength: phoneLength_NO,
          phoneLength: phoneLength_NO,
          phoneLengthLandlineMin: phoneLength_NO,
          phoneLengthLandlineMax: phoneLength_NO,
          zipcodeLength: 4,
          defaultMapCenterLatitude: 65.4465118,
          defaultMapCenterLongitude: 16.7209846,
          countryCode: "no"
        };
    }
  }

  /**
   * Takes a phone number either as a string or a number and returns the formated version for display.
   */
  public getFormatedPhoneNumber(phoneNumber?: number, phoneNumberString?: string): string {
    if (phoneNumber) {
      if (this.customerCountryId !== BiCountryId.DK && this.customerCountryId !== BiCountryId.NO)
        return phoneNumber.toString()[0] !== "0" ? "0" + phoneNumber.toString() : phoneNumber.toString();
      else return phoneNumber.toString();
    } else if (!isStringNullOrEmpty(phoneNumberString)) {
      if (this.customerCountryId !== BiCountryId.DK && this.customerCountryId !== BiCountryId.NO)
        return phoneNumberString[0] !== "0" ? "0" + phoneNumberString : phoneNumberString; // there should be 1 leading 0 (as agreed with Flemming). Dashes are removed elsewhere

      return phoneNumberString;
    }
    return "";
  }


  /*
   * Return a Country Or Language Name based on its Id.
   */
  public getLanguageName(languageId: number | string): string {
    if (languageId === BiCountryId.DK || languageId === BiCountryId.DK.toString()) {
      return "Danmark";
    }
    if (languageId === BiCountryId.SE || languageId === BiCountryId.SE.toString()) {
      return "Sverige";
    }
    if (languageId === BiCountryId.EN || languageId === BiCountryId.EN.toString()) {
      return "England";
    }
    if (languageId === BiCountryId.FI || languageId === BiCountryId.FI.toString()) {
      return "Finland";
    }
    if (languageId === BiCountryId.NO || languageId === BiCountryId.NO.toString()) {
      return "Norge";
    }
  }

  public getCountryName(countryId: BiCountryId) {
    switch (countryId) {
      case BiCountryId.DK:
        return this.translator.instant("shared.Denmark");

      case BiCountryId.SE:
        return this.translator.instant("shared.Sweden");

      case BiCountryId.EN:
        return this.translator.instant("shared.England");

      case BiCountryId.FI:
        return this.translator.instant("shared.Finland");

      case BiCountryId.NO:
        return this.translator.instant("shared.Norway");

      default: return 0;
    }
  }

  public getAllMonths() {
    const calender = [];
    for (let i = 1; i < 13; i++) {
      calender.push(this.getMonthName(i));
    }
    return calender;
  }

  public getMonthName(monthNumber: number) {
    switch (monthNumber) {
      case (1): return this.translator.instant("shared.Month.January");
      case (2): return this.translator.instant("shared.Month.February");
      case (3): return this.translator.instant("shared.Month.March");
      case (4): return this.translator.instant("shared.Month.April");
      case (5): return this.translator.instant("shared.Month.May");
      case (6): return this.translator.instant("shared.Month.June");
      case (7): return this.translator.instant("shared.Month.July");
      case (8): return this.translator.instant("shared.Month.August");
      case (9): return this.translator.instant("shared.Month.September");
      case (10): return this.translator.instant("shared.Month.October");
      case (11): return this.translator.instant("shared.Month.November");
      case (12): return this.translator.instant("shared.Month.December");
      default: return "N/A";
    }
  }

  public getDurationString(data: any): any {
    const minuteValue = data as number;
    const totalMinutes = Math.trunc(minuteValue);
    const seconds = Math.trunc((minuteValue - totalMinutes) * 60);
    const minutes = totalMinutes % 60;
    const hours = Math.floor(totalMinutes / 60) % 24;
    const days = Math.floor(totalMinutes / 1440);

    return ((days > 0 ? days.toString() + " " + this.translator.instant("shared.Days") + " " : "") + (hours > 0 ? hours.toString() + " " + this.translator.instant("shared.Hours") + " " : "") + (minutes > 0 ? minutes.toString() + " " + this.translator.instant("shared.Minutes") + " " : "") + (seconds > 0 ? seconds.toString() + " " + this.translator.instant("shared.Seconds") : "")).trim();
  }

  //#endregion


}
