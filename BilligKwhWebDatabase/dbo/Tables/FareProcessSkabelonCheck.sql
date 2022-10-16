CREATE TABLE [dbo].[FareProcessSkabelonCheck] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [SkabelonID]            INT            NOT NULL,
    [LbNr]                  SMALLINT       NOT NULL,
    [CheckTekst]            NVARCHAR (500) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_ProcessSkabelonCheck_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_ProcessSkabelonCheck_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_ProcessSkabelonCheck_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcessSkabelonCheck] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcessSkabelonCheck_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_ProcessSkabelonCheck_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_ProcessSkabelonCheck_ProcessSkabelon] FOREIGN KEY ([SkabelonID]) REFERENCES [dbo].[FareProcessSkabelon] ([ID])
);

