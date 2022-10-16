CREATE TABLE [dbo].[BedriftMinkType] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [BedriftID]              INT      NOT NULL,
    [ListePostID]            INT      CONSTRAINT [DF_BedriftMinkType_ListePostID] DEFAULT ((0)) NOT NULL,
    [VedligeholdKcalHun]     INT      CONSTRAINT [DF_BedriftMinkType_VedligeholdKcalHun] DEFAULT ((0)) NOT NULL,
    [VaegtaendringDagligHun] INT      CONSTRAINT [DF_BedriftMinkType_VaegtaendringDagligHun] DEFAULT ((0)) NOT NULL,
    [VedligeholdKcalHan]     INT      CONSTRAINT [DF_BedriftMinkType_VedligeholdKcalHan] DEFAULT ((0)) NOT NULL,
    [VaegtaendringDagligHan] INT      CONSTRAINT [DF_BedriftMinkType_VaegtaendringDagligHan] DEFAULT ((0)) NOT NULL,
    [SidstRettet]            DATETIME NOT NULL,
    [SidstRettetAfBrugerID]  INT      NOT NULL,
    [Slettet]                BIT      NOT NULL,
    CONSTRAINT [PK_BedriftMinkType] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BedriftMinkType_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_BedriftMinkType_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_BedriftMinkType_ListePost] FOREIGN KEY ([ListePostID]) REFERENCES [dbo].[ListePost] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_BedriftMinkType]
    ON [dbo].[BedriftMinkType]([BedriftID] ASC, [ListePostID] ASC);

