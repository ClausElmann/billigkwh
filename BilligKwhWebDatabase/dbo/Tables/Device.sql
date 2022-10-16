CREATE TABLE [dbo].[Device] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [DeviceID]              NVARCHAR (25)  NOT NULL,
    [TypeID]                SMALLINT       CONSTRAINT [DF_Device_TypeID] DEFAULT ((0)) NOT NULL,
    [KundeID]               INT            CONSTRAINT [DF_Device_KundeID] DEFAULT ((0)) NOT NULL,
    [Nr]                    SMALLINT       CONSTRAINT [DF_Device_Nr] DEFAULT ((0)) NOT NULL,
    [Navn]                  NVARCHAR (50)  CONSTRAINT [DF_Device_Navn] DEFAULT ('') NOT NULL,
    [OpsaetningData]        NVARCHAR (MAX) CONSTRAINT [DF_Device_OpsaetningData] DEFAULT ('') NOT NULL,
    [OpsaetningModtaget]    DATETIME       NULL,
    [Beskrivelse]           NVARCHAR (250) CONSTRAINT [DF_Device_Beskrivelse] DEFAULT ('') NOT NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_Device_OrdreNr] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Device_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Device_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Device_Slettet] DEFAULT ((0)) NOT NULL,
    [Konfiguration]         NVARCHAR (MAX) CONSTRAINT [DF_Device_Configuration] DEFAULT ('') NOT NULL,
    [SendForsinkelse]       SMALLINT       CONSTRAINT [DF_Device_SendDelay] DEFAULT ((0)) NULL,
    [GatewayID]             NVARCHAR (25)  CONSTRAINT [DF_Device_GatewayID] DEFAULT ('0') NOT NULL,
    [SidstOnline]           DATETIME       NULL,
    [Overvaagning]          BIT            CONSTRAINT [DF_Device_Overvaagning] DEFAULT ((1)) NOT NULL,
    [MailAdvisering]        BIT            CONSTRAINT [DF_Device_MailAdvisering] DEFAULT ((0)) NOT NULL,
    [Temperatur]            BIT            CONSTRAINT [DF_Device_Temperatur] DEFAULT ((0)) NOT NULL,
    [LuftFugtighed]         BIT            CONSTRAINT [DF_Device_LuftFugtighed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Device] PRIMARY KEY NONCLUSTERED ([DeviceID] ASC),
    CONSTRAINT [FK_Device_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Device_DeviceType] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[DeviceType] ([ID]),
    CONSTRAINT [FK_Device_Gateway] FOREIGN KEY ([GatewayID]) REFERENCES [dbo].[Gateway] ([DeviceID]),
    CONSTRAINT [FK_Device_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DeviceNrUnique]
    ON [dbo].[Device]([KundeID] ASC, [Nr] ASC);


GO
CREATE CLUSTERED INDEX [CI_Device]
    ON [dbo].[Device]([KundeID] ASC, [DeviceID] ASC);

