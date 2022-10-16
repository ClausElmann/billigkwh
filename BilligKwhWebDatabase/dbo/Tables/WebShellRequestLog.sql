CREATE TABLE [dbo].[WebShellRequestLog] (
    [ID]                       INT              IDENTITY (1, 1) NOT NULL,
    [TimestampUtc]             DATETIME         NOT NULL,
    [MachineName]              NVARCHAR (50)    NOT NULL,
    [IsDebugRecord]            BIT              NOT NULL,
    [KundeID]                  INT              NULL,
    [BrugerID]                 INT              NULL,
    [UsernameCode]             UNIQUEIDENTIFIER NULL,
    [RequestMethod]            NVARCHAR (500)   NOT NULL,
    [SessionCookie]            NVARCHAR (50)    NOT NULL,
    [LoginUnknownUser]         BIT              NOT NULL,
    [LoginPasswordError]       BIT              NOT NULL,
    [LoginInvalidTimestamp]    BIT              NOT NULL,
    [LoginInvalidAccessRights] BIT              NOT NULL,
    [LoginSwopFailed]          BIT              CONSTRAINT [DF_WebShellRequestLog_LoginSwopFailed] DEFAULT ((0)) NOT NULL,
    [UncaughtException]        BIT              NOT NULL,
    [UnknownMethodCall]        BIT              NOT NULL,
    CONSTRAINT [PK_WebShellRequestLog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

