CREATE TABLE [dbo].[MinkIndsaettelse] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [BedriftID]             INT              CONSTRAINT [DF_MinkIndsaettelse_StaldID] DEFAULT ((0)) NOT NULL,
    [RaekkeID]              INT              CONSTRAINT [DF_MinkIndsaettelse_RaekkeID] DEFAULT ((0)) NOT NULL,
    [FoersteBurNr]          INT              CONSTRAINT [DF_MinkIndsaettelse_HoldID] DEFAULT ((0)) NOT NULL,
    [SidsteBurNr]           INT              CONSTRAINT [DF_MinkIndsaettelse_StartBurNr1] DEFAULT ((0)) NOT NULL,
    [Aar]                   INT              CONSTRAINT [DF_MinkIndsaettelse_Aar] DEFAULT ((0)) NOT NULL,
    [AntalDyr]              INT              CONSTRAINT [DF_MinkIndsaettelse_AntalHan] DEFAULT ((0)) NOT NULL,
    [LigeUligeTypeID]       INT              NOT NULL,
    [IndsaettelseTypeID]    INT              NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_MinkIndsaettelse_Bemaerkning] DEFAULT ('') NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkIndsaettelse_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [Indsat]                DATETIME         NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkIndsaettelse_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkIndsaettelse_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkIndsaettelse] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkIndsaettelse_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkIndsaettelse_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkIndsaettelse_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkIndsaettelse_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkIndsaettelse_MinkRaekke] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkIndsaettelse]
    ON [dbo].[MinkIndsaettelse]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkIndsaettelse]
    ON [dbo].[MinkIndsaettelse]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkIndsaettelse]
    ON [dbo].[MinkIndsaettelse]([KundeID] ASC, [ID] ASC);

