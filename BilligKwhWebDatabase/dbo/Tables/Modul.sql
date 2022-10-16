CREATE TABLE [dbo].[Modul] (
    [ID]          INT            NOT NULL,
    [Navn]        NVARCHAR (50)  NOT NULL,
    [Placering]   INT            NOT NULL,
    [Beskrivelse] NVARCHAR (500) CONSTRAINT [DF_Modul_Beskrivelse] DEFAULT ('') NULL,
    [ErOffentlig] BIT            CONSTRAINT [DF_Modul_ErKundemodul] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ModulNy] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

