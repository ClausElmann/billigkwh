CREATE TABLE [dbo].[MinkHvalpeVejning] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [BedriftID]             INT              CONSTRAINT [DF_MinkHvalpeVejning_StaldID] DEFAULT ((0)) NOT NULL,
    [DyreTypeID]            INT              CONSTRAINT [DF_MinkHvalpeVejning_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [HoldNr]                INT              NOT NULL,
    [Aar]                   INT              NOT NULL,
    [Foedt]                 DATE             CONSTRAINT [DF_MinkHvalpeVejning_Dato] DEFAULT (getdate()) NOT NULL,
    [AntalHan28]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHan1] DEFAULT ((0)) NOT NULL,
    [AntalHun28]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHun1] DEFAULT ((0)) NOT NULL,
    [HanVaegt28]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HanVaegt1] DEFAULT ((0)) NOT NULL,
    [HunVaegt28]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HunVaegt1] DEFAULT ((0)) NOT NULL,
    [AntalHan42]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHan2] DEFAULT ((0)) NOT NULL,
    [AntalHun42]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHun2] DEFAULT ((0)) NOT NULL,
    [HanVaegt42]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HanVaegt2] DEFAULT ((0)) NOT NULL,
    [HunVaegt42]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HunVaegt2] DEFAULT ((0)) NOT NULL,
    [AntalHan56]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHan3] DEFAULT ((0)) NOT NULL,
    [AntalHun56]            INT              CONSTRAINT [DF_MinkHvalpeVejning_AntalHun3] DEFAULT ((0)) NOT NULL,
    [HanVaegt56]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HanVaegt3] DEFAULT ((0)) NOT NULL,
    [HunVaegt56]            INT              CONSTRAINT [DF_MinkHvalpeVejning_HunVaegt3] DEFAULT ((0)) NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkHvalpeVejning_Slettet] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkHvalpeVejning_ObjektGuid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_MinkHvalpeVejning] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkHvalpeVejning_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkHvalpeVejning_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkHvalpeVejning_DyreType] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkHvalpeVejning]
    ON [dbo].[MinkHvalpeVejning]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkHvalpeVejning]
    ON [dbo].[MinkHvalpeVejning]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkHvalpeVejning]
    ON [dbo].[MinkHvalpeVejning]([BedriftID] ASC, [Aar] ASC, [HoldNr] ASC, [DyreTypeID] ASC);

