CREATE TABLE [dbo].[Dokument] (
    [ID]                 INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]            INT              NOT NULL,
    [ObjektGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Dokument_DokumentGuid] DEFAULT (newid()) NOT NULL,
    [RefTypeID]          INT              CONSTRAINT [DF_Dokument_TypeID] DEFAULT ((0)) NOT NULL,
    [RefGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_Dokument_ObjektGuid1] DEFAULT (newid()) NOT NULL,
    [FiltypeID]          INT              NOT NULL,
    [Filnavn]            NVARCHAR (250)   NOT NULL,
    [Beskrivelse]        NVARCHAR (MAX)   CONSTRAINT [DF_Dokument_Beskrivelse] DEFAULT ('') NOT NULL,
    [FilData]            VARBINARY (MAX)  NOT NULL,
    [FilStørrelse]       INT              CONSTRAINT [DF_Dokument_FilStørrelse] DEFAULT ((0)) NOT NULL,
    [BrugerDeviceID]     INT              NULL,
    [Oprettet]           DATETIME         CONSTRAINT [DF_Dokument_Oprettet] DEFAULT (getdate()) NOT NULL,
    [OprettetAfBrugerID] INT              NOT NULL,
    [SidstRettet]        DATETIME         CONSTRAINT [DF_Dokument_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Xkoordinat]         DECIMAL (12, 3)  CONSTRAINT [DF_Dokument_Xkoordinat] DEFAULT ((0)) NOT NULL,
    [Ykoordinat]         DECIMAL (12, 3)  CONSTRAINT [DF_Dokument_Ykoordinat] DEFAULT ((0)) NOT NULL,
    [Slettet]            BIT              CONSTRAINT [DF_Dokument_Slettet] DEFAULT ((0)) NOT NULL,
    [ModulDokumentID]    INT              NULL,
    [HelpdeskID]         INT              NULL,
    [InstillingEnumID]   INT              NULL,
    [Info]               NVARCHAR (MAX)   CONSTRAINT [DF_Dokument_Info] DEFAULT ('') NULL,
    [DokumentGuid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Dokument_DokumentGuid1] DEFAULT (newid()) NULL,
    [RefObjektGuid]      VARCHAR (50)     CONSTRAINT [DF_Dokument_RefObjektGuid1] DEFAULT (newid()) NULL,
    [RefID]              INT              CONSTRAINT [DF_Dokument_ObjectID] DEFAULT ((0)) NULL,
    [DeviceID]           NVARCHAR (50)    CONSTRAINT [DF_Dokument_DeviceID] DEFAULT ('') NULL,
    CONSTRAINT [PK_Dokument] PRIMARY KEY NONCLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Dokument_Bruger] FOREIGN KEY ([OprettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Dokument_BrugerDevice] FOREIGN KEY ([BrugerDeviceID]) REFERENCES [dbo].[BrugerDevice] ([ID]),
    CONSTRAINT [FK_Dokument_Filtype] FOREIGN KEY ([FiltypeID]) REFERENCES [dbo].[Filtype] ([ID]),
    CONSTRAINT [FK_Dokument_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Dokument]
    ON [dbo].[Dokument]([ObjektGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dokument_SidstRettet]
    ON [dbo].[Dokument]([SidstRettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_Dokument]
    ON [dbo].[Dokument]([KundeID] ASC, [RefTypeID] ASC, [ID] ASC);

