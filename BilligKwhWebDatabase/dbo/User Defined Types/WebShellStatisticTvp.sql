CREATE TYPE [dbo].[WebShellStatisticTvp] AS TABLE (
    [TimestampUtc]             DATETIME      NULL,
    [MachineName]              NVARCHAR (50) NULL,
    [IsHourlyDetails]          BIT           NULL,
    [IsDebugRecord]            BIT           NULL,
    [LoginUnknownUsers]        INT           NULL,
    [LoginPasswordErrors]      INT           NULL,
    [LoginInvalidTimestamps]   INT           NULL,
    [LoginInvalidAccessRights] INT           NULL,
    [UncaughtExceptions]       INT           NULL,
    [UnknownMethodCalls]       INT           NULL,
    [JsonRequestCount]         INT           NULL,
    [JsonMessageBytesIn]       BIGINT        NULL,
    [JsonMessageBytesOut]      BIGINT        NULL);

