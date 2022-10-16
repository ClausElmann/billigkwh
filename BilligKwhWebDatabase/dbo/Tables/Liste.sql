CREATE TABLE [dbo].[Liste] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]       INT           CONSTRAINT [DF_Liste_KundeID] DEFAULT ((0)) NOT NULL,
    [Navn]          NVARCHAR (50) CONSTRAINT [DF_Liste_Navn] DEFAULT ('') NOT NULL,
    [Kommentar]     NVARCHAR (50) CONSTRAINT [DF_Liste_Kommentar] DEFAULT ('') NOT NULL,
    [BilligKwhListe] BIT           CONSTRAINT [DF_Liste_BilligKwhListe] DEFAULT ((0)) NOT NULL,
    [AdminListe]    BIT           CONSTRAINT [DF_Liste_AdminListe] DEFAULT ((0)) NOT NULL,
    [Label]         NVARCHAR (6)  CONSTRAINT [DF_Liste_Label] DEFAULT ('') NOT NULL,
    [Slettet]       BIT           CONSTRAINT [DF_Liste_Slettet] DEFAULT ((0)) NOT NULL,
    [Gris]          BIT           CONSTRAINT [DF_Liste_Gris] DEFAULT ((0)) NOT NULL,
    [Mink]          BIT           CONSTRAINT [DF_Liste_Mink] DEFAULT ((0)) NOT NULL,
    [PrBedrift]     BIT           CONSTRAINT [DF_Liste_PrBedrift] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Liste] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Liste] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

