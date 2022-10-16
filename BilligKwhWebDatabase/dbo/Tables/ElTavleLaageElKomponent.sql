CREATE TABLE [dbo].[ElTavleLaageElKomponent] (
    [ID]               INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]          INT              CONSTRAINT [DF_ElTavleLaageElKomponent_KundeID] DEFAULT ((0)) NOT NULL,
    [ElTavleID]        INT              NOT NULL,
    [ElTavleLaageId] INT              NULL,
    [KomponentID]      INT              NOT NULL,
    [Placering]        INT              CONSTRAINT [DF_ElTavleLaageElKomponent_Placering] DEFAULT ((0)) NOT NULL,
    [SidstRettet]      DATETIME         CONSTRAINT [DF_ElTavleLaageElKomponent_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Navn]             NVARCHAR (50)    CONSTRAINT [DF_ElTavleLaageElKomponent_Navn] DEFAULT ('') NOT NULL,
    [Line]             INT              CONSTRAINT [DF_ElTavleLaageElKomponent_Line] DEFAULT ((1)) NOT NULL,
    [ErExtraDisp]      BIT              DEFAULT ((0)) NOT NULL,
    [AngivetNavn]      BIT              CONSTRAINT [DF_ElTavleLaageElKomponent_AngivetNavn] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ElTavleLaageElKomponent] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElTavleLaageElKomponent_ElKomponent] FOREIGN KEY ([KomponentID]) REFERENCES [dbo].[ElKomponent] ([ID]),
    CONSTRAINT [FK_ElTavleLaageElKomponent_ElTavle] FOREIGN KEY ([ElTavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
    CONSTRAINT [FK_ElTavleLaageElKomponent_ElTavleLaage] FOREIGN KEY ([ElTavleLaageId]) REFERENCES [dbo].[ElTavleLaage] ([ID]),
    CONSTRAINT [FK_ElTavleLaageElKomponent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);



GO
CREATE CLUSTERED INDEX [CI_ElTavleLaageElKomponent]
    ON [dbo].[ElTavleLaageElKomponent]([KundeID] ASC, [ID] ASC);

