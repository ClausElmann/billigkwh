CREATE TABLE [dbo].[MinkManVejning] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [Dato]                  DATETIME         NOT NULL,
    [RaekkeID]              INT              CONSTRAINT [DF_MinkManVejning_RaekkeID] DEFAULT ((0)) NOT NULL,
    [BurNr]                 INT              CONSTRAINT [DF_MinkManVejning_HoldID] DEFAULT ((0)) NOT NULL,
    [HanVaegt]              SMALLINT         CONSTRAINT [DF_MinkManVejning_AntalHan] DEFAULT ((0)) NOT NULL,
    [HunVaegt]              SMALLINT         CONSTRAINT [DF_MinkManVejning_AntalHun] DEFAULT ((0)) NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkManVejning_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [MinkManVejeholdID]     INT              CONSTRAINT [DF_MinkManVejning_MinkManVejeholdID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkManVejning_Slettet] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkManVejning_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [DeviceID]              NVARCHAR (50)    NULL,
    CONSTRAINT [PK_MinkManVejning] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkManVejning_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkManVejning_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkManVejning_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkManVejning_MinkManVejehold] FOREIGN KEY ([MinkManVejeholdID]) REFERENCES [dbo].[MinkManVejehold] ([ID]),
    CONSTRAINT [FK_MinkManVejning_MinkRaekke] FOREIGN KEY ([RaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MinkVejehold]
    ON [dbo].[MinkManVejning]([MinkManVejeholdID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkManVejning]
    ON [dbo].[MinkManVejning]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkManVejning]
    ON [dbo].[MinkManVejning]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkManVejning]
    ON [dbo].[MinkManVejning]([KundeID] ASC, [RaekkeID] ASC, [BurNr] ASC, [Dato] ASC);

