CREATE TYPE [dbo].[DeviceConnectionStatusTvp] AS TABLE (
    [ID]           INT           NULL,
    [KundeID]      INT           NULL,
    [GatewayID]    NVARCHAR (25) NULL,
    [DeviceID]     NVARCHAR (25) NULL,
    [Dato]         DATETIME      NULL,
    [StatusTypeID] SMALLINT      NULL,
    [SidstRettet]  DATETIME      NULL);

