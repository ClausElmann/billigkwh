CREATE TABLE [dbo].[MinkAvlsdyr] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [RaekkeID]              INT              CONSTRAINT [DF_MinkKlassificering_RaekkeID] DEFAULT ((0)) NOT NULL,
    [Aar]                   INT              CONSTRAINT [DF_MinkKlassificering_BurNr1] DEFAULT ((0)) NOT NULL,
    [BurNr]                 INT              CONSTRAINT [DF_MinkKlassificering_HoldID] DEFAULT ((0)) NOT NULL,
    [Point]                 SMALLINT         CONSTRAINT [DF_MinkKlassificering_AntalSogriseFjernet] DEFAULT ((0)) NOT NULL,
    [Vaegt]                 SMALLINT         CONSTRAINT [DF_MinkKlassificering_AntalGalteFjernet] DEFAULT ((0)) NOT NULL,
    [Laengde]               SMALLINT         CONSTRAINT [DF_MinkKlassificering_AntalHun1] DEFAULT ((0)) NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkKlassificering_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [Taeve]                 BIT              CONSTRAINT [DF_MinkKlassificering_Taeve] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkKlassificering_Slettet] DEFAULT ((0)) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_MinkKlassificering_DeviceID] DEFAULT ('') NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkKlassificering_ObjektGuid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_MinkKlassificering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkKlassificering_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkKlassificering_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkKlassificering_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkKlassificering_MinkRaekke] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkKlassificering]
    ON [dbo].[MinkAvlsdyr]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkKlassificering]
    ON [dbo].[MinkAvlsdyr]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkKlassificering]
    ON [dbo].[MinkAvlsdyr]([KundeID] ASC, [ID] ASC);

