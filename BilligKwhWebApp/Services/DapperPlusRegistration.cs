﻿using Z.Dapper.Plus;
using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services
{
    public static class DapperPlusRegistration
    {
        public static void SetupDapperPlus(string callerModule)
        {
            var licenseName = "2181;701-BlueIdea";
            var licenseKey = "3b440c25-198d-0ddf-072f-5cd7a0be1e67";
            DapperPlusManager.AddLicense(licenseName, licenseKey);

            // CHECK if the license if valid for a specific provider
            if (!DapperPlusManager.ValidateLicense(out var licenseErrorMessage, DapperProviderType.SqlServer))
            {
                var logMsg = $"{licenseErrorMessage}  ERROR running: {callerModule} ";
                //AzureBlobContainer.UploadStringToFile(BlobContainerEnum.errorlogs, "DapperLicense_ERROR.log", logMsg).GetAwaiter().GetResult();
            }
            else
            {
                // Global setting for dapper bulk operations. 
                DapperPlusManager.MapperFactory = mapper => { mapper.BatchDelayInterval(1000); mapper.BatchSize(10000); mapper.BatchTimeout(10 * 60); };
                SetupCustomerTables();
                SetupUserTables();
                SetupSystemTables();
                SetupRequestLog();
                SetupEmailTables();
                SetupMiscTables();
            }
        }

        private static void SetupRequestLog()
        {
            DapperPlusManager.Entity<RequestLog>().Table("RequestLogs").Identity(x => x.Id);
        }

        private static void SetupSystemTables()
        {
            DapperPlusManager.Entity<LocaleStringResource>().Table("dbo.LocaleStringResources").Identity(x => x.Id);
            DapperPlusManager.Entity<SensitivePageLoad>().Table("SensitivePageLoads").Identity(x => x.Id);
            DapperPlusManager.Entity<ApplicationSetting>().Table("dbo.ApplicationSettings").Identity(x => x.Id);
            DapperPlusManager.Entity<Log>().Table("dbo.Logs").Identity(x => x.Id);
        }

        private static void SetupUserTables()
        {
            DapperPlusManager.Entity<Bruger>().Table("dbo.Bruger").Identity(x => x.Id).Ignore(i => i.FuldtNavn);
            DapperPlusManager.Entity<UserRole>().Table("dbo.UserRoles").Identity(x => x.Id);
            DapperPlusManager.Entity<UserRoleMapping>().Table("dbo.UserRoleMappings").Identity(x => x.Id);
            DapperPlusManager.Entity<UserRefreshToken>().Table("dbo.UserRefreshTokens").Identity(i => i.Id);
            DapperPlusManager.Entity<PinCode>().Table("PinCodes").Identity(x => x.Id);
        }

        private static void SetupCustomerTables()
        {
            DapperPlusManager.Entity<Kunde>().Table("dbo.Kunde").Identity(i => i.Id);
            DapperPlusManager.Entity<CustomerUserMapping>().Table("dbo.CustomerUserMappings").Identity(i => i.Id);
            DapperPlusManager.Entity<CustomerUserRoleMapping>().Table("dbo.CustomerUserRoleMappings").Identity(i => i.Id);
        }


        private static void SetupEmailTables()
        {
            DapperPlusManager.Entity<EmailMessage>().Table("dbo.EmailMessages").Identity(x => x.Id);
            DapperPlusManager.Entity<EmailAttachment>().Table("dbo.EmailAttachments").Identity(x => x.Id);
            DapperPlusManager.Entity<EmailTemplate>().Table("dbo.EmailTemplates").Identity(x => x.Id);
        }
        private static void SetupMiscTables()
        {
            DapperPlusManager.Entity<Indstilling>().Table("dbo.Indstilling").Identity(x => x.Id);
            DapperPlusManager.Entity<ElTavleSektionElKomponent>().Table("dbo.ElTavleSektionElKomponent").Identity(x => x.Id);
            DapperPlusManager.Entity<ElTavleSektion>().Table("dbo.ElTavleSektion").Identity(x => x.Id);
            DapperPlusManager.Entity<Dokument>().Table("dbo.Dokument").Key(x => x.Id);
            DapperPlusManager.Entity<ElTavle>().Table("dbo.ElTavle").Identity(x => x.Id);
            DapperPlusManager.Entity<ElTavleLaageElKomponent>().Table("dbo.ElTavleLaageElKomponent").Identity(x => x.Id);
            DapperPlusManager.Entity<ElTavleLaage>().Table("dbo.ElTavleLaage").Identity(x => x.Id);
            DapperPlusManager.Entity<ElPris>().Table("dbo.ElPris").Identity(x => x.Id).Key(k => new { k.DatoUtc, k.TimeDk }).BatchTimeout(120 * 60);
        }
    }
}