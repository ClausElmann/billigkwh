import { BiCountryId } from "app-globals/enums/BiLanguageAndCountryId";

export class GlobalState {
  public countries: Array<{ countryId: BiCountryId; name: string }> = [];

  /**
   * The route that user was trying to visit before he/she was logged out as a result of an expired refresh token.
   * This makes it possible to navigate back to where user left of after logging in again.
   */
  public routeAfterLogin?: string;

  public clientIp: string;

  public constructor(countries: Array<{ countryId: BiCountryId; name: string }>) {
    this.countries = countries;
  }
}
