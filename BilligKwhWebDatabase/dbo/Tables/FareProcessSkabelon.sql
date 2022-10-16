CREATE TABLE [dbo].[FareProcessSkabelon] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_ProcessSkabelon_Beskrivelse] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_ProcessSkabelon_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_ProcessSkabelon_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_ProcessSkabelon_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcessSkabelon] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcessSkabelon_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_ProcessSkabelon_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

