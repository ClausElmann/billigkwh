CREATE TABLE [dbo].[BedriftAdgang] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      CONSTRAINT [DF_BedriftAdgang_KundeID] DEFAULT ((0)) NOT NULL,
    [BedriftID]             INT      CONSTRAINT [DF_BedriftAdgang_BedriftID] DEFAULT ((0)) NOT NULL,
    [GaesteKundeID]         INT      CONSTRAINT [DF_BedriftAdgang_GaesteKundeID] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME NOT NULL,
    [SidstRettet]           DATETIME NOT NULL,
    [SidstRettetAfBrugerID] INT      NOT NULL,
    [Slettet]               BIT      NOT NULL,
    [MGnsKurver]            BIT      CONSTRAINT [DF_BedriftAdgang_MGnsKurver] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BedriftAdgang] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BedriftAdgang_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_BedriftAdgang_GaesteKunde] FOREIGN KEY ([GaesteKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_BedriftAdgang_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_BedriftAdgang]
    ON [dbo].[BedriftAdgang]([KundeID] ASC, [GaesteKundeID] ASC, [BedriftID] ASC);

