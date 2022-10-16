import { findOneIana } from "windows-iana";

/**
 * Converts a Windows/.NET timezone id (aka. time zone index value) into an IANA time zone, which can be used with Moment Js.
 * Windows time zones here: https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values
 * IANA time zones here: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
 * @param timeZoneId Windows/.NET Timezone id
 */
export function convertWindowsTimeZoneToIANA(timeZoneId: string) {
  const timeZone = findOneIana(timeZoneId);
  if (timeZone) return timeZone;

  console.info(`Couldn't match IANA timezone with timezone id "${timeZoneId}". Falling back to "Romance Standard Time"...`);
  return "Europe/Copenhagen";
}
