CREATE TABLE [dbo].[FareProcess] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [StartDato]             DATETIME       NOT NULL,
    [Kommentar]             NVARCHAR (MAX) CONSTRAINT [DF_Process_Beskrivelse] DEFAULT ('') NOT NULL,
    [StiID]                 INT            CONSTRAINT [DF_FareProcess_StiID] DEFAULT ((0)) NOT NULL,
    [SoID]                  INT            NOT NULL,
    [AntalGalte]            SMALLINT       CONSTRAINT [DF_FareProcess_AntalGalte] DEFAULT ((0)) NOT NULL,
    [AntalSoGrise]          SMALLINT       CONSTRAINT [DF_FareProcess_AntalSoGrise] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME       CONSTRAINT [DF_FareProcess_SidstRettet1] DEFAULT (getdate()) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Process_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Process_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Process_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FareProcess_So1] FOREIGN KEY ([SoID]) REFERENCES [dbo].[So] ([ID]),
    CONSTRAINT [FK_FareProcess_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_Process_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Process_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

