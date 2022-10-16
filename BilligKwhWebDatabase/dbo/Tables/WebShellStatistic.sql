CREATE TABLE [dbo].[WebShellStatistic] (
    [TimestampUtc]             DATETIME      NOT NULL,
    [MachineName]              NVARCHAR (50) CONSTRAINT [DF_WebShellStatistics_MachineName] DEFAULT ('') NOT NULL,
    [IsHourlyDetails]          BIT           CONSTRAINT [DF_WebShellStatistic_IsHourlyDetails] DEFAULT ((0)) NOT NULL,
    [IsDebugRecord]            BIT           CONSTRAINT [DF_WebShellStatistic_IsDebugRecord] DEFAULT ((0)) NOT NULL,
    [LoginUnknownUsers]        INT           CONSTRAINT [DF_WebShellStatistic_LoginUnknownUsers] DEFAULT ((0)) NOT NULL,
    [LoginPasswordErrors]      INT           CONSTRAINT [DF_WebShellStatistic_LoginPasswordErrors] DEFAULT ((0)) NOT NULL,
    [LoginInvalidTimestamps]   INT           CONSTRAINT [DF_WebShellStatistic_LoginInvalidTimestamps] DEFAULT ((0)) NOT NULL,
    [LoginInvalidAccessRights] INT           CONSTRAINT [DF_WebShellStatistic_LoginInvalidAccessRights] DEFAULT ((0)) NOT NULL,
    [UncaughtExceptions]       INT           CONSTRAINT [DF_WebShellStatistic_UncaughtExceptions] DEFAULT ((0)) NOT NULL,
    [UnknownMethodCalls]       INT           CONSTRAINT [DF_WebShellStatistic_UnknownMethodCalls] DEFAULT ((0)) NOT NULL,
    [JsonRequestCount]         INT           NOT NULL,
    [JsonMessageBytesIn]       BIGINT        CONSTRAINT [DF_WebShellStatistic_MessageBytesIn] DEFAULT ((0)) NOT NULL,
    [JsonMessageBytesOut]      BIGINT        CONSTRAINT [DF_WebShellStatistic_MessageBytesOut] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WebShellStatistic] PRIMARY KEY CLUSTERED ([TimestampUtc] ASC, [MachineName] ASC)
);

