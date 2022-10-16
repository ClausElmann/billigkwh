CREATE TABLE [dbo].[MinkBur] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [RaekkeID]              INT            NOT NULL,
    [BurNr]                 SMALLINT       CONSTRAINT [DF_MinkBur_MinkBurNr] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkBur_Beskrivelse] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkBur_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkBur_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkBur_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkBur] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkBur_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkBur_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MinkBur]
    ON [dbo].[MinkBur]([KundeID] ASC, [ID] ASC);

