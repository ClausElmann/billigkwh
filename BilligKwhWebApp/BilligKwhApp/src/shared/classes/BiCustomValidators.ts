import { AbstractControl, FormControl, FormGroup, ValidatorFn } from "@angular/forms";
import moment from "moment-timezone";
import { isStringNullOrEmpty } from "../variables-and-functions/helper-functions";

/**
 * Static class holding all the custom validators used for FormControl in FormGroups.
 */
export class BiCustomValidators {

  /**
   * Validator checking wether a value has a required character length. This also works for numbers!
   *  If validation fails, this will add the error "length" to the errors object of the form control
   */
  public static checkLength(requiredLength: number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (c.value) {
        if (!isNaN(c.value) && c.value.toString().length !== requiredLength) return { length: true };
        else if (typeof c.value === "string" && c.value.length !== requiredLength) return { length: true };
      }
      return null;
    };
  }

  /**
   * Validator checking that a control has a value of minimum length corresponding to the given length. The difference from the
   * validator already existing in Angular's Validators is that this one also works for inputs of type number.
   * If validation fails, this will add the error "length" to the errors object of the form control
   */
  public static minLength(minLength: number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (c.value) {
        if (c.value.toString().length < minLength) return { length: true };
      }
      return null;
    };
  }



  /**
   * Function for validating a username or email entered in a formcontrol. Uses same email regular expr. as Angular source code.
   *  If validation fails, this will add the error "email" to the errors object of the form control.
   * NB: No/empty value means no error (nothing to validate).      *
   */
  public static email(): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (!c.value || c.value === "") return null; // no email to validate so no error

      const emailRegEx = /^[^\s@]+@[^\s@]+\.[^\s@]+$/i;

      if (emailRegEx.test(c.value) && !/\s/.test(c.value)) {
        return null;
      } // No error

      return { email: true }; // error!
    };
  }

  // /**
  //  * Validates that input value is only digits. This is usefull when input is for a string value that must only contain
  //  *  digits.
  //  * This will add the error "digitsOnly" to formcontrol if there's an error
  //  * @param whiteSpaceAllowed Flag telling if white spaces are allowed in the control's value. Validation function will just temporarily
  //  * remove white space and then validate
  //  */
  // public static digitsOnly(whiteSpaceAllowed?: boolean): ValidatorFn {
  //   return (c: AbstractControl): { [key: string]: boolean } | null => {
  //     if (isStringNullOrEmpty(c.value)) return null; // no email to validate so no error

  //     const value = !whiteSpaceAllowed ? c.value : c.value.toString().replace(/\s/g, "");

  //     if (/^\d+$/.test(value)) {
  //       return null;
  //     } // No error

  //     return { digitsOnly: true }; // error!
  //   };
  // }

  /**
   * Function for validating multiple emails with semicolon-separation entered in a text input. Uses same email regular expr. as Angular source code
   *  If validation fails, this will add the error "email" to the errors object of the form control.
   * NB: No/empty value means no error (nothing to validate).
   */
  public static emailMultiple(): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (!c.value || c.value === "") return null; // no email to validate so no error

      const emailRegEx = /^(?=.{1,254}$)(?=.{1,64}@)[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+(\.[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+)*@[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?(\.[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?)*$/;

      const emails = (c.value as string).split(";");
      for (let i = 0; i < emails.length; i++) {
        if (!emailRegEx.test(emails[i].trim())) return { email: true }; // an email was invalid
      }
      return null; // No error
    };
  }

  /**
   * Function for validating a user profile password using the following criteria: valid if it contains a number,
   * a lower case letter, a upper case Letter and one of these special character *$-+?_&=!%{}/()#
   *  If validation fails, this will add the error "password" to the errors object of the form control
   */
  public static password(canBeEmpty = false): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (isStringNullOrEmpty(c.value)) return canBeEmpty ? null : { password: true }; // error!

      // Regex for strong password (meeting all our criteria) - strongly inspired by
      // https://www.thepolyglotdeveloper.com/2015/05/use-regex-to-test-password-strength-in-javascript/
      const passwordRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[*$+?_&=!%{}/()#-])(?=.{8,})");

      if (passwordRegex.test(c.value)) {
        return null;
      } // No error

      return { password: true }; // error!
    };
  }

  /**
   * For validating wether the value of the FormControl (to which this validator is applied) is equal to antoher FormControl's value.
   * If validation fails, the error "equalToOther" will be added to the errors object of the FormControl
   * @param otherInput The other FormControl
   */
  public static equalToOther(otherInput: AbstractControl): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      return c.value === otherInput.value ? null : { equalToOther: true }; // error!
    };
  }

  /**
   * Validator that valdiates whether the input value is not equal to a specified value.
   * If the value is equal, the error property "notEqualTo: true" will be set on errors
   * @param value The value that input value must not be equal to
   */
  public static notEqualTo(value: string): ValidatorFn {
    return (c: FormControl): { [key: string]: boolean } | null => {
      if (!c.value || c.value.trim() !== value.trim()) return null;

      return {
        notEqualTo: c.value.trim() === value.trim()
      }
    }
  }

  /**
   * Validator that checks whether the control has a value or at least 1 of all the passed controls has.
   * If validation fails, the error "atLeast1Required" is set on all controls
   * NB: this validator ensures that if there no more errors on a control that earlier had, then errors object is set to null and control is updated.
   * @param theOtherControls The other form controls that this control depends on
   * @returns null if no errors, {atLeast1Required: true} if error
   */
  public static checkAtLeastOneHasValue(...theOtherControls: AbstractControl[]): ValidatorFn {
    // Local function used for checking if there are any errors on the other controls and then remove the "required" error
    const checkAndRemoveErrorOnOtherCtrl = () => {
      theOtherControls.forEach((control) => {
        if (control.errors) {
          delete control.errors.atLeast1Required;
          // If no more error, set errors object to null and update the control
          if (Object.keys(control.errors).length === 0) {
            control.setErrors(null);
            control.markAsUntouched();
            control.updateValueAndValidity();
          }
        }
      });
    };

    return (c: AbstractControl): { [key: string]: boolean } | null => {
      // if (c.pristine && theOtherControls.findIndex((c) => c.dirty) === -1) return null;

      if (c.value) {
        // This validation is succesfull - be sure to fully remove this error on the other controls
        checkAndRemoveErrorOnOtherCtrl();
        return null;
      } else {
        // check the other controls
        for (let i = 0; i < theOtherControls.length; i++) {
          if (theOtherControls[i].value) {
            checkAndRemoveErrorOnOtherCtrl();
            return null;
          }
        }
      }

      // No values! For all control, set errors and mark as dirty and touched (for the view to show the errors)
      theOtherControls.forEach((control) => {
        if (control.touched || control.dirty) {
          control.setErrors({ atLeast1Required: true });
          control.markAsDirty();
          control.markAsTouched();
        }
      });
      return { atLeast1Required: true };
    };
  }

  /**
   * As Angular's inbuild "required" validator func doesn't work for <select> formcontrols, we can use this validator instead. Adds the error "required" to the formcontrol
   * @param nonSelectedValue The value if no value is selected - meaning validation fails if selected value is equal to this.
   */
  public static requiredSelect(nonSelectedValue: string | number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      return c.value === nonSelectedValue.toString() ? { required: true } : null;
    };
  }

  /**
   * Validates that a string representing latitude and longitude is correctly formated as "lat,long"
   */
  public static latOrLongValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      const error = { latLong: true },
        latLong = control.value,
        commaSplit = latLong.split(","),
        dotSplit = latLong.split(".");

      if (isStringNullOrEmpty(control.value)) return null;
      if (latLong.includes(",") && latLong.includes(".")) return error;

      if (latLong.includes(".")) {
        // Split-string must have two digit entries and the last entry contain 2 digits or more
        if (dotSplit.length !== 2 || dotSplit[1].length < 2) return error;
        if (isNaN(dotSplit[0]) || isNaN(dotSplit[1])) return error;

        return null;
      }
      if (latLong.includes(",")) {
        // Split-string must have two digit entries and the last entry contain 2 digits or more
        if (commaSplit.length !== 2 || commaSplit[1].length < 2) return error;
        if (isNaN(commaSplit[0]) || isNaN(commaSplit[1])) return error;

        return null;
      }

      return error;
    };
  }
  public static latLongValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (isStringNullOrEmpty(control.value)) return null;

      const error = { latLong: true };

      const latLong = control.value;
      if (latLong.indexOf(",") === -1) return error;
      const commaSplit = latLong.split(",");
      // There should be 1 and ONLY 1 comma and hereby a split of length 2. More or less is an error
      if (commaSplit.length !== 2) return error;

      // Must be numbers
      if (isNaN(commaSplit[0]) || isNaN(commaSplit[1])) return error;

      return null; // should be ok
    };
  }


  /**
   * Returns a validator function to be used for a text control. Validates that ther text doesn't
   * contain an invalid/unavailable merge field. If validation fails, the error "mergeField" is set on the errors object for FormControl.
   * @param availableMergefields Array of the names of the available merge fields for a text input
   */
  public static invalidMergeFieldsValidator(availableMergefields: Array<string>): ValidatorFn {
    return (c: AbstractControl) => {
      if (isStringNullOrEmpty(c.value)) return null;
      const foundMergefields = (c.value as string).match(/\[(.*?)\]/g);
      if (foundMergefields) {
        const availableMergefieldsJoined = availableMergefields.join(","); // create a string so we can easily search for existence of merge fields
        for (let i = 0; i < foundMergefields.length; i++) {
          const field = foundMergefields[i].replace("c_", "").replace("[", "").replace("]", "");
          if (availableMergefieldsJoined.indexOf(field) === -1) return { mergeField: true };
        }
      }

      return null;
    };
  }

  //#region DATE AND TIME VALIDATION
  /**
   * Returns validator function that validates a date against correct format. This will add the error "format" on the form control if validation fails.
   * @param dateFormat The (moment Js compatible) format of the date so we know what format the input string on control is. E.g. DD/MM/YYYY in DK
   */
  public static validateDateFormat(dateFormat: string): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (!c.value || c.value === "") return null; // nothing entered => nothing to validate
      const valid = moment(c.value, dateFormat, true).isValid();
      return valid ? null : { format: true };
    };
  }

  /**
   * Validates that a time is in correct format, which is "HH:mm". If validation fails, the error "format" will be set on the errors object
   */
  public static validateTime(): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (!c.value || c.value === "") return null; // nothing entered => nothing to validate
      const valid = moment(c.value, "HH:mm", true).isValid();
      return valid ? null : { format: true };
    };
  }

  /**
   * Returns validator function that validates wether the control's date is the same or after another date (not looking at time). This will again add the error "isSameOrAfter" on the form control if validation fails.
   * @param date The date to check against. Must be a moment object
   * @param dateFormat The (moment Js compatible) format of the date so we know what format the input string on control is. E.g. DD/MM/YYYY in DK
   */
  public static validateDateIsSameOrAfterDate(date: moment.Moment, dateFormat: string): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const momentDate = moment(c.value, dateFormat, true);
      if (momentDate.isValid() && date.isValid()) {
        return momentDate.isSameOrAfter(date, "day") ? null : { isSameOrAfter: true };
      }
      return null; // not a date, so cannot validate.
    };
  }

  /**
   * Returns validator function that validates wether the control's datetime is the same or after another datetime. This will again add the
   * error "isSameOrAfter" on the form control if validation fails.
   * @param dateTime The datetime to check against. Must be a moment object
   * @param dateTimeFormat The (moment Js compatible) format of the date so we know what format the input string on control is.
   * E.g. DD/MM/YYYY HH:mm in DK
   */
  public static validateDateTimeIsSameOrAfterDateTime(dateTime: moment.Moment, dateTimeFormat: string, timeControl: FormControl): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (dateTime.isValid() && timeControl.valid) {
        const momentDate = moment(`${c.value} ${timeControl.value}`, dateTimeFormat, true);
        if (momentDate.isValid()) {
          if (momentDate.isSameOrAfter(dateTime, "minute")) {
            timeControl.setErrors({ ...timeControl.errors, isSameOrAfter: null });
            return null;
          } else {
            timeControl.setErrors({ ...timeControl.errors, isSameOrAfter: true });
            return { isSameOrAfter: true };
          }
        }
      }
      return null; // not a date, so cannot validate.
    };
  }

  /**
   * Validates whether a "from date"-control is same or before the date entered in a "to date"-control.
   * That is, this is meant to be used with a FormControl representing a FROM DATE.
   * If validation fails, an error called "isSameOrBefore" will be set.
   */
  public static validateFromDateIsSameOrBeforeToDate(toDateCtrl: FormControl, dateFormat: string): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const momentFromDate = moment(c.value, dateFormat, true);
      const momentToDate = moment(toDateCtrl.value, dateFormat, true);
      if (momentFromDate.isValid() && momentToDate.isValid()) {
        return momentFromDate.isSameOrBefore(momentToDate, "day") ? null : { isSameOrBefore: true };
      }
      return null; // not a date, so cannot validate.
    };
  }

  /**
   * Validates whether a "to date"-control is same or after the date entered in a "from date"-control.
   * That is, this is meant to be used with a FormControl representing a TO DATE
   */
  public static validateToDateIsSameOrAfterFromDate(fromDateCtrl: FormControl, dateFormat: string): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (isStringNullOrEmpty(c.value)) return null;
      const momentFromDate = moment(fromDateCtrl.value, dateFormat, true);
      const momentToDate = moment(c.value, dateFormat, true);
      if (momentFromDate.isValid() && momentToDate.isValid()) {
        return momentFromDate.isSameOrBefore(momentToDate, "day") ? null : { isSameOrAfter: true };
      }
      return null; // not a date, so cannot validate.
    };
  }

  /**
   * Validates that a from date and to date are in sync ("from" not after "to" and the other way). You can pass times also making the
   * validation to also look at the time if dates are equal. Note here: we assume that the passed controls are not empty and in correct format.
   * NOTE: To be used on a FORMGROUP - not a FORMCONTROL! Also, the error(s) set on form will also be set on the control(s).
   * @returns Error object with a single property: {errorName: true} OR null. Possible errors are: "fromDateAfterTo" and "fromTimeAfterTo"
   */
  public static validateFromDateTimeInSyncWithTo(
    fromDateCtrl: FormControl,
    toDateCtrl: FormControl,
    dateFormat: string,
    fromTimeCtrl?: FormControl,
    toTimeCtrl?: FormControl
  ): ValidatorFn {
    return (fGroup: FormGroup): { [key: string]: boolean } | null => {
      // This function is only concentrating about the "from" and "to" synchronization and expects otherwise valid dates.
      // Only allowed errors here are the one we're interested in - otherwise don't do anything
      if (
        (fromDateCtrl.valid || (Object.keys(fromDateCtrl.errors).length == 1 && fromDateCtrl.errors.fromDateAfterTo)) &&
        (toDateCtrl.valid || (Object.keys(toDateCtrl.errors).length == 1 && toDateCtrl.errors.fromDateAfterTo))
      ) {
        const fromDateMoment = moment(fromDateCtrl.value, dateFormat, true);
        const toDateMoment = moment(toDateCtrl.value, dateFormat, true);
        if (fromDateMoment.isAfter(toDateMoment, "day")) {
          // Set errors on the controls and form
          fromDateCtrl.setErrors({ fromDateAfterTo: true });
          toDateCtrl.setErrors({ fromDateAfterTo: true });
          return { fromDateAfterTo: true };
        }

        // reset old errors
        fromDateCtrl.setErrors(undefined);
        toDateCtrl.setErrors(undefined);

        // From is before or same as to. If it's the same, then look at possible time values
        if (fromTimeCtrl && toTimeCtrl && fromDateMoment.isSame(toDateMoment)) {
          if (moment(fromTimeCtrl.value, "HH:mm").isValid() && moment(toTimeCtrl.value, "HH:mm").isValid()) {
            if (moment(fromTimeCtrl.value, "HH:mm").isSameOrAfter(moment(toTimeCtrl.value, "HH:mm"))) {
              // mark controls as dirty. Handles the case when user just selected the same DATE without touching the time
              fromTimeCtrl.markAsDirty();
              fromTimeCtrl.setErrors({ fromTimeAfterTo: true });
              toTimeCtrl.setErrors({ fromTimeAfterTo: true });
              return { fromTimeAfterTo: true };
            }
            // Clear error. Important to do it in this scope as we know that the time controls doesn't have any other errors
            fromTimeCtrl.setErrors(null);
            toTimeCtrl.setErrors(null);
          }
        }
        return null;
      }
      return fGroup.errors;
    };
  }

  //#endregion
}
