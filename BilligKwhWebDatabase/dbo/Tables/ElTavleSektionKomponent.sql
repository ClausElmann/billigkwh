CREATE TABLE [dbo].[ElTavleSektionKomponent] (
    [ID]               INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]          INT              CONSTRAINT [DF_ElTavleSektionKomponent_KundeID] DEFAULT ((0)) NOT NULL,
    [ElTavleID]        INT              NOT NULL,
    [ElTavleSektionID] INT              NULL,
    [KomponentID]      INT              NOT NULL,
    [Antal]            INT              NOT NULL,
    [Placering]        INT              CONSTRAINT [DF_ElTavleSektionKomponent_Placering] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]       UNIQUEIDENTIFIER NOT NULL,
    [SidstRettet]      DATETIME         CONSTRAINT [DF_ElTavleSektionKomponent_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ElTavleSektionKomponent] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElTavleSektionKomponent_ElKomponent] FOREIGN KEY ([KomponentID]) REFERENCES [dbo].[ElKomponent] ([ID]),
    CONSTRAINT [FK_ElTavleSektionKomponent_ElTavle] FOREIGN KEY ([ElTavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
    CONSTRAINT [FK_ElTavleSektionKomponent_ElTavleSektion1] FOREIGN KEY ([ElTavleSektionID]) REFERENCES [dbo].[ElTavleSektion] ([ID]),
    CONSTRAINT [FK_ElTavleSektionKomponent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_ElTavleSektionKomponent]
    ON [dbo].[ElTavleSektionKomponent]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_ElTavleSektionKomponent]
    ON [dbo].[ElTavleSektionKomponent]([KundeID] ASC, [ID] ASC);

