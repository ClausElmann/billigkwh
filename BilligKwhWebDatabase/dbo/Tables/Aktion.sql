CREATE TABLE [dbo].[Aktion] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [AktionGruppeID]        INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [StandardTekst]         NVARCHAR (MAX) NULL,
    [Placering]             INT            CONSTRAINT [DF_AndreOpg_Placering] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       NOT NULL,
    [SidstRettetAfBrugerID] INT            NOT NULL,
    [Slettet]               BIT            NOT NULL,
    CONSTRAINT [PK_Aktion] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Aktion_AktionGruppe] FOREIGN KEY ([AktionGruppeID]) REFERENCES [dbo].[AktionGruppe] ([ID]),
    CONSTRAINT [FK_Aktion_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Aktion_SidstRettetAfBruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);

