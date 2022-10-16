CREATE TABLE [dbo].[MinkVaegthold] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [VejeholdID]            INT      CONSTRAINT [DF_MinkVaegthold_VejeholdID] DEFAULT ((0)) NOT NULL,
    [CelleNr]               SMALLINT NOT NULL,
    [BurNr]                 INT      CONSTRAINT [DF_MinkVaegthold_BurNr] DEFAULT ((0)) NOT NULL,
    [SidsteHanVaegt]        SMALLINT CONSTRAINT [DF_Table_1_SidsteVaegt] DEFAULT ((0)) NOT NULL,
    [SidsteHunVaegt]        SMALLINT CONSTRAINT [DF_Table_1_SidsteAntalSogrise] DEFAULT ((0)) NOT NULL,
    [SidsteHanVaegtDato]    DATE     NULL,
    [SlankeStartVaegt]      SMALLINT CONSTRAINT [DF_MinkVaegthold_SidsteHanVaegt1] DEFAULT ((0)) NOT NULL,
    [SidsteHunVaegtDato]    DATE     NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_MinkVejehold_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_MinkVejehold_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_MinkVejehold_Slettet] DEFAULT ((0)) NOT NULL,
    [StartAlder]            SMALLINT CONSTRAINT [DF_Table_1_StartAlderGrise] DEFAULT ((0)) NULL,
    [StartDato]             DATE     NULL,
    [SlutDato]              DATE     NULL,
    [Aar]                   INT      CONSTRAINT [DF_MinkVaegthold_Aar] DEFAULT ((2017)) NULL,
    [VinterHold]            BIT      CONSTRAINT [DF_MinkVejehold_VinterHold] DEFAULT ((0)) NULL,
    [DyreTypeID]            INT      CONSTRAINT [DF_MinkVejehold_HanDyreTypeID1_1] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MinkVejehold] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVaegthold_MinkVejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[MinkVejehold] ([ID]),
    CONSTRAINT [FK_MinkVejehold_DyreTypeID] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MinkVejehold_SidstRettetAfBruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MinkVejehold_Rettet_Slettet]
    ON [dbo].[MinkVaegthold]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkVaegthold]
    ON [dbo].[MinkVaegthold]([VejeholdID] ASC, [CelleNr] ASC);

