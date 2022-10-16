CREATE TABLE [dbo].[Medicinering] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [MedicinProcessID]      INT              NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_Medicinering_Bemaerkning] DEFAULT ('') NOT NULL,
    [LbNr]                  SMALLINT         CONSTRAINT [DF_Medicinering_LbNr_1] DEFAULT ((0)) NOT NULL,
    [Dag]                   SMALLINT         CONSTRAINT [DF_Medicinering_LbNr] DEFAULT ((0)) NOT NULL,
    [AntalGalte]            SMALLINT         CONSTRAINT [DF_Medicinering_AntalGalte] DEFAULT ((0)) NOT NULL,
    [AntalSogrise]          SMALLINT         CONSTRAINT [DF_Medicinering_AntalSogrise] DEFAULT ((0)) NOT NULL,
    [Maengde]               DECIMAL (6, 2)   CONSTRAINT [DF_Medicinering_Maengde] DEFAULT ((0)) NOT NULL,
    [Pris]                  DECIMAL (6, 2)   CONSTRAINT [DF_Medicinering_Maengde1] DEFAULT ((0)) NOT NULL,
    [Udfort]                DATETIME         CONSTRAINT [DF_Medicinering_Udfort] DEFAULT (getdate()) NOT NULL,
    [BehandlingNr]          SMALLINT         CONSTRAINT [DF_Medicinering_BehandlingNr] DEFAULT ((1)) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_Medicinering_DeviceID] DEFAULT ('') NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_Medicinering_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_Medicinering_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Medicinering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Medicinering_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Medicinering_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Medicinering_MedicinProcess] FOREIGN KEY ([MedicinProcessID]) REFERENCES [dbo].[MedicinProcess] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Medicinering]
    ON [dbo].[Medicinering]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_Medicinering]
    ON [dbo].[Medicinering]([KundeID] ASC, [MedicinProcessID] ASC, [Slettet] ASC, [ID] ASC);

