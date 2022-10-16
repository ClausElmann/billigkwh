import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";
import moment from "moment-timezone";
import { isDigitsOnly, parseFormatedDateStringToMoment } from "@shared/variables-and-functions/helper-functions";
import { BiLocalizationHelperService } from "./bi-localization-helper.service";
import { BiLanguageId } from "@globals/enums/BiLanguageAndCountryId";

/**
 * Service used for setting up Pickadate js: http://amsul.ca/pickadate.js/ and
 * Angular 2 Input Mask (https://github.com/text-mask/text-mask/tree/master/angular2#readme)
 */
@Injectable()
export class BiDateTimePickerConfigService {
  constructor(private localizationHelper: BiLocalizationHelperService) { }

  /**
   * Returns date picker configuration.
   * @returns
   */
  public getGeneralDatepickerConfig(): Pickadate.DateOptions {
    const config: Pickadate.DateOptions = {
      selectYears: 20,
      selectMonths: true,
      clear: "",
      closeOnSelect: true,
      editable: true,
      firstDay: 1,
      min: false,
      max: false
    };

    // Danish
    if (this.localizationHelper.currentUserLanguageId === BiLanguageId.DK) {
      config.format = "dd/mm/yyyy";
      config.formatSubmit = "dd/mm/yyyy";
      config.today = "I dag";
      config.close = "Luk";
      config.monthsFull = ["Januar", "Februar", "Marts", "April", "Maj", "Juni", "Juli", "August", "September", "Oktober", "November", "December"];
      config.monthsShort = ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"];
      config.weekdaysFull = ["Søndag", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag"];
      config.weekdaysShort = ["Søn", "Man", "Tirs", "Ons", "Tors", "Fre", "Lør"];
    }
    // Swedish
    else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.SE) {
      config.format = "yyyy-mm-dd";
      config.formatSubmit = "yyyy-mm-dd";
      config.today = "Idag";
      config.close = "Stäng";
      config.monthsFull = ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"];
      config.monthsShort = ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"];
      config.weekdaysFull = ["Söndag", "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag"];
      config.weekdaysShort = ["Sön", "Mån", "Tis", "Ons", "Tor", "Fre", "Lör"];
    }
    // English
    else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.EN) {
      config.format = "dd/mm/yyyy";
      config.formatSubmit = "dd/mm/yyyy";
      config.today = "Today";
      config.close = "Close";
      config.monthsFull = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
      config.monthsShort = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
      config.weekdaysFull = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
      config.weekdaysShort = ["Sun", "Mon", "Tues", "Wed", "Thurs", "Fri", "Sat"];
    }
    // Finnish
    else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.FI) {
      config.format = "dd/mm/yyyy";
      config.formatSubmit = "dd/mm/yyyy";
      config.today = "Tänään";
      config.close = "Sulje";
      config.monthsFull = [
        "Tammikuu",
        "Helmikuu",
        "Maaliskuu",
        "Huhtikuu",
        "Toukokuu",
        "Kesäkuu",
        "Heinäkuu",
        "Elokuu",
        "Syyskuu",
        "Lokakuu",
        "Marraskuu",
        "Joulukuu"
      ];
      config.monthsShort = ["Tammi", "Helmi", "Maali", "Huhti", "Touku", "Kesä", "Heinä", "Elo", "Syys", "Loka", "Marras", "Joulu"];
      config.weekdaysFull = ["Sunnuntai", "Maanantai", "Tiistai", "Keskiviikko", "Torstai", "Perjantai", "Lauantai"];
      config.weekdaysShort = ["Su", "Ma", "Ti", "Ke", "To", "Pe", "La"];
    }
    // Norwegian
    if (this.localizationHelper.currentUserLanguageId === BiLanguageId.NO) {
      config.format = "dd/mm/yyyy";
      config.formatSubmit = "dd/mm/yyyy";
      config.today = "Idag";
      config.close = "Lukk";
      config.monthsFull = ["Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Desember"];
      config.monthsShort = ["Jan", "Feb", "Mar", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Des"];
      config.weekdaysFull = ["Søndag", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag"];
      config.weekdaysShort = ["Søn", "Man", "Tirs", "Ons", "Tors", "Fre", "Lør"];
    }
    return config;
  }

  /**
   * Returns mask configuration for a date input that's using the Angular 2 Input Mask library
   */
  public getDateInputMaskConfig(): {
    mask: (rawValue) => Array<any>;
    placeholderChar: string;
    keepCharPositions: boolean;
  } {
    return {
      mask: (rawValue: string) => {
        let defaultMask;

        // Danish & Finnish
        if (this.localizationHelper.currentUserLanguageId === BiLanguageId.DK ||
          this.localizationHelper.currentUserLanguageId === BiLanguageId.FI ||
          this.localizationHelper.currentUserLanguageId === BiLanguageId.NO) {
          defaultMask = [/[0-3]/, /[0-9]/, "/", /[0-1]/, /[0-9]/, "/", /[2]/, /[0-1]/, /[0-9]/, /[0-9]/];
        }
        // Swedish
        else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.SE) {
          defaultMask = [/[2]/, /[0-1]/, /[0-9]/, /[0-9]/, "-", /[0-1]/, /[0-9]/, "-", /[0-3]/, /[0-9]/];
        }
        // English
        else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.EN) {
          defaultMask = [/[0-3]/, /[0-9]/, "/", /[0-1]/, /[0-9]/, "/", /[2]/, /[0-1]/, /[0-9]/, /[0-9]/];
        }

        // Remove all placeholder characters and separators
        rawValue = rawValue.replace(/-/g, "").replace("///g", "");

        if (rawValue.length === 0) return defaultMask;

        // MUST only be digits!
        if (isDigitsOnly(rawValue)) {
          const mask = [];

          // Danish and Finish
          if (
            this.localizationHelper.currentUserLanguageId === BiLanguageId.DK ||
            this.localizationHelper.currentUserLanguageId === BiLanguageId.NO ||
            this.localizationHelper.currentUserLanguageId === BiLanguageId.FI
          ) {
            if (Number(rawValue[0]) <= 2) {
              mask.push(/[0-2]/, /[1-9]/, "/");
            } else mask.push(/[3]/, /[0-1]/, "/");

            // If month is entered
            if (rawValue.length >= 3) {
              if (rawValue[2] === "0") mask.push(/[0]/, /[0-9]/, "/");
              else mask.push(/[1]/, /[0-2]/, "/");
            } else mask.push(/[0-1]/, /[1-9]/, "/");

            // add year-rule
            mask.push(/[2]/, /[0-1]/, /[0-9]/, /[0-9]/);
            return mask;
          }
          // Swedish
          else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.SE) {
            // add year-rule first
            mask.push(/[2]/, /[0-1]/, /[0-9]/, /[0-9]/, "-");

            // If month is entered (comes after year)
            if (rawValue.length >= 5) {
              if (rawValue[4] === "0") mask.push(/[0]/, /[0-9]/, "-");
              else mask.push(/[1]/, /[0-2]/, "-");

              // If day is entered
              if (rawValue.length >= 7) {
                if (Number(rawValue[6]) <= 2) {
                  mask.push(/[0-2]/, /[0-9]/);
                } else mask.push(/[3]/, /[0-1]/);
              }
            }
            return mask;
          }
          // English
          else if (this.localizationHelper.currentUserLanguageId === BiLanguageId.EN) {
            if (Number(rawValue[0]) <= 2) {
              mask.push(/[0-2]/, /[1-9]/, "/");
            } else mask.push(/[3]/, /[0-1]/, "/");

            // If month is entered
            if (rawValue.length >= 3) {
              if (rawValue[2] === "0") mask.push(/[0]/, /[0-9]/, "/");
              else mask.push(/[1]/, /[0-2]/, "/");
            } else mask.push(/[0-1]/, /[1-9]/, "/");

            // add year-rule
            mask.push(/[2]/, /[0-1]/, /[0-9]/, /[0-9]/);
            return mask;
          }
        }

        // Contains non-digits
        return defaultMask;
      },
      placeholderChar: this.localizationHelper.isDanishLang ? "-" : "_",
      keepCharPositions: true
    };
  }

  /**
   * Returns mask configuration for a time input that's using the Angular 2 Input Mask library
   */
  public getTimeInputMaskProperties(): {
    mask: (rawValue) => Array<any>;
    placeholderChar: string;
    keepCharPositions: boolean;
  } {
    return {
      mask: (rawValue: string) => {
        // Remove all placeholder characters and separators
        rawValue = rawValue.replace(/-/g, "").replace(/:/g, "");

        const mask = [];
        if (rawValue.length === 0 || !isDigitsOnly(rawValue)) return [/[0-9]/, /[0-9]/, ":", /[0-5]/, /[0-9]/];

        // Setup hour rule
        if (rawValue[0] === "0" || rawValue[0] === "1") mask.push(/[0-1]/, /[0-9]/, ":");
        else mask.push(/[2]/, /[0-3]/, ":"); // max. 23 hour

        // Now setup minute rule (max. 59)
        mask.push(/[0-5]/, /[0-9]/);

        return mask;
      },
      placeholderChar: "-",
      keepCharPositions: true
    };
  }

  //#region DATE AND TIME PICKER SETUP FUNCTIONS

  /**
   * Initializes and input with Pickadate date picker. Note: if you set this up on an input used as an Angular FormCOntrol, you must manually create a close function for the
   * date picker and in here set the value of the FormControl
   * @param idOfHtmlInput Id of the HTML native element (excluding the # tag)
   * @param formControl The associated form control for this datepicker
   * @returns The Pickadate DatePicker object
   */
  public setupDatepicker(idOfHtmlInput: string, formControl: AbstractControl, minDate?: Date, maxDate?: Date): Pickadate.DatePicker {
    const datepickerConfig = this.getGeneralDatepickerConfig();

    // Set min if date must be today or after
    if (minDate) datepickerConfig.min = minDate;
    if (maxDate) datepickerConfig.max = maxDate;

    // Get the jQuery ref to the input element
    const dateInput = $("#" + idOfHtmlInput);
    const pickadateObject: Pickadate.DatePicker = dateInput.pickadate(datepickerConfig).pickadate("picker");

    // FIX for the bug causing picker to be opened immediately after closing it (picker both opens on input click and focus
    // and we only want it to open on click)
    dateInput.off("focus");

    pickadateObject.on("open", () => {
      if ((dateInput.val() as string).trim() !== "") {
        // Get input value (if user entered something manually)
        const date = parseFormatedDateStringToMoment(dateInput.val() as string, this.localizationHelper.dateFormat);
        let validationResult = true;

        if (date.isValid()) {
          if (minDate && date.valueOf() < minDate.valueOf()) validationResult = false;

          if (maxDate && date.valueOf() > maxDate.valueOf()) validationResult = false;
        } else validationResult = false;

        if (validationResult) pickadateObject.set("select", [date.year(), date.month(), date.date()]);
      }
    });
    pickadateObject.on("close", () => {
      formControl.setValue(pickadateObject.get());
      formControl.markAsDirty(); // as the form group doesn't mark control dirty if set programatically
      dateInput.blur();
    });

    return pickadateObject;
  }

  /**
   * Initializes and input with Pickadate time picker. Note: if you set this up on an input used as an Angular FormCOntrol, you must manually create a close function for the
   *  time picker and in here set the value of the FormControl
   * @param idOfHtmlInput Id of the HTML native element (excluding the # tag)
   * @returns The Pickadate Timepicker object
   */
  public setupTimepicker(idOfHtmlInput: string, formControl: AbstractControl, defaultValue?: string): Pickadate.TimePicker {
    const timeHtmlInput = $("#" + idOfHtmlInput);
    const pickatimeObject: Pickadate.TimePicker = timeHtmlInput
      .pickatime({
        format: "HH:i",
        editable: true,
        clear: "",
        interval: 10
      })
      .pickatime("picker");

    pickatimeObject.on("open", () => {
      // Handle if user has entered something manually - the time picke value must be correctly updated
      const momentTime = moment(timeHtmlInput.val(), "HH:mm");
      if (momentTime.isValid()) pickatimeObject.set("select", [momentTime.hour(), momentTime.minute()]);

      // HACK: as the pickatimeObject.set("view", "08:00") doesn't work, we need to manually scroll to the highlighted value!
      const hightlightedListItem$ = pickatimeObject.$root.find(".picker__list-item--highlighted");
      pickatimeObject.$root.find(".picker__holder").scrollTop((hightlightedListItem$.index() - 3) * hightlightedListItem$.outerHeight());
    });

    pickatimeObject.on("close", () => {
      formControl.setValue(pickatimeObject.get());
      formControl.markAsDirty(); // as the form group doesn't mark control dirty if set programatically
      timeHtmlInput.trigger("blur");

    });

    // FIX for the bug causing picker to be opened immediately after closing it (picker both opens on input click and focus
    // and we only want it to open on click)
    timeHtmlInput.off("focus");

    pickatimeObject.set("select", defaultValue ?? "08:00");

    return pickatimeObject;
  }

  //#endregion

  /**
   * Wether a date is invalid or before today's date
   * @param dateString
   * @returns 0 if valid, 1 if invalid or 2 if before today's date
   */
  public dateBeforeTodayOrInvalid(momentDate: moment.Moment): number {
    if (!momentDate.isValid()) return 1;

    return momentDate.isBefore(moment(), "day") ? 2 : 0;
  }

  /**
   * Checks wether a date is before right now in either day, minute or both.
   * @param momentDate The date as a Moment object
   */
  public isDateAndTimeBeforeToday(momentDate: moment.Moment): boolean {
    if (momentDate.isBefore(moment(), "day")) return true;
    if (momentDate.isSame(moment(), "day") && momentDate.isBefore(moment(), "minute")) return true;
  }

  /**
   * Wether a start date is in sync with an end date
   * @returns True if in sync, false if not
   */
  public validateStartAndEndDateTime(momentStartDate: moment.Moment, momentEndDate: moment.Moment): boolean {
    if (momentStartDate.isBefore(momentEndDate, "day")) return true;
    if (momentStartDate.isSame(momentEndDate, "day")) {
      // Same day - check time
      return momentStartDate.isBefore(momentEndDate, "minute");
    }
    return false; // start date seems to be after end date - that's a false!
  }
}
