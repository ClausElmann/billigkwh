CREATE TABLE [dbo].[ElTavle] (
    [ID]                          INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]                     INT              NOT NULL,
    [BrugerDeviceID]              INT              NULL,
    [TavlefabrikatID]             INT              CONSTRAINT [DF_ElTavle_TavlefabrikatID] DEFAULT ((1)) NOT NULL,
    [Adresse]                     NVARCHAR (MAX)   CONSTRAINT [DF_Eltavle_Adresse] DEFAULT ('') NOT NULL,
    [Rekvisition]                 NVARCHAR (50)    CONSTRAINT [DF_Eltavle_Rekvisition] DEFAULT ('') NOT NULL,
    [OrdreNr]                     INT              NULL,
    [TavleNr]                     INT              NULL,
    [OprettetAfBrugerID]          INT              NOT NULL,
    [BeregnetDato]                DATETIME         NOT NULL,
    [BestiltDato]                 DATETIME         CONSTRAINT [DF_ElTavle_BestiltDato] DEFAULT (getdate()) NULL,
    [KomponenterPris]             INT              CONSTRAINT [DF_ElTavle_InstallatoerKomponenter] DEFAULT ((0)) NOT NULL,
    [KomponenterInstallatoer]     INT              CONSTRAINT [DF_ElTavle_KomponenterInstallatoer] DEFAULT ((0)) NOT NULL,
    [TimeAntal]                   INT              CONSTRAINT [DF_ElTavle_InstallatoerTimer] DEFAULT ((0)) NOT NULL,
    [TimePris]                    INT              CONSTRAINT [DF_ElTavle_InstallatoerTimePris] DEFAULT ((0)) NOT NULL,
    [DBFaktor]                    DECIMAL (4, 2)   CONSTRAINT [DF_ElTavle_InstallatoerDB] DEFAULT ((0)) NOT NULL,
    [Fragt]                       INT              CONSTRAINT [DF_ElTavle_InstallatoerFragt] DEFAULT ((0)) NOT NULL,
    [PrisInclTimerOgDB]           INT              CONSTRAINT [DF_ElTavle_InstallatoerKomponenterPris] DEFAULT ((0)) NOT NULL,
    [Kommentar]                   NVARCHAR (MAX)   CONSTRAINT [DF_ElTavle_Kommentar] DEFAULT ('') NOT NULL,
    [Slettet]                     BIT              CONSTRAINT [DF_ElTavle_Slettet] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]                  UNIQUEIDENTIFIER CONSTRAINT [DF_Eltavle_TavleGuid] DEFAULT (newid()) NOT NULL,
    [OptjentBonus]                INT              CONSTRAINT [DF_ElTavle_OptjentBonus] DEFAULT ((0)) NULL,
    [UdbetaltBonus]               INT              CONSTRAINT [DF_ElTavle_UdbetaltBonus] DEFAULT ((0)) NULL,
    [SidstRettet]                 DATETIME         CONSTRAINT [DF_ElTavle_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Moduler]                     INT              CONSTRAINT [DF_ElTavle_Moduler] DEFAULT ((0)) NOT NULL,
    [KabinetModuler]              INT              CONSTRAINT [DF_ElTavle_KabinetModuler] DEFAULT ((0)) NOT NULL,
    [Antal]                       INT              CONSTRAINT [DF_ElTavle_Antal] DEFAULT ((1)) NOT NULL,
    [EconomicId]                  BIGINT           NULL,
    [NettoPris]                   INT              CONSTRAINT [DF_ElTavle_NettoPris] DEFAULT ((0)) NOT NULL,
    [InitialRabat]                BIT              CONSTRAINT [DF_ElTavle_InitialRabat] DEFAULT ((0)) NOT NULL,
    [BonusGivende]                BIT              CONSTRAINT [DF_ElTavle_BonusGivende] DEFAULT ((0)) NOT NULL,
    [TypeID]                      INT              CONSTRAINT [DF_ElTavle_TypeID] DEFAULT ((2)) NOT NULL,
    [Aar]                         INT              NULL,
    [LoebeNr]                     INT              NULL,
    [EconomicOrderNumber]         INT              NULL,
    [EconomicDraftInvoiceNumber]  INT              NULL,
    [EconomicBookedInvoiceNumber] INT              NULL,
    [EconomicSidstRettet]         DATETIME         NULL,
    [MaerkeStroem]                NVARCHAR (50)    NULL,
    [KapslingsKlasse]             NVARCHAR (50)    NULL,
    [DriftsSpaending]             NVARCHAR (50)    NULL,
    [MaxKortslutningsStroem]      NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Eltavle] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Eltavle_Bruger] FOREIGN KEY ([OprettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_ElTavle_BrugerDevice] FOREIGN KEY ([BrugerDeviceID]) REFERENCES [dbo].[BrugerDevice] ([ID]),
    CONSTRAINT [FK_Eltavle_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);
GO

CREATE CLUSTERED INDEX [CI_ElTavle]
    ON [dbo].[ElTavle]([KundeID] ASC, [ID] ASC);
GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_TypeIDLobeNrAndAar]
    ON [dbo].[ElTavle]([TypeID] ASC, [LoebeNr] ASC, [Aar] ASC) WHERE ([LoebeNr] IS NOT NULL AND [Aar] IS NOT NULL);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_ElTavle]
    ON [dbo].[ElTavle]([ObjektGuid] ASC);
GO

