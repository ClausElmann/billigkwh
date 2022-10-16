CREATE TABLE [dbo].[Behandling] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [InitialBehandlingID]   INT              CONSTRAINT [DF_Behandling_InitialBehandlingID] DEFAULT ((0)) NOT NULL,
    [StaldID]               INT              NOT NULL,
    [SektionID]             INT              NOT NULL,
    [VejeholdID]            INT              NOT NULL,
    [StiID]                 INT              NOT NULL,
    [MedicinID]             INT              NOT NULL,
    [AarsagID]              INT              CONSTRAINT [DF_Behandling_AarsagID] DEFAULT ((0)) NOT NULL,
    [BehandlingNr]          SMALLINT         CONSTRAINT [DF_Behandling_BehandlingNr] DEFAULT ((1)) NOT NULL,
    [AntalBehandlinger]     SMALLINT         CONSTRAINT [DF_Behandling_ManglendeBehandlinger1] DEFAULT ((1)) NOT NULL,
    [LbNr]                  SMALLINT         CONSTRAINT [DF_Behandling_LbNr] DEFAULT ((0)) NOT NULL,
    [Dag]                   SMALLINT         CONSTRAINT [DF_Behandling_Dag] DEFAULT ((0)) NOT NULL,
    [AntalGalte]            SMALLINT         CONSTRAINT [DF_Behandling_AntalGalte] DEFAULT ((0)) NOT NULL,
    [AntalSogrise]          SMALLINT         CONSTRAINT [DF_Behandling_AntalSogrise] DEFAULT ((0)) NOT NULL,
    [Maengde]               DECIMAL (12, 2)  CONSTRAINT [DF_Behandling_Maengde] DEFAULT ((0)) NOT NULL,
    [Pris]                  DECIMAL (6, 2)   CONSTRAINT [DF_Behandling_Pris] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [Udfores]               DATETIME         NOT NULL,
    [Udfort]                DATETIME         NULL,
    [UdfortAfBrugerID]      INT              CONSTRAINT [DF_Behandling_UdfortAfBrugerID] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [Stoppet]               BIT              CONSTRAINT [DF_Behandling_Afsluttet] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_Behandling_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_Behandling_Bemaerkning] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_Behandling_Slettet] DEFAULT ((0)) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_Behandling_DeviceID] DEFAULT ('') NULL,
    CONSTRAINT [PK_Behandling] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Behandling_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Behandling_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Behandling_ListePost] FOREIGN KEY ([AarsagID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_Behandling_Medicin] FOREIGN KEY ([MedicinID]) REFERENCES [dbo].[Medicin] ([ID]),
    CONSTRAINT [FK_Behandling_OprettetAfBruger] FOREIGN KEY ([UdfortAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Behandling_Stald] FOREIGN KEY ([StaldID]) REFERENCES [dbo].[Stald] ([ID]),
    CONSTRAINT [FK_Behandling_StaldSektion] FOREIGN KEY ([SektionID]) REFERENCES [dbo].[StaldSektion] ([ID]),
    CONSTRAINT [FK_Behandling_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_Behandling_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Behandling]
    ON [dbo].[Behandling]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_Behandling]
    ON [dbo].[Behandling]([KundeID] ASC, [ID] ASC);

