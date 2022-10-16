import { AbstractControl, ValidatorFn } from "@angular/forms";
import { BiCountryId } from "@globals/enums/BiLanguageAndCountryId";
import { isStringNullOrEmpty } from "@globals/helper-functions";
import { landLineLength_FI, landLineLength_SE, phoneLength_DK, phoneLength_FI, phoneLength_SE } from "./global-constants";

export class CustomValidators {

  /**
 * Validates a phone number's length. Sometimes, there's a min. length and a max. length and other times, there's 1 required length.
 * If length is below min., the error "phoneMin" is added to FormControl.
 * If length is more than max., the error "phoneMax" is added to FormControl.
 * Finally, if a required length is not met, the error "phoneLength" is added to FormControl.
 * @param isLandLine Whether the phone number to be validated is a landline numnber (fastnet)
 */
  public static phoneLengthValidator(countryId: number, isLandLine = false, allowCountryCode = false): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (c.value) {
        const inputLength = (c.value as number).toString().length;

        // In some countries, there's a minimum required phone length as a leading 0 is sometimes used
        if (countryId !== BiCountryId.DK && countryId !== BiCountryId.NO) {
          let minLength: number;
          let maxLength: number;
          if (isLandLine) {
            if (countryId == BiCountryId.SE) {
              minLength = landLineLength_SE[0];
              maxLength = landLineLength_SE[1];
            } else if (countryId == BiCountryId.FI) {
              minLength = landLineLength_FI[0];
              maxLength = landLineLength_FI[1];
            }
          } else {
            // NOT landline but mobile number.
            if (countryId == BiCountryId.SE) {
              minLength = phoneLength_SE[0];
              maxLength = phoneLength_SE[1];
            } else if (countryId == BiCountryId.FI) {
              minLength = phoneLength_FI[0];
              maxLength = phoneLength_FI[1];
            }
          }
          if (inputLength < minLength) return { phoneMin: true };
          if (inputLength > maxLength) return { phoneMax: true };
        } else {
          // DK and NO number lengths are equal
          if (allowCountryCode && inputLength === (phoneLength_DK + 2) && (c.value.toString() as string).slice(0, 2) === (countryId === BiCountryId.DK ? "45" : "47"))
            return undefined;

          if (inputLength !== phoneLength_DK) {
            return { phoneLength: true };
          }
        }
      }
      return null;
    };
  }

  /**
 * Validates that input value is only digits. This is usefull when input is for a string value that must only contain
 *  digits.
 * This will add the error "digitsOnly" to formcontrol if there's an error
 * @param whiteSpaceAllowed Flag telling if white spaces are allowed in the control's value. Validation function will just temporarily
 * remove white space and then validate
 */
  public static digitsOnly(whiteSpaceAllowed?: boolean): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (isStringNullOrEmpty(c.value)) return null; // no email to validate so no error

      const value = !whiteSpaceAllowed ? c.value : c.value.toString().replace(/\s/g, "");

      if (/^\d+$/.test(value)) {
        return null;
      } // No error

      return { digitsOnly: true }; // error!
    };
  }

}
