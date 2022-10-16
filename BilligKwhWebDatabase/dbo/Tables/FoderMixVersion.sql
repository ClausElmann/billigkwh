CREATE TABLE [dbo].[FoderMixVersion] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT             NOT NULL,
    [FoderMixID]            INT             NOT NULL,
    [Version]               SMALLINT        NOT NULL,
    [Tørstof]               DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Tørstof] DEFAULT ((100)) NOT NULL,
    [Pris]                  DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Pris] DEFAULT ((0.0)) NOT NULL,
    [Energi]                DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Energi] DEFAULT ((1000)) NOT NULL,
    [Lysin]                 DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Lysin] DEFAULT ((0)) NOT NULL,
    [Råprotein]             DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Råprotein] DEFAULT ((50)) NOT NULL,
    [FoderEnhedPrKg]        DECIMAL (12, 2) CONSTRAINT [DF_FoderMixVersion_Råprotein1] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME        CONSTRAINT [DF_FoderMixVersion_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT             CONSTRAINT [DF_FoderMixVersion_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT             CONSTRAINT [DF_FoderMixVersion_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderMixVersion] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderMixVersion_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderMixVersion_FoderMix] FOREIGN KEY ([FoderMixID]) REFERENCES [dbo].[FoderMix] ([ID]),
    CONSTRAINT [FK_FoderMixVersion_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_FoderMixVersion]
    ON [dbo].[FoderMixVersion]([KundeID] ASC, [FoderMixID] ASC, [Version] ASC);

