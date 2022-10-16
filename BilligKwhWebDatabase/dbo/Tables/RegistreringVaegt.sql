CREATE TABLE [dbo].[RegistreringVaegt] (
    [ID]                    INT        NOT NULL,
    [KundeID]               INT        NOT NULL,
    [VejeholdID]            INT        NOT NULL,
    [Dato]                  DATETIME   NOT NULL,
    [Maengde]               FLOAT (53) NOT NULL,
    [SidstRettet]           DATETIME   CONSTRAINT [DF_RegistreringVaegt_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT        CONSTRAINT [DF_RegistreringVaegt_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT        CONSTRAINT [DF_RegistreringVaegt_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VejeRegistrering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_RegistreringVaegt_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_RegistreringVaegt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_VejeRegistrering_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_RegistreringVaegt]
    ON [dbo].[RegistreringVaegt]([KundeID] ASC, [VejeholdID] ASC, [Slettet] ASC, [ID] ASC);

