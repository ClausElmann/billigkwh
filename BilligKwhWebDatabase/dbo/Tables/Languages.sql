CREATE TABLE [dbo].[Languages] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [LanguageCulture]   NVARCHAR (20)  NOT NULL,
    [UniqueSeoCode]     NVARCHAR (2)   NOT NULL,
    [DefaultCurrencyId] INT            CONSTRAINT [DF_Languages_DefaultCurrencyId] DEFAULT ((0)) NOT NULL,
    [Published]         BIT            CONSTRAINT [DF_Languages_Published] DEFAULT ((0)) NOT NULL,
    [DisplayOrder]      INT            CONSTRAINT [DF_Languages_DisplayOrder] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON)
);

