CREATE TABLE [dbo].[Customers] (
    [Id]                    INT              IDENTITY (1, 1) NOT NULL,
    [PublicId]              UNIQUEIDENTIFIER CONSTRAINT [DF_Customers_PublicId] DEFAULT (newid()) NOT NULL,
    [Name]                  NVARCHAR (200)   NOT NULL,
    [Address]               NVARCHAR (MAX)   CONSTRAINT [DF_Customers_Address] DEFAULT ('') NOT NULL,
    [CountryId]             INT              CONSTRAINT [DF_Customers_CountryId] DEFAULT ((1)) NOT NULL,
    [Active]                BIT              CONSTRAINT [DF_User_Customer_Active] DEFAULT ((1)) NOT NULL,
    [DateCreatedUtc]        DATETIME         CONSTRAINT [DF_Customers_DateCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [DateDeletedUtc]        DATETIME         NULL,
    [Deleted]               BIT              CONSTRAINT [DF_Customers_Deleted] DEFAULT ((0)) NOT NULL,
    [DateLastUpdatedUtc]    DATETIME         CONSTRAINT [DF__Customers__DateL__1B6CA850] DEFAULT (getutcdate()) NOT NULL,
    [TimeZoneId]            NVARCHAR (100)   CONSTRAINT [DF_Customers_TimeZoneId] DEFAULT ('Romance Standard Time') NOT NULL,
    [LanguageId]            INT              CONSTRAINT [DF_Customers_LanguageId] DEFAULT ((1)) NOT NULL,
    [CompanyRegistrationId] NVARCHAR (50)    CONSTRAINT [DF_Customers_CompanyRegistrationId] DEFAULT ('') NOT NULL,
    [HourWage]              INT              CONSTRAINT [DF_Customers_HourWage] DEFAULT ((400)) NOT NULL,
    [CoveragePercentage]    INT              CONSTRAINT [DF_Customers_CoveragePercentage] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__User_Cus__3214EC078855EB1D] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON)
);

