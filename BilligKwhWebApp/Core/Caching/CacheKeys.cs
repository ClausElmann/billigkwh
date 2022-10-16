namespace BilligKwhWebApp.Core.Caching
{
    public sealed class CacheKeys
    {
        public const string UserRolesAll = "sms.user.roles.all";
        public const string ProfileRolesAll = "sms.profile.roles.all";
        public const string UserRolesByCustomerAndUser = "sms.user.roles.by.id.{0}.{1}";
        public const string ProfileRolesByProfile = "sms.profile.roles.by.id.{0}";
        public const string UserRolesByUser = "sms.user.roles.user.by.id.{0}.{1}";

        public const string AddressStreetsByCountryZip = "sms.address.streets.countryid.{0}.zip.{1}";
        public const string AddressMunicipalitiesByCountry = "sms.address.municipality.countryid.{0}";

        public const string ZipCodeCityMapByCountry = "sms.address.zipcodes.countryid.{0}";

        public const string Icons = "sms.icons.all";
        public const string GroupItemCoordinates = "sms.groupItems.Coordinates.{0}";

        public const string InfobipNumbers = "sms.infobip.numbers.all";

        public const string EnrollmentSearchKeywords = "enrollment.search.keywords.{0}";

        public const string SamlIdentityProviders = "saml.identityProvider";
    }
}
