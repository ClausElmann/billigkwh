CREATE TABLE [dbo].[MinkFoderAnbefaling] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]                INT      NOT NULL,
    [BedriftMinkTypeID]      INT      NOT NULL,
    [Dato]                   DATE     NOT NULL,
    [FoderStyrkeKcal]        INT      NOT NULL,
    [Temperatur]             INT      NOT NULL,
    [VedligeholdKcalHun]     INT      NOT NULL,
    [VaegtaendringDagligHun] INT      NOT NULL,
    [FoderbehovKcalHun]      INT      NOT NULL,
    [FoderbehovGramHun]      INT      NOT NULL,
    [VedligeholdKcalHan]     INT      NOT NULL,
    [VaegtaendringDagligHan] INT      NOT NULL,
    [FoderbehovKcalHan]      INT      NOT NULL,
    [FoderbehovGramHan]      INT      NOT NULL,
    [SidstRettet]            DATETIME CONSTRAINT [DF_MinkFoderAnbefaling_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MinkFoderAnbefaling] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkFoderAnbefaling_BedriftMinkType] FOREIGN KEY ([BedriftMinkTypeID]) REFERENCES [dbo].[BedriftMinkType] ([ID]),
    CONSTRAINT [FK_MinkFoderAnbefaling_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MinkFoderAnbefaling]
    ON [dbo].[MinkFoderAnbefaling]([Dato] ASC, [BedriftMinkTypeID] ASC, [ID] ASC);

