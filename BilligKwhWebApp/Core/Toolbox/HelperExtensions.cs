using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Core.Toolbox
{
    public static class HelperExtensions
    {
        public static string Truncate(this string value, int maxChars, bool useTrailingDots = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            if (useTrailingDots)
            {
                return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
            }
            else
            {
                return value.Length <= maxChars ? value : value.Substring(0, maxChars);
            }
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string FirstCharToUpper(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }


        public static int Limit(this int value, int limit)
        {
            if (value > limit)
            {
                value = limit;
                return value;
            }
            else
            {
                return value;
            }
        }

        public static short SafeStringToShort(this string value)
        {
            short result = 0;
            short.TryParse(value, out result);
            return result;
        }

        public static short? SafeStringToNullableShort(this string value)
        {
            short result = 0;
            if (short.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static int SafeStringToInt(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        public static int? SafeStringToNullableInt(this string value)
        {
            int result = 0;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static long? SafeStringToNullableLong(this string value)
        {
            long result = 0;
            if (long.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static double SafeStringToDouble(this string value)
        {
            double result = 0.0;
            double.TryParse(value, out result);
            return result;
        }

        public static double? SafeStringToNullableDouble(this string value)
        {
            double result = 0.0;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        #region DateTime extensions
        public static string ToSqlFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public static string ToSqlShortFormat(this DateTime dateTime)
        {
            var convertedDay = "";
            var convertedMonth = "";

            // Keep leading zeroes on days on month between 1-9
            if (dateTime.Day > 0 && dateTime.Day <= 9) { convertedDay = "0" + dateTime.Day; } else { convertedDay = dateTime.Day.ToString(); }
            if (dateTime.Month > 0 && dateTime.Month <= 9) { convertedMonth = "0" + dateTime.Month; } else { convertedMonth = dateTime.Month.ToString(); }

            return dateTime.Year.ToString() + convertedMonth + convertedDay;
        }
        public static string ToSqlUtcFormat(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        /// <summary>
        /// Converts a UTC DateTime into a new date time having the provided time zone info applied and returns
        /// its formatted string representation based on language.
        /// </summary>
        /// <param name="utcDateTime">MUST be a UTC DateTime</param>
        /// <param name="timezone">Destination time zone</param>
        /// <returns>Localized date string</returns>
        public static string FromUtcToTimeZone(this DateTime utcDateTime, TimeZoneInfo timezone, int languageId)
        {
            if (timezone == null)
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                if (isWindows)
                {
                    timezone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                }
                else
                {
                    //Linux
                    //https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
                    timezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
                }
            }

            DateTime convertedTimeZone = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezone);
            switch (languageId)
            {
                case CountryConstants.DanishCountryId:
                case CountryConstants.NorwegianCountryId:
                case CountryConstants.FinnishCountryId:
                    return convertedTimeZone.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                case CountryConstants.SwedishCountryId:
                case CountryConstants.EnglishCountryId:
                    return convertedTimeZone.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                default:
                    return convertedTimeZone.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
        }
        public static string FromUtcToLocalDate(this DateTime utcDateTime, TimeZoneInfo timezone, int countryId)
        {
            DateTime convertedTimeZone = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezone);
            switch (countryId)
            {
                case CountryConstants.DanishCountryId:
                case CountryConstants.FinnishCountryId:
                case CountryConstants.NorwegianCountryId:
                    return convertedTimeZone.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                case CountryConstants.SwedishCountryId:
                case CountryConstants.EnglishCountryId:
                    return convertedTimeZone.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                default:
                    return convertedTimeZone.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }
        public static string FromUtcToLocalDate(this DateTime utcDateTime, TimeZoneInfo timezone)
        {
            DateTime convertedTimeZone = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezone);

            return convertedTimeZone.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertFromUnixToUTC(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime? ToNullableUtcDateTime(this DateTimeOffset? date)
        {
            return date.HasValue ? date.Value.UtcDateTime : null;
        }

        /// <summary>
        /// Takes a nullable UTC DateTime and converts it to a nullable DateTimeOffset where offset is set to 0. This
        /// should be used in DTOs with DateTimeOffset props so that DateTimes already being in UTC
        /// won't be converted to UTC again meaning double conversion issues.
        /// </summary>
        /// <param name="utcDate">UTC date</param>
        /// <returns></returns>
        public static DateTimeOffset? ToNullableUtcDateTimeOffset(this DateTime? utcDate)
        {
            return utcDate.HasValue ? new DateTimeOffset(utcDate.Value, TimeSpan.Zero) : null;
        }
        #endregion

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T TryParseEnum<T>(string value, T defaultValue) where T : struct
        {
            T returnEnum = defaultValue;
            Enum.TryParse<T>(value, true, out returnEnum);

            return returnEnum;
        }

        // Enumerables
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }
        public static async Task ForEach<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            foreach (var item in enumerable)
            {
                await action(item);
            }
        }

        public static List<T> AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
            return list;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }

        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    return source?.GroupBy(keySelector).Select(grp => grp.First());
        //}

        /// <summary>
        /// Writes a list to CSV file on local disc for debugging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public static void WriteCSV<T>(this IEnumerable<T> items, string path)
        {
            if (items is null || !items.Any()) return;

            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }

        #region DateTime extensions
        public static DateTime NoMilliseconds(this DateTime datetime)
        {
            return datetime.AddMilliseconds(-datetime.Millisecond);
        }
        #endregion
    }
}
