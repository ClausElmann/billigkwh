CREATE TYPE [dbo].[WebShellRequestLogTvp] AS TABLE (
    [ID]                       INT              NULL,
    [TimestampUtc]             DATETIME         NULL,
    [MachineName]              NVARCHAR (50)    NULL,
    [IsDebugRecord]            BIT              NULL,
    [KundeID]                  INT              NULL,
    [BrugerID]                 INT              NULL,
    [UsernameCode]             UNIQUEIDENTIFIER NULL,
    [RequestMethod]            NVARCHAR (500)   NULL,
    [SessionCookie]            NVARCHAR (50)    NULL,
    [LoginUnknownUser]         BIT              NULL,
    [LoginPasswordError]       BIT              NULL,
    [LoginInvalidTimestamp]    BIT              NULL,
    [LoginInvalidAccessRights] BIT              NULL,
    [LoginSwopFailed]          BIT              NULL,
    [UncaughtException]        BIT              NULL,
    [UnknownMethodCall]        BIT              NULL);

