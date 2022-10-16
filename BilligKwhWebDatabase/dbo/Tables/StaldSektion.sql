CREATE TABLE [dbo].[StaldSektion] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [StaldID]               INT            NOT NULL,
    [SektionNr]             SMALLINT       CONSTRAINT [DF_StaldSektion_Nr] DEFAULT ((0)) NOT NULL,
    [AntalStier]            SMALLINT       CONSTRAINT [DF_StaldSektion_AntalStier] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_StaldSektion_Beskrivelse] DEFAULT ('') NOT NULL,
    [Lat]                   FLOAT (53)     CONSTRAINT [DF_StaldSektion_Lat] DEFAULT ((0)) NOT NULL,
    [Lon]                   FLOAT (53)     CONSTRAINT [DF_StaldSektion_Lon] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_StaldSektion_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_StaldSektion_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_StaldSektion_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StaldSektion] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StaldSektion_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_StaldSektion_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_StaldSektion_Stald] FOREIGN KEY ([StaldID]) REFERENCES [dbo].[Stald] ([ID])
);

