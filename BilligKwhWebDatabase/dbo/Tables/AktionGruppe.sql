CREATE TABLE [dbo].[AktionGruppe] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT           NOT NULL,
    [Navn]                  NVARCHAR (50) NOT NULL,
    [Placering]             INT           CONSTRAINT [DF_AndreOpgGruppe_Placering] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME      NOT NULL,
    [SidstRettetAfBrugerID] INT           NOT NULL,
    [Slettet]               BIT           NOT NULL,
    CONSTRAINT [PK_AktionGruppe] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_AktionGruppe_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_AktionGruppe_SidstRettetAfBruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);

