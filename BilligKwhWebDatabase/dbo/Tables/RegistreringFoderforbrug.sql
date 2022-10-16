CREATE TABLE [dbo].[RegistreringFoderforbrug] (
    [ID]                    INT        NOT NULL,
    [KundeID]               INT        NOT NULL,
    [VejeholdID]            INT        NOT NULL,
    [Dato]                  DATETIME   NOT NULL,
    [Maengde]               FLOAT (53) NOT NULL,
    [SidstRettet]           DATETIME   CONSTRAINT [DF_RegistreringFoderforbrug_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT        CONSTRAINT [DF_RegistreringFoderforbrug_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT        CONSTRAINT [DF_RegistreringFoderforbrug_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderRegistrering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderRegistrering_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID]),
    CONSTRAINT [FK_RegistreringFoderforbrug_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_RegistreringFoderforbrug_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_RegistreringFoderforbrug]
    ON [dbo].[RegistreringFoderforbrug]([KundeID] ASC, [VejeholdID] ASC, [Slettet] ASC, [ID] ASC);

