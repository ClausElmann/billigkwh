CREATE TABLE [dbo].[FoderBegraensning] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [FoderBegraensningPlanID] INT      CONSTRAINT [DF_FoderBegraensning_FoderBegraensningPlanID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]             DATETIME CONSTRAINT [DF_FoderBegraensning_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]   INT      CONSTRAINT [DF_FoderBegraensning_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]                 BIT      CONSTRAINT [DF_FoderBegraensning_Slettet] DEFAULT ((0)) NOT NULL,
    [VaegtDyr]                SMALLINT CONSTRAINT [DF_FoderBegraensning_FraVaegt] DEFAULT ((0)) NOT NULL,
    [Begraensning]            SMALLINT CONSTRAINT [DF_FoderBegraensning_Begraensning] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderBegraensning] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderBegraensning_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderBegraensning_FoderBegraensningPlan] FOREIGN KEY ([FoderBegraensningPlanID]) REFERENCES [dbo].[FoderBegraensningPlan] ([ID]),
    CONSTRAINT [FK_FoderBegraensning_Kunde] FOREIGN KEY ([FoderBegraensningPlanID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_FoderBegraensning]
    ON [dbo].[FoderBegraensning]([FoderBegraensningPlanID] ASC, [Slettet] ASC, [ID] ASC);

