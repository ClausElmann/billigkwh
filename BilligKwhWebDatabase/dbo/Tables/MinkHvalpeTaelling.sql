CREATE TABLE [dbo].[MinkHvalpeTaelling] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [RaekkeID]              INT              CONSTRAINT [DF_MinkHvalpeTaelling_RaekkeID] DEFAULT ((0)) NOT NULL,
    [Aar]                   INT              CONSTRAINT [DF_MinkHvalpeTaelling_BurNr1] DEFAULT ((0)) NOT NULL,
    [BurNr]                 INT              CONSTRAINT [DF_MinkHvalpeTaelling_HoldID] DEFAULT ((0)) NOT NULL,
    [Foedt]                 DATETIME         NULL,
    [AntalHan]              SMALLINT         CONSTRAINT [DF_MinkHvalpeTaelling_AntalHan] DEFAULT ((0)) NOT NULL,
    [AntalHun]              SMALLINT         CONSTRAINT [DF_MinkHvalpeTaelling_AntalHun] DEFAULT ((0)) NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkHvalpeTaelling_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkHvalpeTaelling_Slettet] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkHvalpeTaelling_ObjektGuid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_MinkHvalpeTaelling] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkHvalpeTaelling_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkHvalpeTaelling_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkHvalpeTaelling_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkHvalpeTaelling_MinkRaekke] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkHvalpeTaelling]
    ON [dbo].[MinkHvalpeTaelling]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkHvalpeTaelling]
    ON [dbo].[MinkHvalpeTaelling]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkHvalpeTaelling]
    ON [dbo].[MinkHvalpeTaelling]([KundeID] ASC, [ID] ASC);

