CREATE TABLE [dbo].[MinkVaegtSegment] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      NOT NULL,
    [Nr]                    SMALLINT NOT NULL,
    [HusID]                 INT      CONSTRAINT [DF_MinkVaegtSegment_HusID] DEFAULT ((0)) NOT NULL,
    [BoxID]                 INT      CONSTRAINT [DF_MinkVaegtSegment_BoxID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_MinkVaegtSegment_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_MinkVaegtSegment_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_MinkVaegtSegment_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkVaegtSegment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVaegtSegment_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkVaegtSegment_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkVaegtSegment_MinkHus] FOREIGN KEY ([HusID]) REFERENCES [dbo].[MinkHus] ([ID])
);

