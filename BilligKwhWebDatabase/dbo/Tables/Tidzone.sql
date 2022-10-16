CREATE TABLE [dbo].[Tidzone] (
    [ID]           SMALLINT      IDENTITY (1, 1) NOT NULL,
    [TimeZoneID]   VARCHAR (100) NOT NULL,
    [DisplayNavn]  VARCHAR (100) NOT NULL,
    [StandardNavn] VARCHAR (100) NOT NULL,
    [HarSommertid] BIT           NOT NULL,
    [UTCOffset]    INT           NOT NULL,
    CONSTRAINT [PK_TimeZones] PRIMARY KEY CLUSTERED ([ID] ASC)
);

