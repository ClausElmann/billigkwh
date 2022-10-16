CREATE TABLE [dbo].[MinkFoderProduktion] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Dato]                  DATETIME       NOT NULL,
    [DglFoderPrDyr]         INT            CONSTRAINT [DF_MinkFoderProduktion_DglFoderPrDyr] DEFAULT ((0)) NOT NULL,
    [EnergiKcal]            INT            CONSTRAINT [DF_MinkFoderProduktion_EnergiKcal] DEFAULT ((0)) NOT NULL,
    [DgsProduktionTon]      INT            CONSTRAINT [DF_MinkFoderProduktion_NaesteLeveringMaengde] DEFAULT ((-1)) NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX) CONSTRAINT [DF_MinkFoderProduktion_Beskrivelse] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkFoderProduktion_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkFoderProduktion_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkFoderProduktion_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkFoderProduktion] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkFoderProduktion_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkFoderProduktion_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkFoderProduktion]
    ON [dbo].[MinkFoderProduktion]([KundeID] ASC, [Dato] ASC, [ID] ASC);

