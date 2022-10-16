﻿--CREATE TABLE [dbo].[ElTavleSektionKomponentPlacering] (
--    [ID]               INT           IDENTITY (1, 1) NOT NULL,
--    [KundeID]          INT           CONSTRAINT [DF_ElTavleSektionKomponentPlacering_KundeID] DEFAULT ((0)) NOT NULL,
--    [ElTavleID]        INT           NOT NULL,
--    [ElTavleSektionID] INT           NULL,
--    [KomponentID]      INT           NOT NULL,
--    [Placering]        INT           CONSTRAINT [DF_ElTavleSektionKomponentPlacering_Placering] DEFAULT ((0)) NOT NULL,
--    [Navn]             NVARCHAR (50) CONSTRAINT [DF_ElTavleSektionKomponentPlacering_Navn] DEFAULT ('') NOT NULL,
--    [Line]             INT           CONSTRAINT [DF_ElTavleSektionKomponentPlacering_Line] DEFAULT ((1)) NOT NULL,
--    CONSTRAINT [PK_ElTavleSektionKomponentPlacering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
--    CONSTRAINT [FK_ElTavleSektionKomponentPlacering_ElKomponent] FOREIGN KEY ([KomponentID]) REFERENCES [dbo].[ElKomponent] ([ID]),
--    CONSTRAINT [FK_ElTavleSektionKomponentPlacering_ElTavle] FOREIGN KEY ([ElTavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
--    CONSTRAINT [FK_ElTavleSektionKomponentPlacering_ElTavleSektion1] FOREIGN KEY ([ElTavleSektionID]) REFERENCES [dbo].[ElTavleSektion] ([ID]),
--    CONSTRAINT [FK_ElTavleSektionKomponentPlacering_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
--);

