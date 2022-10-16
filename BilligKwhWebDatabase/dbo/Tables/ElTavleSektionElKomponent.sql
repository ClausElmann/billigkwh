CREATE TABLE [dbo].[ElTavleSektionElKomponent] (
    [ID]               INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]          INT              CONSTRAINT [DF_ElTavleSektionElKomponent_KundeID] DEFAULT ((0)) NOT NULL,
    [ElTavleID]        INT              NOT NULL,
    [ElTavleSektionID] INT              NOT NULL,
    [KomponentID]      INT              NOT NULL,
    [ObjektGuidApp]    UNIQUEIDENTIFIER CONSTRAINT [DF_ElTavleSektionElKomponent_SektionGuid] DEFAULT (newid()) NULL,
    [Placering]        INT              CONSTRAINT [DF_ElTavleSektionElKomponent_Placering] DEFAULT ((0)) NOT NULL,
    [SidstRettet]      DATETIME         CONSTRAINT [DF_ElTavleSektionElKomponent_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Navn]             NVARCHAR (50)    CONSTRAINT [DF_ElTavleSektionElKomponent_Navn] DEFAULT ('') NOT NULL,
    [Line]             INT              CONSTRAINT [DF_ElTavleSektionElKomponent_Line] DEFAULT ((1)) NOT NULL,
    [ErExtraDisp]      BIT              DEFAULT ((0)) NOT NULL,
    [AngivetNavn]      BIT              CONSTRAINT [DF_ElTavleSektionElKomponent_AngivetNavn] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ElTavleSektionElKomponent] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElTavleSektionElKomponent_ElKomponent] FOREIGN KEY ([KomponentID]) REFERENCES [dbo].[ElKomponent] ([ID]),
    CONSTRAINT [FK_ElTavleSektionElKomponent_ElTavle] FOREIGN KEY ([ElTavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
    CONSTRAINT [FK_ElTavleSektionElKomponent_ElTavleSektion] FOREIGN KEY ([ElTavleSektionID]) REFERENCES [dbo].[ElTavleSektion] ([ID]),
    CONSTRAINT [FK_ElTavleSektionElKomponent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);



GO
CREATE CLUSTERED INDEX [CI_ElTavleSektionElKomponent]
    ON [dbo].[ElTavleSektionElKomponent]([KundeID] ASC, [ID] ASC);

