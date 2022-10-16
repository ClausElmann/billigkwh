CREATE TABLE [dbo].[Gris] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT           NOT NULL,
    [Tag]                   NVARCHAR (50) NOT NULL,
    [SidstRettet]           DATETIME      CONSTRAINT [DF_Gris_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT           CONSTRAINT [DF_Gris_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT           CONSTRAINT [DF_Gris_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Gris] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Gris_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Gris_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Gris]
    ON [dbo].[Gris]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

