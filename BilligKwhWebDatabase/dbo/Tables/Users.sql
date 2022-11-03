CREATE TABLE [dbo].[Users] (
    [Id]                               INT              IDENTITY (1, 1) NOT NULL,
    [Email]                            NVARCHAR (256)   NOT NULL,
    [Adgangskode]                      NVARCHAR (25)    NOT NULL,
    [CustomerId]                       INT              NOT NULL,
    [Administrator]                    BIT              NOT NULL,
    [SystemAdministrator]              BIT              CONSTRAINT [DF_Users_EnviTronicSysAdmin] DEFAULT ((0)) NOT NULL,
    [Name]                             NVARCHAR (500)   NOT NULL,
    [Phone]                            NVARCHAR (50)    NULL,
    [NoLogin]                          BIT              CONSTRAINT [DF_Users_LoginUser] DEFAULT ((0)) NOT NULL,
    [LanguageId]                       INT              CONSTRAINT [DF_Users_SprogID] DEFAULT ((1)) NOT NULL,
    [CountryId]                        INT              CONSTRAINT [DF_Users_LandID] DEFAULT ((1)) NOT NULL,
    [Password]                         NVARCHAR (200)   CONSTRAINT [DF_Users_Password] DEFAULT ('') NOT NULL,
    [PasswordSalt]                     NVARCHAR (24)    NULL,
    [FailedLoginCount]                 INT              CONSTRAINT [DF_Users_FailedLoginCount] DEFAULT ((0)) NOT NULL,
    [IsLockedOut]                      BIT              CONSTRAINT [DF_Users_IsLockedOut] DEFAULT ((0)) NOT NULL,
    [DateLastFailedLoginUtc]           DATETIME         NULL,
    [DateLastLoginUtc]                 DATETIME         NULL,
    [PasswordResetToken]               UNIQUEIDENTIFIER NULL,
    [DatePasswordResetTokenExpiresUtc] DATETIME         NULL,
    [ImpersonatingUserId]              INT              NULL,
    [TimeZoneId]                       NVARCHAR (50)    CONSTRAINT [DF_Users_TidzoneId] DEFAULT ('Romance Standard Time') NOT NULL,
    [ResetPhone]                       BIGINT           NULL,
    [Deleted]                          BIT              CONSTRAINT [DF_Users_Deleted_1] DEFAULT ((0)) NOT NULL,
    [LastEditedUtc]                    DATETIME         CONSTRAINT [DF_Users_LastEditedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Users1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Users_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
);





