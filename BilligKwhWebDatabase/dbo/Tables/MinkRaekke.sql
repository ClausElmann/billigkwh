CREATE TABLE [dbo].[MinkRaekke] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [HusID]                 INT            NOT NULL,
    [RaekkeNr]              SMALLINT       CONSTRAINT [DF_MinkRaekke_Nr] DEFAULT ((0)) NOT NULL,
    [StartBur]              SMALLINT       CONSTRAINT [DF_MinkRaekke_StartBur] DEFAULT ((1)) NOT NULL,
    [Slutbur]               SMALLINT       CONSTRAINT [DF_MinkRaekke_StartBur1] DEFAULT ((1)) NOT NULL,
    [AntalBure]             SMALLINT       CONSTRAINT [DF_MinkRaekke_AntalStier] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkRaekke_Beskrivelse] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkRaekke_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkRaekke_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkRaekke_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkRaekke] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkRaekke_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkRaekke_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkRaekke_MinkHus] FOREIGN KEY ([HusID]) REFERENCES [dbo].[MinkHus] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MinkRaekke]
    ON [dbo].[MinkRaekke]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

