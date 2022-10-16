CREATE TABLE [dbo].[Vejehold] (
    [ID]                        INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]                   INT            NOT NULL,
    [FoderVaegtID]              INT            CONSTRAINT [DF_Vejehold_FoderVaegtID1] DEFAULT ((1)) NOT NULL,
    [VejeholdNr]                INT            NOT NULL,
    [Navn]                      NVARCHAR (50)  NOT NULL,
    [Beskrivelse]               NVARCHAR (MAX) CONSTRAINT [DF_Vejehold_Beskrivelse] DEFAULT ('') NOT NULL,
    [StartDato]                 DATETIME       NOT NULL,
    [SlutDato]                  DATE           NULL,
    [FoderMix1VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID] DEFAULT ((0)) NOT NULL,
    [FoderMix2VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID7] DEFAULT ((0)) NOT NULL,
    [FoderMix3VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID6] DEFAULT ((0)) NOT NULL,
    [FoderMix4VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID5] DEFAULT ((0)) NOT NULL,
    [FoderMix5VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID4] DEFAULT ((0)) NOT NULL,
    [FoderMix6VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID3] DEFAULT ((0)) NOT NULL,
    [FoderMix7VersionID]        INT            CONSTRAINT [DF_Vejehold_FoderMix1ID2] DEFAULT ((0)) NOT NULL,
    [SidstRettet]               DATETIME       CONSTRAINT [DF_Vejehold_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]     INT            CONSTRAINT [DF_Vejehold_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]                   BIT            CONSTRAINT [DF_Vejehold_Slettet] DEFAULT ((0)) NOT NULL,
    [StartAlderGrise]           SMALLINT       CONSTRAINT [DF_Vejehold_StartAlderGrise] DEFAULT ((0)) NOT NULL,
    [SlutLbNr]                  SMALLINT       CONSTRAINT [DF_Vejehold_SlutLbNr] DEFAULT ((8759)) NOT NULL,
    [FoderBegraensningPlanID]   INT            CONSTRAINT [DF_Vejehold_FoderBegraensningID] DEFAULT ((0)) NOT NULL,
    [HoldID]                    INT            CONSTRAINT [DF_Vejehold_FoderBegraensningPlanID1] DEFAULT ((0)) NOT NULL,
    [Offline]                   BIT            NULL,
    [DyreGruppeTypeListePostID] INT            CONSTRAINT [DF_Vejehold_DyreGruppeTypeID1] DEFAULT ((0)) NOT NULL,
    [HoldTypeListePostID]       INT            CONSTRAINT [DF_Vejehold_HoldTypeID1] DEFAULT ((0)) NOT NULL,
    [StiID]                     INT            CONSTRAINT [DF_Vejehold_StiID] DEFAULT ((0)) NOT NULL,
    [Afsluttet]                 BIT            CONSTRAINT [DF_Vejehold_Slettet1] DEFAULT ((0)) NOT NULL,
    [SidsteVaegt]               INT            CONSTRAINT [DF_Vejehold_SidsteVaegt] DEFAULT ((0)) NOT NULL,
    [SidsteAntalSogrise]        SMALLINT       CONSTRAINT [DF_Vejehold_SidsteAntalSogrise] DEFAULT ((0)) NOT NULL,
    [SidsteAntalGalte]          SMALLINT       CONSTRAINT [DF_Vejehold_SidsteAntalSogrise1] DEFAULT ((0)) NOT NULL,
    [StartVaegt]                INT            CONSTRAINT [DF_Vejehold_StartVaegt] DEFAULT ((0)) NOT NULL,
    [x_StartAntal]              INT            CONSTRAINT [DF_Vejehold_StartAntal] DEFAULT ((0)) NOT NULL,
    [StartAntalSogrise]         SMALLINT       CONSTRAINT [DF_Vejehold_SidsteAntalSogrise1_1] DEFAULT ((0)) NOT NULL,
    [StartAntalGalte]           SMALLINT       CONSTRAINT [DF_Vejehold_SidsteAntalGalte1] DEFAULT ((0)) NOT NULL,
    [NyModel]                   BIT            CONSTRAINT [DF_Vejehold_NyModel] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Vejehold] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Vejehold_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderBegraensningPlan] FOREIGN KEY ([FoderBegraensningPlanID]) REFERENCES [dbo].[FoderBegraensningPlan] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion1] FOREIGN KEY ([FoderMix1VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion2] FOREIGN KEY ([FoderMix2VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion3] FOREIGN KEY ([FoderMix3VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion4] FOREIGN KEY ([FoderMix4VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion5] FOREIGN KEY ([FoderMix5VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion6] FOREIGN KEY ([FoderMix6VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderMixVersion7] FOREIGN KEY ([FoderMix7VersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_Vejehold_FoderVaegt] FOREIGN KEY ([FoderVaegtID]) REFERENCES [dbo].[FoderVaegt] ([ID]),
    CONSTRAINT [FK_Vejehold_Hold] FOREIGN KEY ([HoldID]) REFERENCES [dbo].[Hold] ([ID]),
    CONSTRAINT [FK_Vejehold_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Vejehold_ListePost] FOREIGN KEY ([DyreGruppeTypeListePostID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Vejehold_ListePost1] FOREIGN KEY ([HoldTypeListePostID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Vejehold_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Vejehold]
    ON [dbo].[Vejehold]([VejeholdNr] ASC, [FoderVaegtID] ASC);


GO
CREATE CLUSTERED INDEX [CX_Vejehold]
    ON [dbo].[Vejehold]([KundeID] ASC, [ID] ASC);

