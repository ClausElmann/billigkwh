CREATE TABLE [dbo].[Sti] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [StaldSektionID]        INT            NOT NULL,
    [StiNr]                 SMALLINT       CONSTRAINT [DF_Sti_StiNr] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_Sti_Beskrivelse] DEFAULT ('') NOT NULL,
    [Areal]                 INT            CONSTRAINT [DF_Sti_Areal] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Sti_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Sti_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Sti_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Sti] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Sti_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Sti_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Sti_StaldSektion] FOREIGN KEY ([StaldSektionID]) REFERENCES [dbo].[StaldSektion] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [CK_Sti]
    ON [dbo].[Sti]([KundeID] ASC, [StaldSektionID] ASC, [Slettet] ASC, [ID] ASC);

