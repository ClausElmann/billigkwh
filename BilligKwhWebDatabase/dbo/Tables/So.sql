CREATE TABLE [dbo].[So] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      NOT NULL,
    [BedriftID]             INT      CONSTRAINT [DF_So_BedriftID] DEFAULT ((0)) NOT NULL,
    [Nr]                    SMALLINT CONSTRAINT [DF_So_SoNr] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_So_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_So_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_So_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_So] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_So_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_So_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_So_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_So]
    ON [dbo].[So]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

