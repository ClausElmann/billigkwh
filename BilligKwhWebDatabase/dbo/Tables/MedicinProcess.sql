CREATE TABLE [dbo].[MedicinProcess] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [StaldID]               INT              NOT NULL,
    [SektionID]             INT              NULL,
    [VejeholdID]            INT              NULL,
    [StiID]                 INT              NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_MedicinProcess_Bemaerkning] DEFAULT ('') NOT NULL,
    [MedicinID]             INT              NOT NULL,
    [AarsagID]              INT              CONSTRAINT [DF_MedicinProcess_AarsagID] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [AntalBehandlinger]     SMALLINT         CONSTRAINT [DF_MedicinProcess_ManglendeBehandlinger1] DEFAULT ((1)) NOT NULL,
    [ManglendeBehandlinger] SMALLINT         CONSTRAINT [DF_MedicinProcess_ManglendeBehandlinger] DEFAULT ((0)) NOT NULL,
    [NaesteBehandling]      DATETIME         CONSTRAINT [DF_MedicinProcess_NaesteBehandling] DEFAULT (getdate()) NOT NULL,
    [Afsluttet]             DATETIME         NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_MedicinProcess_DeviceID] DEFAULT ('') NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MedicinProcess_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MedicinProcess_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MedicinProcess] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MedicinProcess_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MedicinProcess_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MedicinProcess_ListePost] FOREIGN KEY ([AarsagID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_MedicinProcess_Medicin] FOREIGN KEY ([MedicinID]) REFERENCES [dbo].[Medicin] ([ID]),
    CONSTRAINT [FK_MedicinProcess_StaldSektion] FOREIGN KEY ([SektionID]) REFERENCES [dbo].[StaldSektion] ([ID]),
    CONSTRAINT [FK_MedicinProcess_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_MedicinProcess_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MedicinProcess]
    ON [dbo].[MedicinProcess]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_MedicinProcess]
    ON [dbo].[MedicinProcess]([KundeID] ASC, [StaldID] ASC, [Slettet] ASC, [ID] ASC);

