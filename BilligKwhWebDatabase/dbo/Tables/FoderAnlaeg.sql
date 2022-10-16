CREATE TABLE [dbo].[FoderAnlaeg] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [UniqueID]              NVARCHAR (50)  CONSTRAINT [DF_FoderAnlaeg_UniqueID] DEFAULT ('') NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_FoderAnlaeg_Beskrivelse] DEFAULT ('') NOT NULL,
    [BedriftID]             INT            NOT NULL,
    [MixNr1Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr1Aktuator] DEFAULT ((1)) NOT NULL,
    [MixNr2Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr2Aktuator] DEFAULT ((2)) NOT NULL,
    [MixNr3Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr3Aktuator] DEFAULT ((3)) NOT NULL,
    [MixNr4Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr4Aktuator] DEFAULT ((12)) NOT NULL,
    [MixNr5Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr5Aktuator] DEFAULT ((23)) NOT NULL,
    [MixNr6Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr6Aktuator] DEFAULT ((13)) NOT NULL,
    [MixNr7Aktuator]        SMALLINT       CONSTRAINT [DF_FoderAnlaeg_MixNr7Aktuator] DEFAULT ((123)) NOT NULL,
    [OpsaetningData]        NVARCHAR (MAX) CONSTRAINT [DF_FoderAnlaeg_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]    DATETIME       CONSTRAINT [DF_FoderAnlaeg_OpsaetningModtaget] DEFAULT (getdate()) NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_FoderAnlaeg_OrdreNr] DEFAULT ((0)) NULL,
    [Mix1VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix1VersionID] DEFAULT ((0)) NOT NULL,
    [Mix2VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix2VersionID] DEFAULT ((0)) NOT NULL,
    [Mix3VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix3VersionID] DEFAULT ((0)) NOT NULL,
    [Mix4VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix4VersionID] DEFAULT ((0)) NOT NULL,
    [Mix5VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix5VersionID] DEFAULT ((0)) NOT NULL,
    [Mix6VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix6VersionID] DEFAULT ((0)) NOT NULL,
    [Mix7VersionID]         INT            CONSTRAINT [DF_FoderAnlaeg_Mix7VersionID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_FoderAnlaeg_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_FoderAnlaeg_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_FoderAnlaeg_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderAnlaeg] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderAnlaeg_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion1] FOREIGN KEY ([Mix1VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion2] FOREIGN KEY ([Mix2VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion3] FOREIGN KEY ([Mix3VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion4] FOREIGN KEY ([Mix4VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion5] FOREIGN KEY ([Mix5VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion6] FOREIGN KEY ([Mix6VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_FoderMixVersion7] FOREIGN KEY ([Mix7VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_FoderAnlaeg_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_UniqueID_notnull]
    ON [dbo].[FoderAnlaeg]([UniqueID] ASC) WHERE ([UniqueID] IS NOT NULL);


GO
CREATE CLUSTERED INDEX [CI_FoderAnlaeg]
    ON [dbo].[FoderAnlaeg]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

