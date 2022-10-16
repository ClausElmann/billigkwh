CREATE TABLE [dbo].[FoderBegraensningPlan] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  CONSTRAINT [DF_FoderBegraensningPlan_Navn] DEFAULT ('') NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_FoderBegraensningPlan_Beskrivelse] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_FoderBegraensningPlan_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_FoderBegraensningPlan_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_FoderBegraensningPlan_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderBegraensningPlan] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderBegraensningPlan_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderBegraensningPlan_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

