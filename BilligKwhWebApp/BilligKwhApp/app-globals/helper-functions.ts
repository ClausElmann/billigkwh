import { cloneDeep } from "lodash";

import { BiCountryId, BiLanguageId } from "./enums/BiLanguageAndCountryId";

/**
 * Generates a unique string id. Copied from: https://gist.github.com/gordonbrander/2230317
 */
export function uniqueID(): string {
  return "_" + Math.random().toString(36).substr(2, 9);
}

/**
 * Detects whether the app is currently running in Internet Explorer or not.
 * Found here: https://stackoverflow.com/a/9851769
 */
export function isRunningInIE() {
  return false || !!(document as any).documentMode;
}

/**
 * Helper for determining wether a string is null, undefined or blank
 * @param theString
 */
export function isStringNullOrEmpty(str: string | null | undefined): boolean {
  return !str || /^\s*$/.test(str);
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
