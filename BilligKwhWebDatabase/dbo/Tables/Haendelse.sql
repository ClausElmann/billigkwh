CREATE TABLE [dbo].[Haendelse] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [Dato]                  DATE             CONSTRAINT [DF_Haendelse_Dato] DEFAULT (getdate()) NOT NULL,
    [KildeVejeholdDataID]   BIGINT           NULL,
    [VejeholdDataID]        BIGINT           NULL,
    [StaldID]               INT              CONSTRAINT [DF_VejeholdEvent_StaldID] DEFAULT ((0)) NOT NULL,
    [SektionID]             INT              CONSTRAINT [DF_VejeholdEvent_SektionID] DEFAULT ((0)) NOT NULL,
    [StiID]                 INT              CONSTRAINT [DF_VejeholdEvent_StiID] DEFAULT ((0)) NOT NULL,
    [VejeholdID]            INT              NOT NULL,
    [HoldID]                INT              CONSTRAINT [DF_VejeholdEvent_HoldID] DEFAULT ((0)) NOT NULL,
    [HaendelseTypeID]       INT              NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_VejeholdEvent_Bemaerkning] DEFAULT ('') NOT NULL,
    [LbNr]                  SMALLINT         CONSTRAINT [DF_VejeholdEvent_LbNr] DEFAULT ((0)) NOT NULL,
    [AntalGalteFjernet]     SMALLINT         CONSTRAINT [DF_VejeholdEvent_AntalGalteFjernet] DEFAULT ((0)) NOT NULL,
    [AntalSogriseFjernet]   SMALLINT         CONSTRAINT [DF_VejeholdEvent_AntalSogriseFjernet] DEFAULT ((0)) NOT NULL,
    [AntalFjernet]          INT              CONSTRAINT [DF_Haendelse_AntalFjernet] DEFAULT ((0)) NOT NULL,
    [VaegtFjernet]          INT              CONSTRAINT [DF_VejeholdEvent_VaegtFjernet] DEFAULT ((0)) NOT NULL,
    [AarsagID]              INT              CONSTRAINT [DF_VejeholdEvent_DoedAarsagListepostID] DEFAULT ((0)) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [BrugerDeviceID]        INT              NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [SidstRettetAfBrugerID] INT              NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_VejeholdEvent_Slettet] DEFAULT ((0)) NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_VejeholdEvent_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [Dag]                   SMALLINT         CONSTRAINT [DF_VejeholdEvent_Dag] DEFAULT ((0)) NOT NULL,
    [Pris]                  DECIMAL (6, 2)   CONSTRAINT [DF_VejeholdEvent_Pris] DEFAULT ((0)) NOT NULL,
    [Udfort]                DATETIME         CONSTRAINT [DF_VejeholdEvent_Udfort] DEFAULT (getdate()) NOT NULL,
    [UdfortAfBrugerID]      INT              CONSTRAINT [DF_VejeholdEvent_UdfortAfBrugerID] DEFAULT ((0)) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_VejeholdEvent_DeviceID] DEFAULT ('') NULL,
    [SlutAntalGalte]        SMALLINT         CONSTRAINT [DF_Haendelse_StartAntalSogrise] DEFAULT ((0)) NOT NULL,
    [SlutAntalSogrise]      SMALLINT         CONSTRAINT [DF_Haendelse_SlutAntalSogrise] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VejeholdEvent] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_VejeholdEvent_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Bruger1] FOREIGN KEY ([UdfortAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_EventType] FOREIGN KEY ([HaendelseTypeID]) REFERENCES [dbo].[HaendelseType] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Hold] FOREIGN KEY ([HoldID]) REFERENCES [dbo].[Hold] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_ListePost] FOREIGN KEY ([AarsagID]) REFERENCES [dbo].[ListePost] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Stald] FOREIGN KEY ([StaldID]) REFERENCES [dbo].[Stald] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_StaldSektion] FOREIGN KEY ([SektionID]) REFERENCES [dbo].[StaldSektion] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Sti] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_VejeholdEvent_Vejehold2] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Haendelse]
    ON [dbo].[Haendelse]([ObjektGuid] ASC);


GO
CREATE CLUSTERED INDEX [CI_Haendelse]
    ON [dbo].[Haendelse]([KundeID] ASC, [Slettet] ASC, [VejeholdID] ASC);

