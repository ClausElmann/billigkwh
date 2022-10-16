namespace BilligKwhWebApp.Core
{
    public static class UserRolePermissionProvider
    {
        public const string Bearer = "Bearer";
        public const string SuperAdmin = "SuperAdmin";
        public const string API = "API";
        public const string TwoFactorAuthenticate = "TwoFactorAuthenticate";

    }

    public enum UserRolesEnum
    {
        SuperAdmin = 1,
        ManageUsers = 2,
        CustomerSetup = 3,
        Protected = 4,
    }

    public enum RefType
    {
        Helpdesk = 1,
        ModulDokument = 2,
        BrugerDevice = 3,
        Job = 4,
        Overvaagning = 5,
        ElTavle = 6,
        Bruger = 7
    }

    public enum ProfileRolesEnum
    {
        Undefined,
        
    }
}
