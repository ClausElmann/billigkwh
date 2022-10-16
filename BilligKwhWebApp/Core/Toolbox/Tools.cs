using BilligKwhWebApp.Core.Domain;
using SendGrid.Helpers.Mail;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace BilligKwhWebApp.Core.Toolbox
{
    public static class Tools
    {
        private static readonly Regex EmailValidateRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

        public static int? SafeStringToInt(string s, int? defaultvalue = null)
        {
            int result;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            return defaultvalue;
        }

        public static bool ValidateEmail(string email)
        {
            var emails = email.Trim().ToLowerInvariant().Split(';');

            foreach (var addr in emails)
            {
                var mailToValidate = addr;
                if (addr.Contains(" <"))
                {
                    var mailAddress = MailHelper.StringToEmailAddress(addr);
                    mailToValidate = mailAddress.Email;
                }

                if (!EmailValidateRegex.IsMatch(mailToValidate))
                {
                    return false;
                }
            }
            return true;
        }

        public static string EmailFromFormatted(string fromName, string fromEmail)
        {
            return $"{fromName}<{fromEmail}>";
        }

        public static DateTime? TimeToCentralEuropean(DateTime? dateTimeUtc)
        {
            if (dateTimeUtc == null) return null;
            return TimeToCentralEuropean(dateTimeUtc.Value);
        }

        public static DateTime TimeToCentralEuropean(DateTime dateTimeUtc)
        {
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            TimeZoneInfo euTimeZone;
            if (isWindows)
            {
                euTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            }
            else
            {
                //Linux
                //https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
                euTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
            }
            return TimeZoneInfo.ConvertTime(dateTimeUtc, TimeZoneInfo.Utc, euTimeZone);
        }

        public static string OnlyDigits(string s, string defaultString = "")
        {
            var result = Regex.Replace(s, "[^0-9]+", string.Empty).Trim();
            return string.IsNullOrEmpty(result) ? defaultString : result;
        }

        public static string OnlyLetters(string s, string defaultString = "")
        {
            var result = Regex.Replace(s, @"[^A-Z]+", string.Empty).Trim();
            return string.IsNullOrEmpty(result) ? defaultString : result;
        }

        [Obsolete("Use CountryConstants.GetPhoneCode")]
        public static int GetAccessCode(int countryId)
        {
            return CountryConstants.GetPhoneCode(countryId);
        }

        public static string PhoneAddPrefixes(string phonenumber, int countryId, bool isLandline, out long phoneAsLong, out bool validPhone)
        {
            phoneAsLong = 0;

            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                validPhone = false;
                return "";
            }

            validPhone = PhoneGetParts(phonenumber, countryId, isLandline, out _, out int prefix, out phoneAsLong);
            if (!validPhone)
            {
                return "";
            }

            return $"+{prefix}{phoneAsLong}";
        }

        public static bool PhoneGetParts(string phoneNumber, int defaultCountryId, bool isLandline, out int countryId, out int prefix, out long number)
        {
            number = 0;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                countryId = 0;
                prefix = 0;
                return false;
            }

            phoneNumber = Regex.Replace(phoneNumber, "[^+0-9]", "");

            prefix = PhoneGetPrefix(ref phoneNumber, out countryId);

            if (prefix != 0)
            {
                phoneNumber = phoneNumber[(prefix.ToString(CultureInfo.InvariantCulture).Length)..];
            }
            else
            {
                countryId = defaultCountryId;
                prefix = CountryConstants.GetPhoneCode(defaultCountryId);
            }

            return ValidatePhoneLength(phoneNumber, countryId, isLandline) && long.TryParse(phoneNumber, out number);
        }

        private static int PhoneGetPrefix(ref string phoneNumber, out int countryId)
        {
            if (phoneNumber.StartsWith("+", StringComparison.OrdinalIgnoreCase))
            {
                phoneNumber = phoneNumber[1..];
            }
            else if (phoneNumber.StartsWith("00", StringComparison.OrdinalIgnoreCase))
            {
                phoneNumber = phoneNumber[2..];
            }
            else
            {
                countryId = 0;
                return 0;
            }

            if (phoneNumber.StartsWith("45", StringComparison.OrdinalIgnoreCase))
            {
                countryId = CountryConstants.DanishCountryId;
                return CountryConstants.DanishPhoneCode;
            }
            else if (phoneNumber.StartsWith("46", StringComparison.OrdinalIgnoreCase))
            {
                countryId = CountryConstants.SwedishCountryId;
                return CountryConstants.SwedishPhoneCode;
            }
            else if (phoneNumber.StartsWith("358", StringComparison.OrdinalIgnoreCase))
            {
                countryId = CountryConstants.FinnishCountryId;
                return CountryConstants.FinnishPhoneCode;
            }
            else if (phoneNumber.StartsWith("47", StringComparison.OrdinalIgnoreCase))
            {
                countryId = CountryConstants.NorwegianCountryId;
                return CountryConstants.NorwegianPhoneCode;
            }
            else if (phoneNumber.StartsWith("49", StringComparison.OrdinalIgnoreCase))
            {
                countryId = CountryConstants.GermanCountryId;
                return CountryConstants.GermanPhoneCode;
            }

            countryId = 0;
            return 0;
        }

        public static string PhoneRemovePrefixes(string phonenumber, int countryId, bool isLandline, out long phoneAsLong, out bool validPhone)
        {
            validPhone = true;
            phoneAsLong = 0;

            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                validPhone = false;
                return "";
            }

            phonenumber = Regex.Replace(phonenumber, "[^0-9]", "").TrimStart('0');

            if (validPhone)
            {
                switch (countryId)
                {
                    case CountryConstants.DanishCountryId:
                        //For Danish numbers we always remove leading 45.
                        if (phonenumber.Length == 10 && phonenumber.Substring(0, 2) == "45")
                        {
                            phonenumber = phonenumber.Substring(2, 8);
                        }

                        validPhone = phonenumber.Length == 8;
                        break;
                    case CountryConstants.SwedishCountryId:
                        if ((isLandline && phonenumber.Length >= 8 && phonenumber.Substring(0, 2) == "46") ||
                            !isLandline && phonenumber.Length > 10 && phonenumber.Substring(0, 2) == "46")
                        {
                            phonenumber = phonenumber.Substring(2);
                        }
                        break;

                    case CountryConstants.FinnishCountryId:
                        if (phonenumber.Length > 10 && phonenumber.Substring(0, 3) == "358")
                            phonenumber = phonenumber.Substring(3);
                        break;
                    case CountryConstants.NorwegianCountryId:
                        if (phonenumber.Length == 10 || phonenumber.Length == 14 && phonenumber.Substring(0, 2) == "47")
                            phonenumber = phonenumber.Substring(2);
                        break;

                }

                if (validPhone)
                {
                    validPhone = long.TryParse(phonenumber, out phoneAsLong);
                }
                return phonenumber;
            }

            return phonenumber;
        }

        /// <summary>
        /// Parses a string representation of a phone number into a long.
        /// </summary>
        /// <param name="phonenumber">Phone nuber as string</param>
        /// <param name="validPhone">ref to boolean for storing value telling if it when well or not</param>
        /// <returns>The phone number as a long OR the number 0 if phone was not valid</returns>
        [Obsolete("We'll make a new and improved version soon")]
        public static long PhoneParseToLong(string phonenumber, int countryId, bool isLandline, out bool validPhone)
        {
            phonenumber = PhoneRemovePrefixes(phonenumber, countryId, isLandline, out long phoneLong, out _);
            validPhone = ValidatePhoneLength(phonenumber, countryId, isLandline);
            if (!validPhone) return 0;
            return phoneLong;
        }

        public static string PhoneParseToString(string phonenumber, int countryId, bool isLandline, out bool validPhone)
        {
            string phone = PhoneAddPrefixes(phonenumber, countryId, isLandline, out _, out validPhone);
            return phone;
        }

        public static bool ValidatePhoneLength(string phoneNumber, int countryId, bool isLandline)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;

            switch (countryId)
            {
                case CountryConstants.DanishCountryId:
                    return phoneNumber.Length == CountryConstants.DanishPhoneLengthMin ||
                        (phoneNumber.Length == CountryConstants.DanishPhoneLengthMax && phoneNumber.StartsWith(CountryConstants.DanishPhoneCode.ToString()));

                case CountryConstants.NorwegianCountryId:
                    return phoneNumber.Length == CountryConstants.NorwegianPhoneLengthMin ||
                        (phoneNumber.Length == CountryConstants.NorwegianPhoneLengthMax && phoneNumber.StartsWith(CountryConstants.NorwegianPhoneCode.ToString()));


                case CountryConstants.SwedishCountryId:
                    if (!isLandline) return phoneNumber.Length >= CountryConstants.SwedishPhoneLengthMin && phoneNumber.Length <= CountryConstants.SwedishPhoneLengthMax;
                    else return phoneNumber.Length >= CountryConstants.SwedishLandlineLengthMin && phoneNumber.Length <= CountryConstants.SwedishLandlineLengthMax;


                case CountryConstants.FinnishCountryId:
                    if (phoneNumber.StartsWith(CountryConstants.FinnishPhoneCode.ToString()))
                    {
                        phoneNumber = phoneNumber[3..];
                    }

                    if (!isLandline)
                    {
                        return phoneNumber.Length >= CountryConstants.FinnishPhoneLengthMin && phoneNumber.Length <= CountryConstants.FinnishPhoneLengthMax;
                    }
                    else
                    {
                        return phoneNumber.Length >= CountryConstants.FinnishLandlineLengthMin && phoneNumber.Length <= CountryConstants.FinnishLandlineLengthMax;
                    }

                case CountryConstants.GermanCountryId:
                    if (phoneNumber.StartsWith(CountryConstants.GermanPhoneCode.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    {
                        phoneNumber = phoneNumber[2..];
                    }

                    return phoneNumber.Length >= CountryConstants.GermanPhoneLengthMin && phoneNumber.Length <= CountryConstants.GermanPhoneLengthMax;

                default:
                    return false;

            }
        }
    }

}
