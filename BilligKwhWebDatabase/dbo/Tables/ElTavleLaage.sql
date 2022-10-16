CREATE TABLE [dbo].[ElTavleLaage] (
    [ID]              INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]         INT              CONSTRAINT [DF_ElTavleLaage_KundeID] DEFAULT ((0)) NOT NULL,
    [TavleID]         INT              NOT NULL,
    [Navn]   NVARCHAR(50)              NOT NULL DEFAULT '',
    FabrikatId  int not null,   
    [Bredde] INT NOT NULL DEFAULT 400, 
    [DinSkinner] int not null 
    CONSTRAINT [PK_ElTavleLaage] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ElTavleLaage_Eltavle] FOREIGN KEY ([TavleID]) REFERENCES [dbo].[ElTavle] ([ID]),
    CONSTRAINT [FK_ElTavleLaage_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);