CREATE TABLE [dbo].[FoderAnlaegTimeMix] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      NOT NULL,
    [FoderAnlaegID]         INT      NOT NULL,
    [Time]                  SMALLINT NOT NULL,
    [MixNr1]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr1] DEFAULT ((0)) NOT NULL,
    [MixNr2]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr11] DEFAULT ((0)) NOT NULL,
    [MixNr3]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr12] DEFAULT ((0)) NOT NULL,
    [MixNr4]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr13] DEFAULT ((0)) NOT NULL,
    [MixNr5]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr14] DEFAULT ((0)) NOT NULL,
    [MixNr6]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr15] DEFAULT ((0)) NOT NULL,
    [MixNr7]                BIT      CONSTRAINT [DF_FoderAnlaegTime_MixNr16] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_FoderAnlaegTime_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_FoderAnlaegTime_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_FoderAnlaegTime_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderAnlaegTime] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderAnlaegTime_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderAnlaegTime_FoderAnlaeg] FOREIGN KEY ([FoderAnlaegID]) REFERENCES [dbo].[FoderAnlaeg] ([ID]),
    CONSTRAINT [FK_FoderAnlaegTime_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_FoderAnlaegTime]
    ON [dbo].[FoderAnlaegTimeMix]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

