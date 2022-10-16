CREATE TABLE [dbo].[ListePost] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            CONSTRAINT [DF_ListePost_BedriftID] DEFAULT ((0)) NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Kommentar]             NVARCHAR (250) CONSTRAINT [DF_ListePost_Kommentar] DEFAULT ('') NOT NULL,
    [ListeID]               INT            NOT NULL,
    [Placering]             INT            NOT NULL,
    [BilligKwhListePost]     BIT            NOT NULL,
    [IkonUrl]               NVARCHAR (50)  CONSTRAINT [DF_ListePost_IkonUrl] DEFAULT ('') NOT NULL,
    [Label]                 NVARCHAR (6)   CONSTRAINT [DF_ListePost_Label] DEFAULT ('') NOT NULL,
    [Slettet]               BIT            NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_ListePost_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_ListePost_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [KopiAfID]              INT            NULL,
    CONSTRAINT [PK_ListePost] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ListePost] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_ListePost_Liste] FOREIGN KEY ([ListeID]) REFERENCES [dbo].[Liste] ([ID])
);

