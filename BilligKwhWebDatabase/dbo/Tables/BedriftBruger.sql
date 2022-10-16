CREATE TABLE [dbo].[BedriftBruger] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [BedriftID]             INT      NOT NULL,
    [BrugerID]              INT      NOT NULL,
    [Oprettet]              DATETIME NOT NULL,
    [SidstRettet]           DATETIME NOT NULL,
    [SidstRettetAfBrugerID] INT      NOT NULL,
    [Slettet]               BIT      NOT NULL,
    CONSTRAINT [PK_BedriftBruger] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BedriftBruger_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_BedriftBruger_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_BedriftBruger]
    ON [dbo].[BedriftBruger]([BedriftID] ASC, [BrugerID] ASC, [ID] ASC);

