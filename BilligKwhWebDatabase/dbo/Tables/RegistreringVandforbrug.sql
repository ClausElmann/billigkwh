CREATE TABLE [dbo].[RegistreringVandforbrug] (
    [ID]                    INT        NOT NULL,
    [KundeID]               INT        NOT NULL,
    [VejeholdID]            INT        NOT NULL,
    [Dato]                  DATETIME   NOT NULL,
    [Maengde]               FLOAT (53) NOT NULL,
    [SidstRettet]           DATETIME   CONSTRAINT [DF_RegistreringVandforbrug_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT        CONSTRAINT [DF_RegistreringVandforbrug_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT        CONSTRAINT [DF_RegistreringVandforbrug_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VandRegistrering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_RegistreringVandforbrug_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_RegistreringVandforbrug_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_VandRegistrering_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_RegistreringVandforbrug]
    ON [dbo].[RegistreringVandforbrug]([KundeID] ASC, [VejeholdID] ASC, [Slettet] ASC, [ID] ASC);

