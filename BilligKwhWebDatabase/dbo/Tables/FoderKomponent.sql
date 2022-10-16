CREATE TABLE [dbo].[FoderKomponent] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (100) NOT NULL,
    [Placering]             INT            NOT NULL,
    [Label]                 NVARCHAR (6)   CONSTRAINT [DF_FoderIngrediens_Label] DEFAULT ('') NOT NULL,
    [Producent]             NVARCHAR (50)  CONSTRAINT [DF_FoderIngrediens_Producent] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_FoderIngrediens_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_FoderIngrediens_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_FoderIngrediens_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderIngrediens] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderIngrediens_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderIngrediens_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_FoderIngrediens]
    ON [dbo].[FoderKomponent]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

