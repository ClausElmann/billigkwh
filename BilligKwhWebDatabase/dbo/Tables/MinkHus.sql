CREATE TABLE [dbo].[MinkHus] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            NOT NULL,
    [HusNr]                 SMALLINT       CONSTRAINT [DF_MinkHus_RaekkeNr] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkHus_Beskrivelse] DEFAULT ('') NOT NULL,
    [Opskrift]              NVARCHAR (150) CONSTRAINT [DF_MinkHus_Opskrift] DEFAULT ('') NOT NULL,
    [AntalRaekker]          SMALLINT       CONSTRAINT [DF_MinkHus_AntalStier] DEFAULT ((0)) NULL,
    [BurePrRaekke]          SMALLINT       CONSTRAINT [DF_MinkHus_AntalRaekker1] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkHus_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkHus_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkHus_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkHus] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkHus_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkHus_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkHus_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MinkHus]
    ON [dbo].[MinkHus]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

