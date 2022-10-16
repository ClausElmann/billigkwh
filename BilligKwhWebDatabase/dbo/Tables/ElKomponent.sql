CREATE TABLE [dbo].[ElKomponent] (
    [ID]              INT           NOT NULL,
    [Navn]            NVARCHAR (50) CONSTRAINT [DF_ElKomponent_Navn] DEFAULT ('') NOT NULL,
    [KategoriID]      INT           NOT NULL,
    [Placering]       INT           CONSTRAINT [DF_ElKomponent_Placering] DEFAULT ((0)) NOT NULL,
    [Modul]           INT           CONSTRAINT [DF_ElKomponent_Modul] DEFAULT ((4)) NOT NULL,
    [DinSkinner]      INT           CONSTRAINT [DF_ElKomponent_Striber] DEFAULT ((0)) NOT NULL,
    [SidstRettet]     DATETIME      CONSTRAINT [DF_ElKomponent_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]         BIT           CONSTRAINT [DF_ElKomponent_Slettet] DEFAULT ((0)) NOT NULL,
    [KostKomponent]   INT           CONSTRAINT [DF_ElKomponent_KostKomponent] DEFAULT ((0)) NOT NULL,
    [KostHjaelpeMat]  INT           CONSTRAINT [DF_ElKomponent_KostHjaelpemat] DEFAULT ((0)) NOT NULL,
    [KostLoen]        INT           CONSTRAINT [DF_ElKomponent_KostLoen] DEFAULT ((0)) NOT NULL,
    [DB]              INT           CONSTRAINT [DF_ElKomponent_DB] DEFAULT ((30)) NULL,
    [BruttoPris]      INT           CONSTRAINT [DF_ElKomponent_BruttoPris] DEFAULT ((0)) NOT NULL,
    [DaekningsBidrag] INT           CONSTRAINT [DF_ElKomponent_DB1] DEFAULT ((30)) NOT NULL,
    [HPFI]            BIT           CONSTRAINT [DF_ElKomponent_HPFI] DEFAULT ((0)) NOT NULL,
    [KombiRelae]      BIT           CONSTRAINT [DF_ElKomponent_Kombi] DEFAULT ((0)) NOT NULL,
    [UdenBeskyttelse] BIT           CONSTRAINT [DF_ElKomponent_Uden] DEFAULT ((0)) NOT NULL,
    [MontageMinutter] INT           CONSTRAINT [DF_ElKomponent_MontageMinutter] DEFAULT ((0)) NOT NULL,
    [TavlePlacering]  INT           CONSTRAINT [DF_ElKomponent_TavlePlacering] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ElKomponent] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElKomponent_ElKomponentKategori] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[ElKomponentKategori] ([ID])
);







