CREATE TABLE [dbo].[Countries] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (100) NOT NULL,
    [TwoLetterIsoCode]   NVARCHAR (2)   NOT NULL,
    [ThreeLetterIsoCode] NVARCHAR (3)   NOT NULL,
    [NumericIsoCode]     SMALLINT       CONSTRAINT [DF_Countries_NumericIsoCode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON)
);

