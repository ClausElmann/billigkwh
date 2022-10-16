CREATE TABLE [dbo].[DeviceConnectionStatus] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]      INT           CONSTRAINT [DF_DeviceConnectionStatus_KundeID] DEFAULT ((0)) NOT NULL,
    [GatewayID]    NVARCHAR (25) CONSTRAINT [DF_DeviceConnectionStatus_GatewayID] DEFAULT ('') NOT NULL,
    [DeviceID]     NVARCHAR (25) CONSTRAINT [DF_DeviceConnectionStatus_DeviceID] DEFAULT ((0)) NOT NULL,
    [Dato]         DATETIME      CONSTRAINT [DF_DeviceConnectionStatus_Dato] DEFAULT (getdate()) NOT NULL,
    [StatusTypeID] SMALLINT      CONSTRAINT [DF_DeviceConnectionStatus_StatusType] DEFAULT ((0)) NOT NULL,
    [SidstRettet]  DATETIME      CONSTRAINT [DF_DeviceConnectionStatus_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DeviceConnectionStatus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DeviceConnectionStatus_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID]),
    CONSTRAINT [FK_DeviceConnectionStatus_DeviceConnectionStatusType] FOREIGN KEY ([StatusTypeID]) REFERENCES [dbo].[DeviceConnectionStatusType] ([ID]),
    CONSTRAINT [FK_DeviceConnectionStatus_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DeviceConnectionStatus]
    ON [dbo].[DeviceConnectionStatus]([GatewayID] ASC, [DeviceID] ASC);

