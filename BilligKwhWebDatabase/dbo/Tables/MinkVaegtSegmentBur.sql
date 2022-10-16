CREATE TABLE [dbo].[MinkVaegtSegmentBur] (
    [ID]                    INT      NOT NULL,
    [KundeID]               INT      NOT NULL,
    [SegmentID]             INT      CONSTRAINT [DF_MinkVaegtSegmentBur_MinkVaegtSegmentID] DEFAULT ((0)) NOT NULL,
    [BurID]                 INT      NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_MinkVægtBur_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_MinkVægtBur_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_MinkVægtBur_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkVægtBur] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVaegtBur_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkVaegtBur_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkVaegtBur_MinkBur] FOREIGN KEY ([BurID]) REFERENCES [dbo].[MinkBur] ([ID]),
    CONSTRAINT [FK_MinkVaegtSegmentBur_MinkVaegtSegment] FOREIGN KEY ([SegmentID]) REFERENCES [dbo].[MinkVaegtSegment] ([ID])
);

