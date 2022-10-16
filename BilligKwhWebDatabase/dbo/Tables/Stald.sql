CREATE TABLE [dbo].[Stald] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  CONSTRAINT [DF_Stald_StaldSektion] DEFAULT ('') NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_Stald_Beskrivelse] DEFAULT ('') NOT NULL,
    [Lat]                   FLOAT (53)     CONSTRAINT [DF_Stald_Lat] DEFAULT ((0)) NOT NULL,
    [Lon]                   FLOAT (53)     CONSTRAINT [DF_Stald_Lon] DEFAULT ((0)) NOT NULL,
    [Mix1VersionID]         INT            CONSTRAINT [DF_Stald_Mix1VersionID] DEFAULT ((0)) NOT NULL,
    [Mix2VersionID]         INT            CONSTRAINT [DF_Stald_Mix2VersionID] DEFAULT ((0)) NOT NULL,
    [Mix3VersionID]         INT            CONSTRAINT [DF_Stald_Mix3VersionID] DEFAULT ((0)) NOT NULL,
    [Mix4VersionID]         INT            CONSTRAINT [DF_Stald_Mix4VersionID] DEFAULT ((0)) NOT NULL,
    [Mix5VersionID]         INT            CONSTRAINT [DF_Stald_Mix5VersionID] DEFAULT ((0)) NOT NULL,
    [Mix6VersionID]         INT            CONSTRAINT [DF_Stald_Mix6VersionID] DEFAULT ((0)) NOT NULL,
    [Mix7VersionID]         INT            CONSTRAINT [DF_Stald_Mix7VersionID] DEFAULT ((0)) NOT NULL,
    [AntalSektioner]        SMALLINT       CONSTRAINT [DF_Stald_AntalStier] DEFAULT ((0)) NOT NULL,
    [StierPrSektion]        SMALLINT       CONSTRAINT [DF_Stald_AntalSektioner1] DEFAULT ((0)) NOT NULL,
    [StaldTypeListePostID]  INT            CONSTRAINT [DF_Stald_StaldTypeID1] DEFAULT ((0)) NOT NULL,
    [Slagtesvin]            BIT            CONSTRAINT [DF_Stald_Slagtesvin] DEFAULT ((0)) NOT NULL,
    [Soer]                  BIT            CONSTRAINT [DF_Stald_Soer] DEFAULT ((0)) NOT NULL,
    [Pattegrise]            BIT            CONSTRAINT [DF_Stald_Pattegrise] DEFAULT ((0)) NOT NULL,
    [Fravaenningsgrise]     BIT            CONSTRAINT [DF_Stald_Fravaenningsgrise] DEFAULT ((0)) NOT NULL,
    [Polte]                 BIT            CONSTRAINT [DF_Stald_Polte] DEFAULT ((0)) NOT NULL,
    [Orner]                 BIT            CONSTRAINT [DF_Stald_Orner] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Stald_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Stald_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Stald_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Stald] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Stald_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_Stald_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix1Version] FOREIGN KEY ([Mix1VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix2Version] FOREIGN KEY ([Mix2VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix3Version] FOREIGN KEY ([Mix3VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix4Version] FOREIGN KEY ([Mix4VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix5Version] FOREIGN KEY ([Mix5VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix6Version] FOREIGN KEY ([Mix6VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_FoderMix7Version] FOREIGN KEY ([Mix7VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Stald_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Stald_ListePost] FOREIGN KEY ([StaldTypeListePostID]) REFERENCES [dbo].[ListePost] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Stald]
    ON [dbo].[Stald]([KundeID] ASC, [BedriftID] ASC, [Slettet] ASC, [ID] ASC);

