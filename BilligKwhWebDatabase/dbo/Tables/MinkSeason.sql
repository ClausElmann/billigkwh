CREATE TABLE [dbo].[MinkSeason] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [BedriftID]             INT      NOT NULL,
    [SeasonAarID]           INT      CONSTRAINT [DF_MinkSeason_SeasonAarID] DEFAULT ((20171)) NOT NULL,
    [Forstedag]             DATE     CONSTRAINT [DF_MinkSeason_Forstedag] DEFAULT (getdate()) NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_MinkSeason_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_MinkSeason_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Sidstedag]             DATE     CONSTRAINT [DF_MinkSeason_Forstedag1] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MinkSeason] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkSeason_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkSeason_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkSeason_MinkSeasonAar] FOREIGN KEY ([SeasonAarID]) REFERENCES [dbo].[MinkSeasonAar] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkSeason]
    ON [dbo].[MinkSeason]([BedriftID] ASC, [SeasonAarID] ASC);

