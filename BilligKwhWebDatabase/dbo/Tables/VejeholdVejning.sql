CREATE TABLE [dbo].[VejeholdVejning] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [VejeholdID]            INT              CONSTRAINT [DF_VejeholdVejning_VejeholdID] DEFAULT ((0)) NOT NULL,
    [Dato]                  DATE             CONSTRAINT [DF_VejeholdVejning_Dato] DEFAULT (getdate()) NOT NULL,
    [StartLbNr]             SMALLINT         CONSTRAINT [DF_VejeholdVejning_StartLbNr_1] DEFAULT ((0)) NOT NULL,
    [SlutLbNr]              SMALLINT         CONSTRAINT [DF_VejeholdVejning_StartLbNr] DEFAULT ((-1)) NOT NULL,
    [StartAntalGalte]       SMALLINT         CONSTRAINT [DF_VejeholdVejning_Antal1] DEFAULT ((0)) NOT NULL,
    [SlutAntalGalte]        SMALLINT         CONSTRAINT [DF_VejeholdVejning_Antal2] DEFAULT ((0)) NOT NULL,
    [StartAntalSogrise]     SMALLINT         CONSTRAINT [DF_VejeholdVejning_StartAntal1] DEFAULT ((0)) NOT NULL,
    [SlutAntalSogrise]      SMALLINT         CONSTRAINT [DF_VejeholdVejning_SlutAntal1] DEFAULT ((0)) NOT NULL,
    [StartVaegt]            INT              CONSTRAINT [DF_VejeholdVejning_StartVaegt] DEFAULT ((0)) NOT NULL,
    [SlutVaegt]             INT              NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_VejeholdVejning_Bemaerkning] DEFAULT ('') NOT NULL,
    [FoderMixVersionID]     INT              CONSTRAINT [DF_VejeholdVejning_FoderMixVersionID] DEFAULT ((0)) NOT NULL,
    [Foder]                 INT              CONSTRAINT [DF_VejeholdVejning_Foder] DEFAULT ((0)) NOT NULL,
    [DagligTilvaekstGram]   INT              CONSTRAINT [DF_VejeholdVejning_DagligTilvækstGram] DEFAULT ((0)) NOT NULL,
    [EffektivitetFoder]     FLOAT (53)       CONSTRAINT [DF_VejeholdVejning_EffektivitetFoder] DEFAULT ((0)) NOT NULL,
    [EffektivitetGenerelt]  FLOAT (53)       CONSTRAINT [DF_VejeholdVejning_EffektivitetGenerelt] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_VejeholdVejning_Slettet] DEFAULT ((0)) NOT NULL,
    [GnsFoderForbrugGram]   INT              CONSTRAINT [DF_VejeholdVejning_GnsTilvaekstGram1] DEFAULT ((0)) NOT NULL,
    [AkkTilv]               INT              CONSTRAINT [DF_VejeholdVejning_AkkTilv] DEFAULT ((0)) NOT NULL,
    [FEsvDyrDag]            DECIMAL (7, 2)   CONSTRAINT [DF_VejeholdVejning_FEsvDyrDag] DEFAULT ((0)) NOT NULL,
    [FEsvKgTilv]            DECIMAL (7, 2)   CONSTRAINT [DF_VejeholdVejning_FEsv_KgTilv] DEFAULT ((0)) NOT NULL,
    [AkkFEsvKgTilv]         DECIMAL (7, 2)   CONSTRAINT [DF_VejeholdVejning_AkkTilv1] DEFAULT ((0)) NOT NULL,
    [DkrKgTilv]             DECIMAL (7, 2)   CONSTRAINT [DF_VejeholdVejning_FEsv_KgTilv1] DEFAULT ((0)) NOT NULL,
    [KgM2]                  INT              CONSTRAINT [DF_VejeholdVejning_Kg/m2] DEFAULT ((0)) NOT NULL,
    [FoderTotal]            INT              CONSTRAINT [DF_VejeholdVejning_FoderTotal] DEFAULT ((0)) NOT NULL,
    [GnsTilvaekstGram]      INT              CONSTRAINT [DF_VejeholdVejning_GnsTilvækstGram] DEFAULT ((0)) NOT NULL,
    [SlutAntal]             INT              CONSTRAINT [DF_VejeholdVejning_SlutAntal] DEFAULT ((0)) NOT NULL,
    [DageImellem]           SMALLINT         CONSTRAINT [DF_VejeholdVejning_DageImellem] DEFAULT ((0)) NOT NULL,
    [SlutDag]               SMALLINT         CONSTRAINT [DF_VejeholdVejning_Dag] DEFAULT ((0)) NOT NULL,
    [SlutAlder]             SMALLINT         CONSTRAINT [DF_VejeholdVejning_Alder] DEFAULT ((0)) NOT NULL,
    [BeregningLog]          NVARCHAR (MAX)   CONSTRAINT [DF_VejeholdVejning_Bemaerkning1] DEFAULT ('') NULL,
    [ManFoder]              INT              NULL,
    [ManVand]               INT              NULL,
    [ManMixNr]              SMALLINT         NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_VejeholdVejning_NotatGuid] DEFAULT (newid()) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_VejeholdVejning_DeviceID] DEFAULT ('') NULL,
    CONSTRAINT [PK_VejeholdVejning] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_VejeholdVejning_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_VejeholdVejning_FoderMixVersion] FOREIGN KEY ([FoderMixVersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_VejeholdVejning_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_VejeholdVejning_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_VejeholdVejning]
    ON [dbo].[VejeholdVejning]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_VejeholdVejning]
    ON [dbo].[VejeholdVejning]([KundeID] ASC, [VejeholdID] ASC, [Slettet] ASC, [ID] ASC);

