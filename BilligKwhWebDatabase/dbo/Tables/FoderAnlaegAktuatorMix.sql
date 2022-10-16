CREATE TABLE [dbo].[FoderAnlaegAktuatorMix] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      NOT NULL,
    [FoderAnlaegID]         INT      NOT NULL,
    [MixNr1Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr1] DEFAULT ((1)) NOT NULL,
    [MixNr2Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr11] DEFAULT ((2)) NOT NULL,
    [MixNr3Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr12] DEFAULT ((3)) NOT NULL,
    [MixNr4Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr13] DEFAULT ((12)) NOT NULL,
    [MixNr5Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr14] DEFAULT ((23)) NOT NULL,
    [MixNr6Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr15] DEFAULT ((13)) NOT NULL,
    [MixNr7Aktuator]        SMALLINT CONSTRAINT [DF_FoderAnlaegAktuatorMix_MixNr16] DEFAULT ((123)) NOT NULL,
    [SidstRettet]           DATETIME CONSTRAINT [DF_FoderAnlaegAktuatorMix_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT      CONSTRAINT [DF_FoderAnlaegAktuatorMix_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT      CONSTRAINT [DF_FoderAnlaegAktuatorMix_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderAnlaegAktuatorMix] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderAnlaegAktuatorMix_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderAnlaegAktuatorMix_FoderAnlaeg] FOREIGN KEY ([FoderAnlaegID]) REFERENCES [dbo].[FoderAnlaeg] ([ID]),
    CONSTRAINT [FK_FoderAnlaegAktuatorMix_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_FoderAnlaegAktuatorMix]
    ON [dbo].[FoderAnlaegAktuatorMix]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

