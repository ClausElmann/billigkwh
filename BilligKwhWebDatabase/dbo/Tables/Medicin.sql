CREATE TABLE [dbo].[Medicin] (
    [ID]                          INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]                     INT            NOT NULL,
    [Navn]                        NVARCHAR (100) NOT NULL,
    [Placering]                   INT            NOT NULL,
    [Beskrivelse]                 NVARCHAR (MAX) CONSTRAINT [DF_MedicinKomponent_Beskrivelse] DEFAULT ('') NOT NULL,
    [Anvisning]                   NVARCHAR (100) CONSTRAINT [DF_Medicin_DosisTxt] DEFAULT ('') NOT NULL,
    [Diagnose]                    NVARCHAR (50)  CONSTRAINT [DF_Medicin_DosisTxt1] DEFAULT ('') NOT NULL,
    [Tilbageholdelse]             SMALLINT       CONSTRAINT [DF_Medicin_DosisTxt3] DEFAULT ((0)) NOT NULL,
    [Producent]                   NVARCHAR (50)  CONSTRAINT [DF_FoderKomponent_Producent] DEFAULT ('') NOT NULL,
    [Pris]                        DECIMAL (6, 2) CONSTRAINT [DF_Medicin_Pris] DEFAULT ((0.0)) NOT NULL,
    [MedicinDoseringTypeID]       SMALLINT       CONSTRAINT [DF_Medicin_MedicinDoseringTypeID] DEFAULT ((1)) NOT NULL,
    [DosePrEnhed]                 DECIMAL (6, 2) CONSTRAINT [DF_Medicin_DosePrEnhed] DEFAULT ((1)) NOT NULL,
    [PrEnhedMaengde]              SMALLINT       CONSTRAINT [DF_Medicin_PrEnhedMaengde] DEFAULT ((1)) NOT NULL,
    [DageIMellem]                 SMALLINT       CONSTRAINT [DF_Medicin_TimerIMellem1] DEFAULT ((1)) NOT NULL,
    [AntalBehandlinger]           SMALLINT       CONSTRAINT [DF_Medicin_AntalBehandlinger] DEFAULT ((1)) NOT NULL,
    [SidstRettet]                 DATETIME       CONSTRAINT [DF_FoderKomponent_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]       INT            CONSTRAINT [DF_FoderKomponent_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]                     BIT            CONSTRAINT [DF_FoderKomponent_Slettet] DEFAULT ((0)) NOT NULL,
    [MedicinTypeListePostID]      INT            CONSTRAINT [DF_Medicin_MedicinTypeID1_1] DEFAULT ((25741)) NOT NULL,
    [MedicineringTypeListePostID] INT            CONSTRAINT [DF_Medicin_MedicineringTypeID1] DEFAULT ((25743)) NOT NULL,
    [MedicinEnhedListePostID]     INT            CONSTRAINT [DF_Medicin_MedicinTypeListePostID1] DEFAULT ((25741)) NOT NULL,
    [Slagtesvin]                  BIT            CONSTRAINT [DF_Medicin_Slagtesvin] DEFAULT ((0)) NOT NULL,
    [Soer]                        BIT            CONSTRAINT [DF_Medicin_Slagtesvin1] DEFAULT ((0)) NOT NULL,
    [Fravaenningsgrise]           BIT            CONSTRAINT [DF_Medicin_Slagtesvin2] DEFAULT ((0)) NOT NULL,
    [Pattegrise]                  BIT            CONSTRAINT [DF_Medicin_Slagtesvin3] DEFAULT ((0)) NOT NULL,
    [Polte]                       BIT            CONSTRAINT [DF_Medicin_Slagtesvin4] DEFAULT ((0)) NOT NULL,
    [Orner]                       BIT            CONSTRAINT [DF_Medicin_Slagtesvin5] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Medicin] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderKomponent_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderKomponent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Medicin_ListePost] FOREIGN KEY ([MedicinTypeListePostID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Medicin_ListePost1] FOREIGN KEY ([MedicineringTypeListePostID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Medicin_ListePost2] FOREIGN KEY ([MedicinEnhedListePostID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Medicin_MedicinDoseringType] FOREIGN KEY ([MedicinDoseringTypeID]) REFERENCES [dbo].[MedicinDoseringType] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_Medicin]
    ON [dbo].[Medicin]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

