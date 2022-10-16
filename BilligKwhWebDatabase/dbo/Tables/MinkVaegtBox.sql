CREATE TABLE [dbo].[MinkVaegtBox] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            CONSTRAINT [DF_MinkVaegtBox_BedriftID] DEFAULT ((0)) NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkVaegtBox_Beskrivelse] DEFAULT ('') NOT NULL,
    [GensendFraID]          INT            CONSTRAINT [DF_MinkVaegtBox_HentFraID] DEFAULT ((0)) NOT NULL,
    [OpsaetningData]        NVARCHAR (MAX) CONSTRAINT [DF_MinkVaegtBox_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]    DATETIME       CONSTRAINT [DF_MinkVaegtBox_SidstRettet1] DEFAULT (getdate()) NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkVaegtBox_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_MinkVaegtBox_OrdreNr] DEFAULT ((0)) NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkVaegtBox_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkVaegtBox_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkVaegtBox] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVaegtBox_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkVaegtBox_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkVaegtBox_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

