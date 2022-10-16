using System;

namespace BilligKwhWebApp.Core.Domain
{
    public static class CountryConstants
    {
        public const int DanishCountryId = 1;
        public const int SwedishCountryId = 2;
        public const int EnglishCountryId = 3;
        public const int FinnishCountryId = 4;
        public const int NorwegianCountryId = 5;
        public const int GermanCountryId = 6;

        public const int DanishLanguageId = 1;
        public const int SwedishLanguageId = 2;
        public const int EnglishLanguageId = 3;
        public const int FinnishLanguageId = 4;
        public const int NorwegianLanguageId = 5;
        public const int GermanLanguageId = 6;

        public const int DanishPhoneCode = 45;
        public const int SwedishPhoneCode = 46;
        public const int NorwegianPhoneCode = 47;
        public const int FinnishPhoneCode = 358;
        public const int EnglishPhoneCode = 44;
        public const int GermanPhoneCode = 49;

        public const int DanishPhoneLengthMin = 8;
        public const int DanishPhoneLengthMax = 10;
        public const int NorwegianPhoneLengthMin = 8;
        public const int NorwegianPhoneLengthMax = 10;
        public const int SwedishPhoneLengthMin = 9;
        public const int SwedishPhoneLengthMax = 11;
        public const int SwedishLandlineLengthMin = 6;
        public const int SwedishLandlineLengthMax = 12;
        public const int FinnishPhoneLengthMin = 6;
        public const int FinnishPhoneLengthMax = 10;
        public const int FinnishLandlineLengthMin = 8;
        public const int FinnishLandlineLengthMax = 10;

        public const int GermanPhoneLengthMin = 7;
        public const int GermanPhoneLengthMax = 12;

        public static string GetCultureString(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => "da-DK",
                SwedishCountryId => "sv-SE",
                EnglishCountryId => "en-GB",
                FinnishCountryId => "fi-FE",
                NorwegianCountryId => "nb-NO",
                _ => "da-DK",
            };
        }

        public static string GetCountryCode(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => "dk",
                SwedishCountryId => "se",
                EnglishCountryId => "gb",
                FinnishCountryId => "fi",
                NorwegianCountryId => "no",
                _ => "dk",
            };
        }

        public static string GetLanguageName(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => "Danish",
                SwedishCountryId => "Swedish",
                EnglishCountryId => "English",
                FinnishCountryId => "Finnish",
                NorwegianCountryId => "Norwegian",
                _ => "Danish"
            };
        }

        public static string GetCountryTranslateKey(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => "shared.Denmark",
                SwedishCountryId => "shared.Sweden",
                EnglishCountryId => "shared.English",
                FinnishCountryId => "shared.Finland",
                NorwegianCountryId => "shared.Norway",
                _ => "shared.Denmark",
            };
        }

        public static string GetCountryName(int countryId)
        {
            switch (countryId)
            {
                case DanishCountryId: return "Danmark";
                case SwedishCountryId: return "Sverige";
                case EnglishCountryId: return "England";
                case FinnishCountryId: return "Finland";
                case NorwegianCountryId: return "Norge";
                default: return "Danmark";
            }
        }

        public static int GetCountryId(int phoneCode)
        {
            return phoneCode switch
            {
                DanishPhoneCode => DanishCountryId,
                SwedishPhoneCode => SwedishCountryId,
                EnglishPhoneCode => EnglishCountryId,
                FinnishPhoneCode => FinnishCountryId,
                NorwegianPhoneCode => NorwegianCountryId,
                _ => DanishCountryId,
            };
        }

        public static int GetPhoneCode(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => DanishPhoneCode,
                SwedishCountryId => SwedishPhoneCode,
                FinnishCountryId => FinnishPhoneCode,
                EnglishCountryId => EnglishPhoneCode,
                NorwegianCountryId => NorwegianPhoneCode,
                GermanCountryId => GermanPhoneCode,
                _ => DanishPhoneCode,
            };
        }

        public static int GetLanguageId(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => DanishLanguageId,
                SwedishCountryId => SwedishLanguageId,
                EnglishCountryId => EnglishLanguageId,
                FinnishCountryId => FinnishLanguageId,
                NorwegianCountryId => NorwegianLanguageId,
                _ => DanishLanguageId,
            };
        }

        /// <summary>
        /// Returns the required length of zipcodes based on country
        /// </summary>
        public static int GetZipcodeLength(int countryId)
        {
            switch (countryId)
            {
                case DanishCountryId:
                case NorwegianCountryId:
                case EnglishCountryId:
                    return 4;

                case SwedishCountryId:
                case FinnishCountryId:
                    return 5;

                default: return 4;
            }
        }

        private static readonly TimeZoneInfo westernEuropeanTimeZone = TimeZoneConverter.TZConvert.GetTimeZoneInfo("W. Europe Standard Time");
        private static readonly TimeZoneInfo easternEuropeanTimeZone = TimeZoneConverter.TZConvert.GetTimeZoneInfo("E. Europe Standard Time");
        private static readonly TimeZoneInfo gmtTimeZone = TimeZoneConverter.TZConvert.GetTimeZoneInfo("GMT Standard Time");

        public static TimeZoneInfo GetTimeZone(int countryId)
        {
            return countryId switch
            {
                DanishCountryId => westernEuropeanTimeZone,
                SwedishCountryId => westernEuropeanTimeZone,
                EnglishCountryId => westernEuropeanTimeZone,
                FinnishCountryId => easternEuropeanTimeZone,
                NorwegianCountryId => westernEuropeanTimeZone,
                _ => gmtTimeZone
            };
        }

        public static string GetStandardReceiverText(int countryId)
        {
            switch (countryId)
            {
                case DanishCountryId: return "Standard modtager";
                case SwedishCountryId: return "Standardmottagare";
                case EnglishCountryId: return "Standard reciver";
                case FinnishCountryId: return "Määritä oletusvastaanottajia";
                case NorwegianCountryId: return "Standard receiver";
                default: return "Standard receiver";
            }
        }
    }
}
