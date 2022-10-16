CREATE TABLE [dbo].[Job] (
    [ID]                 INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]            INT              NOT NULL,
    [BedriftID]          INT              NOT NULL,
    [MinkHusID]          INT              CONSTRAINT [DF_Job_HusID] DEFAULT ((0)) NOT NULL,
    [MinkRaekkeID]       INT              CONSTRAINT [DF_Job_RaekkeID] DEFAULT ((0)) NOT NULL,
    [MinkBurNr]          INT              CONSTRAINT [DF_Job_BurNr] DEFAULT ((0)) NOT NULL,
    [StaldID]            INT              CONSTRAINT [DF_Job_StaldID] DEFAULT ((0)) NOT NULL,
    [SektionID]          INT              CONSTRAINT [DF_Job_SektionID] DEFAULT ((0)) NOT NULL,
    [StiID]              INT              CONSTRAINT [DF_Job_StiID] DEFAULT ((0)) NOT NULL,
    [Navn]               NVARCHAR (100)   CONSTRAINT [DF_Job_Beskrivelse1] DEFAULT ('') NOT NULL,
    [Beskrivelse]        NVARCHAR (MAX)   CONSTRAINT [DF_Job_Beskrivelse] DEFAULT ('') NOT NULL,
    [Oprettet]           DATETIME         NOT NULL,
    [OprettetAfBrugerID] INT              NOT NULL,
    [UdforesAfBrugerID]  INT              CONSTRAINT [DF_Job_UdforesAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Udfores]            DATETIME         NOT NULL,
    [Udfort]             DATETIME         NULL,
    [UdfortAfBrugerID]   INT              NULL,
    [UdfortBemaerkning]  NVARCHAR (200)   CONSTRAINT [DF_Job_Navn1] DEFAULT ('') NOT NULL,
    [UdfortTidsforbrug]  FLOAT (53)       CONSTRAINT [DF_Job_Tidsforbrug] DEFAULT ((0)) NOT NULL,
    [Xkoordinat]         DECIMAL (12, 3)  CONSTRAINT [DF_Job_Xkoordinat] DEFAULT ((0)) NOT NULL,
    [Ykoordinat]         DECIMAL (12, 3)  CONSTRAINT [DF_Job_Ykoordinat] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Job_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [SidstRettet]        DATETIME         NOT NULL,
    [Slettet]            BIT              CONSTRAINT [DF_Job_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Job] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Job_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_Job_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Job_MinkHus] FOREIGN KEY ([MinkHusID]) REFERENCES [dbo].[MinkHus] ([ID]),
    CONSTRAINT [FK_Job_MinkRaekke] FOREIGN KEY ([MinkRaekkeID]) REFERENCES [dbo].[MinkRaekke] ([ID]),
    CONSTRAINT [FK_Job_OprettetAfBruger] FOREIGN KEY ([OprettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Job_SidstRettetAfBruger] FOREIGN KEY ([UdfortAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Job_Stald] FOREIGN KEY ([StaldID]) REFERENCES [dbo].[Stald] ([ID]),
    CONSTRAINT [FK_Job_StaldSektion] FOREIGN KEY ([SektionID]) REFERENCES [dbo].[StaldSektion] ([ID]),
    CONSTRAINT [FK_Job_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_Job_UdforesAfBruger] FOREIGN KEY ([UdforesAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Job]
    ON [dbo].[Job]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Job]
    ON [dbo].[Job]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_Job]
    ON [dbo].[Job]([KundeID] ASC, [ID] ASC);

