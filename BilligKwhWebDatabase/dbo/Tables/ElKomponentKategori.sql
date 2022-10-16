CREATE TABLE [dbo].[ElKomponentKategori] (
    [ID]             INT           NOT NULL,
    [Navn]           NVARCHAR (50) NOT NULL,
    [Placering]      INT           NOT NULL,
    [SidstRettet]    DATETIME      CONSTRAINT [DF_ElKomponentKategori_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [ErKabinet]      BIT           CONSTRAINT [DF_ElKomponentKategori_ErKabinet] DEFAULT ((1)) NOT NULL,
    [TavlePlacering] INT           CONSTRAINT [DF_ElKomponentKategori_TavlePlacering] DEFAULT ((0)) NOT NULL,
    [FakturaPlacering] INT NULL, 
    CONSTRAINT [PK_ElKomponentKategori] PRIMARY KEY CLUSTERED ([ID] ASC)
);







