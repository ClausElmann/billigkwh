/**
 * Common, generic model used for dropdown/select values
 */
export class SelectListItem<T> {
  /**
   * Ctor
   * @param value Could be either a string or a number (typically)
   * @param displayName The name to display as key in dropdown/select element
   */
  public constructor(public value: T, public displayName: string) {}
}
