namespace BilligKwhWebApp.Services.Enums
{
    public enum EmailCategoryEnum
    {
        SupportMails = 1,
        //OrderMails = 2,
        PasswordMails = 3,
        //StatstidendeMails = 4,
        //BroadcastReceiptMails = 5,
        //PosListMails = 6,
        //NewsletterMails = 7,
        //BroadcastApproval = 8,
        //SubscriptionMails = 9,
        //ReadyMails = 10,
    }

  

    //public enum UserRoleName
    //{
    //    None = 0,
    //    SuperAdmin = 1,
    //    ManageUsers = 2,
    //    ManageProfiles = 3,
    //    CanCreateScheduledBroadcasts = 13,
    //    ManageReports = 14,
    //    ManageMessages = 15,
    //    ManageBenchmarks = 17,
    //    ManageCustomer = 19,
    //    Benchmark = 21,
    //    CustomerSetup = 22,
    //    Searching = 23,
    //    WEBMessages = 24,
    //    StandardReceivers = 25,
    //    SubscriptionModule = 26,
    //    MessageTemplates = 27,
    //    CanSetupStatstidende = 28,
    //    API = 29,
    //    CanSetupSubscriptionReminders = 30,
    //    Protected = 31,
    //    TwoFactorAuthenticate = 32,
    //    ADLogin = 33,
    //    RequiresApproval = 34,
    //    WeatherWarning = 35,
    //    AlwaysTestMode = 36,

    //}

    public enum Cc_Bcc
    {
        CC,
        BCC
    }

    /// <summary>
    /// Enum defining the names of the claims that is added to the JWT access token token 
    /// </summary>
    public enum AccessTokenClaims
    {
        UserId,
        ProfileId,
        CustomerId,
        ImpersonateFromUserId
    }

    public enum EmailStatus
    {
        Queued,
        Importing,
        SendingToGateway

    }

    public enum EmailTemplateName
    {
        None,
        MasterTemplate,
        ResetPassword,
        NewUser,
        AdminOprettelse,
        BrugerOprettelse,
        TakForDinBestilling,
        ViHarNuSendtDinTavle,
        Faktura,
        ElectricityPricesMissing
    }

    public enum AppSettingEnum
    {
        Undefined = 0,
        SendGridAPIKey = 1,
        BatchAppCommandLine = 2,
        RequestLogLevel = 3,
        KomponentTimeLoen = 4,
    }

    public enum IconTypeEnum
    {
        MapModule
    }

    public enum Komponent
    {
        D02_Forsikring = 32,
        Transient_beskyttelse = 31,
        UML = 42,
        UMS = 43,

        UG6 = 46,
        UG12 = 47,
        UG18 = 48,
        UG24 = 49,

        Fragt = 54,

        PGE7 = 67,
        PGE14 = 68,
        PGE21 = 69,

        HG13 = 71,
        HG26 = 72,
        HG39 = 73,
        HG52 = 74,

        HGV12 = 75,
        HGV24 = 76,
        HGV36 = 77,
        HGV48 = 78,

        SR13 = 79,
        SR26 = 80,
        SR39 = 81,
        SR52 = 82,

        HV12 = 85,
        HV24 = 86,
        HV36 = 87,
        HV48 = 88,
    }

    public enum KomponentKategori
    {
        D02_Forsikring = 1,
        Transient_beskyttelse = 2,
        HPFI = 5,
        LK_Ug_150 = 7,
        UM = 8,
        Fragt = 10,
        LK_PGE_Planforsænket_med_låg = 11,
        Hager_Gamma = 12,
        Hager_Volta_Planforsænket_med_låg = 13,
        Sneider_Resi9 = 14,
        Hager_Vector_IP_65 = 15,
        Automat_sikringer_3pn = 104,
        Automat_sikringer_1pn = 105,
        Kombirelæ_3pn = 106,
        Kombirelæ_1pn = 107,
        Øvrige_komponenter = 110,
    }


    public enum KredsKomponentKategori
    {
        DISP = -1,
        HPFI = 1,
        Kombirelæ = 2,
        Automat_sikring_3p = 3,
        Sikring = 4,
        Måler = 5,
        Transient_beskyttelse = 6,
        Energi_maaler = 7, 
        Automat_sikring_1p = 8,
        Ur = 10,
        Kontaktor = 11,
        Kiprelæ = 12,
        MCB_lille = 19,
        MCB = 20,
    }

    public enum Tavlefabrikat
    {
        Ikke___angivet = 0,
        LK_UG_150 = 1,
        LK_PGE___planforsænket_med_låg = 2,
        Hager_Gamma = 3,
        Hager___planforsænket_med_låg = 4,
        Schneider_Resi9 = 5,
        Hager_Vector_IP_65 = 6
    }

    public enum SektionType
    {
        Indgang = 0,
        HPFI = 1,
        KombiRelae = 2,
        UdenBeskyttelse = 3,
    }
}
