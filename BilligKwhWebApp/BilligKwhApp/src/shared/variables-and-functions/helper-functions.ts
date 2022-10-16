import { cloneDeep } from "lodash";
import { AbstractControl } from "@angular/forms";
//import { BiCountryId } from "@globals/enums/BiLanguageAndCountryId";
import moment, { Moment } from "moment-timezone";
import { BiCountryId, BiLanguageId } from "app-globals/enums/BiLanguageAndCountryId";


/**
 * Detects whether the app is currently running in Internet Explorer or not.
 * Found here: https://stackoverflow.com/a/9851769
 */
export function isRunningInIE() {
  return false || !!(document as any).documentMode;
}

/**
 * Makes a full clone of an object. E.i. you get a ref. to a new, identical object
 */
export function cloneObject<T>(obj: T): T {
  return cloneDeep(obj);
}

/**
 * Replaces all occurrences of new lines in a string (e.g. from a textarea element )
 * with HTML '<br>' tags
 */
export function convertNewlinesToHtmlTags(stringWithLineBreaks: string) {
  return stringWithLineBreaks.replace(/(?:\r\n|\r|\n|\u21B5)/g, "<br/>");
}

/**
 * Returns language code by looking at hostname. If it cannot be determined, null is returned
 */
export function getLanguageCodeByHostName() {
  if (location.hostname.indexOf(".se") !== -1) return "se";
  if (location.hostname.indexOf(".dk") !== -1) return "da";
  if (location.hostname.indexOf(".fi") !== -1) return "fi";
  if (location.hostname.indexOf(".no") !== -1) return "no";

  return null;
}

export function getLanguageCodeByLanguageId(languageId: BiLanguageId) {
  switch (languageId) {
    case BiLanguageId.DK: return "da";
    case BiLanguageId.SE: return "se";
    case BiLanguageId.EN: return "en";
    case BiLanguageId.FI: return "fi";
    case BiLanguageId.NO: return "no";
  }
}

export function getCountryCodeByCountryId(countryId: BiCountryId) {
  switch (countryId) {
    case BiCountryId.DK: return "45";
    case BiCountryId.SE: return "46";
    case BiCountryId.EN: return "44";
    case BiCountryId.FI: return "358";
    case BiCountryId.NO: return "47";
  }
}

/**
 * Returns a new array where the item at specified index is removed. Usefull when working with immutable arrays.
 */
export function removeItemAtImmutable<T>(array: Array<T>, index: number) {
  if (index < array.length) return array.slice(0, index).concat(array.slice(index + 1, array.length));
  else {
    const clone = cloneObject(array);
    clone.pop();
    return clone;
  }
}


//#region DATE VALIDATION

/**
 * Validates a date time against correct format and that it is not before current date and time. Date part of the DateTimeString must be DD/MM/YYYY or YYYY-MM-DD depending on country
 * @param dateTimeString Datetime string in format DD/MM/YYYY HH:mm or YYYY-MM-DD HH:mm depending on current language
 * @param dateTimeFormat The (Moment Js compatible) format of the date time so we know what format the input string is. E.g. DD/MM/YYYY HH:mm in DK
 * @param allowBeforeToday Allow the date time to before current date time
 * @returns True if valid, false if not
 */
export function validateDateTime(dateTimeString: string, dateTimeFormat: string, allowBeforeToday = false): boolean {
  const momentDate = moment(dateTimeString, dateTimeFormat, true);

  if (!momentDate.isValid()) return false;
  if (momentDate.isAfter(moment(), "day")) return true;
  if (momentDate.isBefore(moment(), "day")) return allowBeforeToday;
  if (momentDate.isSame(moment(), "day") && momentDate.isBefore(moment(), "minute")) return false;

  return true;
}

/**
 * Validates a start and end date against each other and checks wether they're in sync (e.g. start not after end etc.)
 * @param dateTimeStringStart Datetime string in format DD/MM/YYYY HH:mm or YYYY-MM-DD HH:mm depending on current language
 * @param  dateTimeStringEnd Datetime string in format DD/MM/YYYY HH:mm or YYYY-MM-DD HH:mm depending on current language
 * @param dateTimeFormat The (Moment Js compatible) format of the date time so we know what format the input string is. E.g. DD/MM/YYYY HH:mm in DK
 * @returns
 */
export function validateStartAndEndDateTimes(dateTimeStringStart: string, dateTimeStringEnd: string, dateTimeFormat: string): boolean {
  const momentStartDate = moment(dateTimeStringStart, dateTimeFormat, true);
  const momentEndDate = moment(dateTimeStringEnd, dateTimeFormat, true);
  if (!momentStartDate.isValid() || !momentEndDate.isValid()) return true; // either both of 1 of them are invalid so cannot validate.

  if (momentStartDate.isBefore(momentEndDate, "day")) return true; // start day before end day = good!
  if (momentStartDate.isSame(momentEndDate, "day") && momentStartDate.isBefore(momentEndDate, "minute")) return true;


  return false;
}

/**
 * Validates a date string against correct format and that it is not after today
 * @param dateTimeFormat The (Moment Js compatible) format of the date time so we know what format the input string is. E.g. DD/MM/YYYY HH:mm in DK
 */
export function validateDate(dateString: string, dateTimeFormat: string) {
  const momentDate = moment(dateString, dateTimeFormat, true);
  if (momentDate.isValid()) return momentDate.isSameOrBefore(moment(), "day");

  return false;
}

/**
 * Validates a date against correct format
 * @param dateTimeFormat The (Moment Js compatible) format of the date time so we know what format the input string is. E.g. DD/MM/YYYY HH:mm in DK
 */
export function validateDateFormat(dateString: string, dateTimeFormat: string) {
  return moment(dateString, dateTimeFormat, true).isValid();
}

//#endregion

//#region GENERAL HELPERS
/**
 * Helper for generating a random string
 * @param {number} nrOfChars Number of characters in the generated string
 * @returns Random string
 */
export function generateRandomString(nrOfChars: number) {
  let id = "";
  const charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
  for (let i = 1; i <= nrOfChars; i++) {
    const randPos = Math.floor(Math.random() * charSet.length);
    id += charSet[randPos];
  }
  return id;
}

/**
 * Generates a unique string id. Copied from: https://gist.github.com/gordonbrander/2230317
 */
export function uniqueID(): string {
  return "_" + Math.random().toString(36).substr(2, 9);
}

/**
 * Compares two objects
 * @param obj1
 * @param obj2
 * @returns {boolean} True if equal, false if not.
 */
export function areObjectsEqual(obj1, obj2): boolean {
  return JSON.stringify(obj1) === JSON.stringify(obj2);
}

/**
 * Helper for determining wether a string is null, undefined or blank
 * @param theString
 */
export function isStringNullOrEmpty(str: string | null | undefined): boolean {
  return !str || /^\s*$/.test(str);
}


/**
 * Inserts 2 backslashes before each dot in a string. This is mainly used for JQuery Datatables with column names containing dots. These must
 * be escaped
 */
export function insertDoubleSlashBeforDot(stringToModify: string): string {
  const chars: string[] = [];
  for (let i = 0; i < stringToModify.length; i++) {
    if (stringToModify[i] === ".") chars.push("\\.");
    else chars.push(stringToModify[i]);
  }
  return chars.join("");
}
/**
 * Function for removing duplicate objects in an array by a specified property name.
 * Inspired by function here: https://ilikekillnerds.com/2016/05/removing-duplicate-objects-array-property-name-javascript/
 * @param {any[]} array The array to remove duplicates in
 * @param prop The property to filter by
 * @returns new array with removed duplicates
 */
export function removeDuplicatesByProperty(array: any[], prop: string): any[] {
  return array.filter((obj, pos) => {
    return array.map((el) => el[prop]).indexOf(obj[prop]) === pos;
  });
}

/**
 * Compares 2 strings. Equal if string1 === string2 or if the lowercased version are equal. Checks for null
 * @param string1?
 * @param string2?
 */
export function areStringsEqual(string1?: string, string2?: string): boolean {
  if (string1 === string2) return true;
  if (string1 && string2) return string1.toLowerCase() === string2.toLowerCase();
  return false;
}

/**
 * Checks wether a string ONLY contains digits
 * @param text
 * @returns
 */
export function isDigitsOnly(text: string) {
  return /^\d+$/.test(text);
}

/**
 * Converts words in a sentence to title case. Example: pass "very awesome" and you get "Very Awesome"
 * Regex and callback copied from here: https://medium.com/@mwhitt.w/converting-text-to-titlecase-using-angular2-pipes-552b3bfa8e22
 * @param sentence
 * @returns New sentence with capitalized words
 */
export function convertToTitleCase(sentence: string): string {
  if (!isStringNullOrEmpty(sentence)) {
    return sentence
      .split(" ")
      .filter((word) => word.length > 0)
      .map((word) => word[0].toUpperCase() + word.substr(1))
      .join(" ");
  } else return "";
}

/**
 * Sets a value on an object using passed property name. Handles nested property using dots.
 * Copied from here: https://stackoverflow.com/a/44168870
 * @param targetObject Toe object on hich the value should be set
 * @param propertyString Name of the property to set. By using dots (.), nested object value can be set - each dot is a level
 * @param value The value to set
 * @example setValue(someObject, 'location.degree.text', 'sometexthere'); ==> someObject will now hace "sometexthere" as value in location>degree>text
 */
export function setObjectValueByString(targetObject: Record<string, unknown>, propertyString: string, value: any) {
  // split the propertyString by dot for getting all keys (if nested value)
  const keys = propertyString.split("."),
    last = keys.pop();

  keys.reduce(function (o, k) {
    return (o[k] = o[k] || {});
  }, targetObject)[last] = value;
}

/**
 * Converts hour and minutes into a string with the format "HH:mm" while rounding UP to nearest 10 in minutes (or next hour if minutes > 50)
 * @param hours Number of hours
 * @param minutes Number of minutes
 * @returns time string in format "HH:mm" and rounded up
 */
export function roundTimeToNextTen(hours: number, minutes: number) {
  let hoursString = hours < 10 ? "0" + hours : hours.toString();

  // If minutes is already clean 10s
  if (minutes % 10 === 0) return hoursString + ":" + (minutes > 0 ? minutes : "00");
  else {
    // If smaller than 50, we don't need to change the hours
    if (minutes < 50) return hoursString + ":" + Math.ceil(minutes / 10) * 10;
    else {
      // More than 50 => rounds up to 60 meaning next hour and "00" minutes
      hours += 1;
      hoursString = hours < 10 ? "0" + hours : hours.toString();
      return hoursString + ":00";
    }
  }
}

/**
 * Function that removes any non-numeric chars, that can be entered in an <input> element of type "number" or "tel" (like "-.,+'").
 * In other words: clean the value entered in  such an input.
 * To be used for the input's change event
 * @param numberInputCtrl Reference to the FormControl behind the HTML input
 */
export function cleanNumberInput(numberInputCtrl: AbstractControl) {
  if (numberInputCtrl.value) {
    if (isNaN(numberInputCtrl.value)) numberInputCtrl.setValue((numberInputCtrl.value as string).replace(/\D/g, ""));
    else numberInputCtrl.setValue((numberInputCtrl.value as number).toString().replace(/\D/g, ""));
  }
}

/**
 * Generates a string representing an address. Used for consistency in the app so addresses are always displayed the same.
 * The generated string is build up depending on the passed address data and follows one of the format:
 * - STREET HOUSENUMBER LETTER, FLOOR. DOOR. - ZIP CITY
 * - STREET HOUSENUMBER-METERSLETTER - ZIP CITY
 * - STREET METERSLETTER - ZIP CITY
 */
export function createAddressString(
  countryId: BiCountryId,
  zipFormatted?: string,
  city?: string,
  street?: string,
  houseNr?: number,
  meters?: number,
  letter?: string,
  floor?: string,
  door?: string
) {
  const addressParts = [];
  if (!isStringNullOrEmpty(street)) addressParts.push(street);
  if (houseNr) {
    addressParts.push(" " + houseNr);
  }

  const hasLetter = !isStringNullOrEmpty(letter) && letter !== "0";

  if (meters != null) {
    addressParts.push("-" + meters);
  }

  if (hasLetter) addressParts.push(letter);

  if ((!isStringNullOrEmpty(floor) && floor !== "0") || !isStringNullOrEmpty(door) && door !== "0") {
    addressParts.push(", ");
    addressParts.push(!isStringNullOrEmpty(floor) ? floor + ". " : "");
    addressParts.push(!isStringNullOrEmpty(door) ? door + ". " : "");
  }

  if (zipFormatted || !isStringNullOrEmpty(city)) {
    if (addressParts.length > 0) addressParts.push(", ");

    addressParts.push(zipFormatted ? zipFormatted + " " : "");
    addressParts.push(!isStringNullOrEmpty(city) ? city : "");
  }

  return addressParts.join("");
}

/**
 * Function for inserting a merge field in a formcontrol being either a text-input or text area input.
 * @param mergeField The mergefield name surrounded with square brackets
 * @param textFormControl Reference to the formcontrol ion which we want to put a merge field
 */
export function insertMergefieldInText(mergeField: string, textFormControl: AbstractControl) {
  let currentMsgText = textFormControl.value;
  if (!currentMsgText) currentMsgText = "";
  textFormControl.setValue(`${currentMsgText} ${mergeField} `);
}

/**
 * Converts a date time to a new date time rounded to the next whole tens
 * @param timeControl Time input Formcontrol
 * @param dateControl Date input form control
 * @param datePicker The Pickadate instance of the date input
 * @param dateFormat The (Moment Js) compatible date format that the input date string is expected to have. E.g. DD/MM/YYYY for DK.
 */
export function adjustDateTimeToNextTen(timeControl: AbstractControl, dateControl: AbstractControl, dateFormat) {
  // Make sure the start time minutes is whole tens if the time is valid
  const startTimeMoment = moment(timeControl.value, "HH:mm", true);
  if (startTimeMoment.isValid()) {
    // If not after 23:50, we don't need to change date
    if (startTimeMoment.hours() < 23 || startTimeMoment.minutes() <= 50)
      timeControl.setValue(roundTimeToNextTen(startTimeMoment.hours(), startTimeMoment.minutes()), {
        emitEvent: false
      });
    else {
      timeControl.setValue("00:00", { emitEvent: false });
      const startDateMoment = parseFormatedDateStringToMoment(dateControl.value, dateFormat);
      if (startDateMoment.isValid()) {
        // Create date object from the start date with 1 day added
        const date1DayLater = startDateMoment.add(1, "days");
        dateControl.setValue(date1DayLater.format(dateFormat), {
          emitEvent: false
        });
      }
    }
  }
}

/**
 * Takes a moment object and modyfies it by rounding to nearest 10s. If time is >= 55 min, it'll be rounded down.
 * Otherwise, minutes are always rounded up/down - the limite lies at number 5 (like, so >=5 = 10, >=15  = 20, etc).
 * @param roundUpAlways Set true if you always want the date to be rounded up to next 10 minutes. NOTE: rounding up could change the day!
 * @param roundDownAlways Just like roundUpAlways, just rounding down.
 * @returns moment object
 */
export function roundToNearest10(moment: Moment, roundUpAlways = false, roundDownAlways = false) {

  if (moment.minutes() >= 55 && !roundUpAlways && !roundDownAlways) return moment.set("minute", 50);

  const minuteDiff = 10 - (moment.minutes() % 10);
  if (minuteDiff === 0) return moment;
  if (roundUpAlways || !roundDownAlways && minuteDiff < 5) return moment.add(minuteDiff, "minutes");
  else if (roundDownAlways || minuteDiff >= 5) return moment.subtract(moment.minutes() % 10, "minutes");

  return moment;
}

/**
 * Sets up "enter pressed"-event for a text input
 * @param inputEl The input element for which you want to listen for "enter pressed"-event
 * @param callback Function callback to call when enter hit
 */
export function setupOnEnterHandler(inputEl: JQuery<HTMLElement>, callback: any) {
  inputEl.keypress((e) => {
    const keycode = (e.keyCode ? e.keyCode : e.which).toString();
    if (keycode === "13") callback();
  });
}

/**
 * Sorts an array of objects in ASCENDING order. Note: it modifies the original array.
 * @param array The array og objects to sort
 * @param propertyName The property to sort by in the objects
 */
export function sortArrayByPropertyAsc<T>(array: Array<T>, propertyName: string) {
  array.sort((objectA, objectB) => {
    // If the property is a string, make it lower case when comparing
    if (objectA[propertyName].toLowerCase) {
      if (objectA[propertyName].toLowerCase() < objectB[propertyName].toLowerCase()) return -1;
      if (objectA[propertyName].toLowerCase() > objectB[propertyName].toLowerCase()) return 1;
    } else {
      if (objectA[propertyName] < objectB[propertyName]) return -1;
      if (objectA[propertyName] > objectB[propertyName]) return 1;
    }

    return 0;
  });
  return array;
}

/**
 * Checks if the application is running inside an iFrame or not.
 */
export function isInsideIFrameOrPopup() {
  return window !== window.parent;
}


/**
 * Takes a string that contains HTML but where the tags has been replaced with HTML entity codes
 * @param stringWithGtAndLt A string containing &gt, &lt and &amp instead of >, < and &, respectively (e.i. HTML entity codes)
 */
export function unEntityHtmlString(stringWithEntityCodes: string) {
  return stringWithEntityCodes.replace(/&amp;/g, "&").replace(/&lt;/g, "<").replace(/&gt;/g, ">");
}

//#endregion

//#region ====================== Moment Js helpers ===================
/**
 * Parses a date string in a specific format into a Moment date object
 * @param dateFormat The (Moment Js) compatible date format that the input date string is expected to have. E.g. DD/MM/YYYY for DK.
 */
export function parseFormatedDateStringToMoment(theDateString: string, dateFormat: string) {
  return moment(theDateString, dateFormat); // we call "toUpper()" as the format is in lowercase (for the datepicker) but Moment Js expects upper case
}

//#endregion ===========================================================

export function gsmDangerChars() {
  return /([^@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ\x20!"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà\f^{}\\[~\]|€]+)/g;
}

export function checkGsmText(text: string) {
  return gsmDangerChars().test(text);
}

