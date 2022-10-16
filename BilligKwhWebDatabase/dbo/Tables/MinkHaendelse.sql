CREATE TABLE [dbo].[MinkHaendelse] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [BedriftID]             INT              CONSTRAINT [DF_MinkHaendelse_StaldID] DEFAULT ((0)) NOT NULL,
    [HusID]                 INT              CONSTRAINT [DF_MinkHaendelse_SektionID] DEFAULT ((0)) NOT NULL,
    [RaekkeID]              INT              CONSTRAINT [DF_MinkHaendelse_RaekkeID] DEFAULT ((0)) NOT NULL,
    [BurNr]                 INT              CONSTRAINT [DF_MinkHaendelse_HoldID] DEFAULT ((0)) NOT NULL,
    [MinkVaegtholdID]       INT              CONSTRAINT [DF_MinkHaendelse_VejeholdID] DEFAULT ((0)) NOT NULL,
    [MinkHaendelseTypeID]   INT              CONSTRAINT [DF_MinkHaendelse_MinkHaendelseTypeID] DEFAULT ((0)) NOT NULL,
    [AntalHan]              INT              CONSTRAINT [DF_MinkHaendelse_AntalHan] DEFAULT ((0)) NOT NULL,
    [AntalHun]              INT              CONSTRAINT [DF_MinkHaendelse_AntalHun] DEFAULT ((0)) NOT NULL,
    [AntalDreng]            INT              CONSTRAINT [DF_MinkHaendelse_AntalHan1] DEFAULT ((0)) NOT NULL,
    [AntalPige]             INT              CONSTRAINT [DF_MinkHaendelse_AntalHun1] DEFAULT ((0)) NOT NULL,
    [MedicinID]             INT              CONSTRAINT [DF_MinkHaendelse_MedicinID] DEFAULT ((0)) NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_MinkHaendelse_Bemaerkning] DEFAULT ('') NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkHaendelse_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [AarsagID]              INT              CONSTRAINT [DF_MinkHaendelse_DoedAarsagListepostID] DEFAULT ((0)) NOT NULL,
    [Dato]                  DATETIME         CONSTRAINT [DF_MinkHaendelse_Dato] DEFAULT (getdate()) NOT NULL,
    [Oprettet]              DATETIME         NULL,
    [SeasonID]              INT              CONSTRAINT [DF_MinkHaendelse_SeasonID] DEFAULT ((0)) NOT NULL,
    [AntalDyr]              INT              NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkHaendelse_Slettet] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkHaendelse_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_MinkHaendelse_DeviceID] DEFAULT ('') NULL,
    [Hanner]                INT              CONSTRAINT [DF_MinkHaendelse_AntalHan1_1] DEFAULT ((0)) NOT NULL,
    [Hunner]                INT              CONSTRAINT [DF_MinkHaendelse_AntalHun1_1] DEFAULT ((0)) NOT NULL,
    [Drenge]                INT              CONSTRAINT [DF_MinkHaendelse_AntalDreng1] DEFAULT ((0)) NOT NULL,
    [Piger]                 INT              CONSTRAINT [DF_MinkHaendelse_Piger] DEFAULT ((0)) NOT NULL,
    [ErAflivet]             BIT              CONSTRAINT [DF_MinkHaendelse_ErAflivet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkHaendelse] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkHaendelse_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_EventType] FOREIGN KEY ([MinkHaendelseTypeID]) REFERENCES [dbo].[MinkHaendelseType] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_Medicin] FOREIGN KEY ([MedicinID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_MinkHus] FOREIGN KEY ([HusID]) REFERENCES [dbo].[MinkHus] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_MinkRaekke] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_MinkSeason] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[MinkSeason] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_MinkVejehold] FOREIGN KEY ([MinkVaegtholdID]) REFERENCES [dbo].[MinkVaegthold] ([ID]),
    CONSTRAINT [FK_MinkHaendelse_Aarsag] FOREIGN KEY ([AarsagID]) REFERENCES [dbo].[ListePost] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkHaendelse]
    ON [dbo].[MinkHaendelse]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkHaendelse]
    ON [dbo].[MinkHaendelse]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkHaendelse]
    ON [dbo].[MinkHaendelse]([KundeID] ASC, [ID] ASC);

