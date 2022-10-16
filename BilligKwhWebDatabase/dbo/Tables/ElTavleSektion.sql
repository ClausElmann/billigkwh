CREATE TABLE [dbo].[ElTavleSektion] (
    [ID]              INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]         INT              CONSTRAINT [DF_ElTavleSektion_KundeID] DEFAULT ((0)) NOT NULL,
    [TavleID]         INT              NOT NULL,
    [Placering]       INT              NOT NULL,
    [ObjektGuid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ElTavleSektion_SektionGuid] DEFAULT (newid()) NOT NULL,
    [TypeID]          INT              CONSTRAINT [DF_ElTavleSektion_TypeID] DEFAULT ((0)) NOT NULL,
    [HPFIKomponentID] INT              NULL,
    [SidstRettet]     DATETIME         CONSTRAINT [DF_ElTavleSektion_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ElTavleSektion] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElTavleSektion_Eltavle] FOREIGN KEY ([TavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
    CONSTRAINT [FK_ElTavleSektion_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_ElTavleSektion]
    ON [dbo].[ElTavleSektion]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_ElTavleSektion]
    ON [dbo].[ElTavleSektion]([KundeID] ASC, [ID] ASC);

