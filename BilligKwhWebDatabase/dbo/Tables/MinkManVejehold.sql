CREATE TABLE [dbo].[MinkManVejehold] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  CONSTRAINT [DF_MinkManVejehold_Navn] DEFAULT ('') NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkManVejehold_Beskrivelse] DEFAULT ('') NOT NULL,
    [StartDato]             DATE           NOT NULL,
    [SlutDato]              DATE           NULL,
    [Aar]                   INT            CONSTRAINT [DF_MinkManVejehold_Aar] DEFAULT ((2017)) NOT NULL,
    [VinterHold]            BIT            CONSTRAINT [DF_MinkManVejehold_VinterHold] DEFAULT ((0)) NOT NULL,
    [DyreTypeID]            INT            CONSTRAINT [DF_MinkManVejehold_DyreTypeID] DEFAULT ((0)) NOT NULL,
    [SidsteGnsHanVaegt]     SMALLINT       CONSTRAINT [DF_MinkManVejehold_SidsteHanVaegt] DEFAULT ((0)) NOT NULL,
    [SidsteGnsHunVaegt]     SMALLINT       CONSTRAINT [DF_MinkManVejehold_SidsteHunVaegt] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkManVejehold_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkManVejehold_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkManVejehold_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkManVejehold] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkManVejehold_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkManVejehold_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkManVejehold_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkManVejehold_ListePost] FOREIGN KEY ([DyreTypeID]) REFERENCES [dbo].[ListePost] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [CI_MinkManVejehold]
    ON [dbo].[MinkManVejehold]([BedriftID] ASC, [KundeID] ASC, [ID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MinkManVejehold]
    ON [dbo].[MinkManVejehold]([SidstRettet] ASC, [Slettet] ASC);

