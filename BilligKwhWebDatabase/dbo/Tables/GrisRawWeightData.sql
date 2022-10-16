CREATE TABLE [dbo].[GrisRawWeightData] (
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [Dato]                DATE           NOT NULL,
    [DeviceID]            NVARCHAR (50)  NOT NULL,
    [VejeholdNr]          SMALLINT       NOT NULL,
    [FoderVaegtID]        INT            CONSTRAINT [DF_GrisRawWeightData_FoderVaegtID] DEFAULT ((0)) NOT NULL,
    [VejeholdID]          INT            CONSTRAINT [DF_GrisRawWeightData_VejeholdID] DEFAULT ((0)) NOT NULL,
    [RawHourData]         NVARCHAR (MAX) CONSTRAINT [DF_GrisRawWeightData_RawHourData] DEFAULT ('') NOT NULL,
    [VandData]            NVARCHAR (MAX) CONSTRAINT [DF_GrisRawWeightData_VandData] DEFAULT ('') NOT NULL,
    [IFVersion]           SMALLINT       NOT NULL,
    [ModtagetStart]       DATETIME       NOT NULL,
    [ModtagetSlut]        DATETIME       NOT NULL,
    [FoderFraVaegt]       INT            CONSTRAINT [DF_GrisRawWeightData_FoderFraVaegt] DEFAULT ((-1)) NOT NULL,
    [FoderTotal]          INT            CONSTRAINT [DF_GrisRawWeightData_FoderTotal] DEFAULT ((-1)) NOT NULL,
    [FoderManuel]         INT            NOT NULL,
    [FoderForbrug]        INT            NOT NULL,
    [VandTotal]           INT            NOT NULL,
    [VandForbrug]         INT            NOT NULL,
    [MixNr]               SMALLINT       NOT NULL,
    [FoderMixVersionID]   INT            CONSTRAINT [DF_GrisRawWeightData_FoderMixVersionID] DEFAULT ((0)) NOT NULL,
    [Vaegt]               INT            NOT NULL,
    [Antal]               INT            CONSTRAINT [DF_GrisRawWeightData_Antal] DEFAULT ((0)) NOT NULL,
    [TilvaekstGramPrGris] INT            CONSTRAINT [DF_GrisRawWeightData_TilvækstGramPrGris] DEFAULT ((0)) NOT NULL,
    [AntalGalte]          SMALLINT       CONSTRAINT [DF_GrisRawWeightData_AntalGalteFjernet] DEFAULT ((0)) NOT NULL,
    [AntalSogrise]        SMALLINT       CONSTRAINT [DF_GrisRawWeightData_AntalSogriseFjernet] DEFAULT ((0)) NOT NULL,
    [VejeholdID1]         INT            CONSTRAINT [DF_GrisRawWeightData_VejeholdID1] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_GrisRawWeightData] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GrisRawWeightData_FoderMixVersion] FOREIGN KEY ([FoderMixVersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_GrisRawWeightData_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_GrisRawWeightData]
    ON [dbo].[GrisRawWeightData]([Dato] ASC, [FoderVaegtID] ASC, [VejeholdID] ASC, [ID] ASC);

