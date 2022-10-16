CREATE TABLE [dbo].[MinkVejehold] (
    [ID]                       INT            IDENTITY (1, 1) NOT NULL,
    [DeviceID]                 NVARCHAR (25)  CONSTRAINT [DF_MinkVaegt_DeviceUid] DEFAULT ('') NOT NULL,
    [KundeID]                  INT            NOT NULL,
    [Navn]                     NVARCHAR (50)  CONSTRAINT [DF_MinkVejehold_Navn] DEFAULT ('') NOT NULL,
    [StartDato]                DATE           NOT NULL,
    [SlutDato]                 DATE           NULL,
    [Aar]                      INT            CONSTRAINT [DF_MinkVejehold_Aar] DEFAULT ((2017)) NOT NULL,
    [DyreTypeID]               INT            CONSTRAINT [DF_MinkVejehold_DyreTypeID] DEFAULT ((0)) NOT NULL,
    [HoldTypeID]               SMALLINT       CONSTRAINT [DF_MinkVejehold_HoldTypeID] DEFAULT ((3)) NOT NULL,
    [Beskrivelse]              NVARCHAR (MAX) CONSTRAINT [DF_MinkVaegt_Beskrivelse] DEFAULT ('') NOT NULL,
    [RaekkeID]                 INT            CONSTRAINT [DF_MinkVaegt_RaekkeID] DEFAULT ((0)) NOT NULL,
    [Bur1Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur1ID1] DEFAULT ((0)) NOT NULL,
    [Bur2Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur2ID1] DEFAULT ((0)) NOT NULL,
    [Bur3Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur3ID1] DEFAULT ((0)) NOT NULL,
    [Bur4Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur4ID1] DEFAULT ((0)) NOT NULL,
    [Bur5Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur5ID1] DEFAULT ((0)) NOT NULL,
    [Bur6Nr]                   SMALLINT       CONSTRAINT [DF_MinkVaegt_Bur6ID1] DEFAULT ((0)) NOT NULL,
    [SidstRettet]              DATETIME       CONSTRAINT [DF_MinkVaegt_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]    INT            CONSTRAINT [DF_MinkVaegt_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]                  BIT            CONSTRAINT [DF_MinkVaegt_Slettet] DEFAULT ((0)) NOT NULL,
    [Placering]                INT            CONSTRAINT [DF_MinkVejehold_Placering] DEFAULT ((0)) NOT NULL,
    [SlankeKurveID]            INT            CONSTRAINT [DF_MinkVejehold_SlankeKurveID] DEFAULT ((0)) NOT NULL,
    [FoderStyrkeNedStartDato]  DATE           NULL,
    [HanGnsTilv7dage]          SMALLINT       NULL,
    [HunGnsTilv7dage]          SMALLINT       NULL,
    [HanGnsTilv2og3dage]       SMALLINT       NULL,
    [HunGnsTilv2og3dage]       SMALLINT       NULL,
    [HanGnsTilvIgaar]          SMALLINT       NULL,
    [HunGnsTilvIgaar]          SMALLINT       NULL,
    [HanGnsSidste3dageVaegtet] SMALLINT       NULL,
    [HunGnsSidste3dageVaegtet] SMALLINT       NULL,
    [SidstOnline]              DATETIME       NULL,
    [Overvaagning]             BIT            CONSTRAINT [DF_MinkVejehold_Overvaagning] DEFAULT ((1)) NULL,
    [GensendFraID]             INT            CONSTRAINT [DF_MinkVaegt_HentFraID] DEFAULT ((0)) NULL,
    [OpsaetningData]           NVARCHAR (MAX) CONSTRAINT [DF_MinkVaegt_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]       DATETIME       CONSTRAINT [DF_MinkVaegt_SidstRettet1] DEFAULT (getdate()) NULL,
    [OrdreNr]                  INT            CONSTRAINT [DF_MinkVaegt_OrdreNr] DEFAULT ((0)) NULL,
    [OldID]                    INT            NULL,
    [SlutDatoKopi]             DATE           NULL,
    CONSTRAINT [PK_MinkVaegt] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVaegt_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkVaegt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkVaegt_MinkRaekke1] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID]),
    CONSTRAINT [FK_MinkVejehold_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID]),
    CONSTRAINT [FK_MinkVejehold_MinkSlankeKurve] FOREIGN KEY ([SlankeKurveID]) REFERENCES [dbo].[MinkSlankeKurve] ([ID]),
    CONSTRAINT [FK_MinkVejehold_MinkVejeholdholdType] FOREIGN KEY ([HoldTypeID]) REFERENCES [dbo].[MinkVejeholdholdType] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_DeviceAarHoldType_DeviceNotNulOrMinusOneOrSlettet]
    ON [dbo].[MinkVejehold]([DeviceID] ASC, [Aar] ASC, [HoldTypeID] ASC) WHERE ([DeviceID]<>'0' AND [DeviceID]<>'-1' AND [Slettet]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_MinkVaegt]
    ON [dbo].[MinkVejehold]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkVejehold]
    ON [dbo].[MinkVejehold]([KundeID] ASC, [ID] ASC);

