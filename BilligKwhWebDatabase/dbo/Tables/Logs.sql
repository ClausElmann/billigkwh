CREATE TABLE [dbo].[Logs] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [LogLevelId]     INT            NOT NULL,
    [ShortMessage]   VARCHAR (1500) NOT NULL,
    [FullMessage]    VARCHAR (MAX)  NULL,
    [IpAddress]      NVARCHAR (20)  NULL,
    [UserId]         INT            NULL,
    [PageUrl]        NVARCHAR (300) NULL,
    [ReferrerUrl]    NVARCHAR (300) NULL,
    [DateCreatedUtc] DATETIME       CONSTRAINT [DF_SystemLogs_DateCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [Module]         NVARCHAR (100) NULL,
    [DataObject]     VARCHAR (MAX)  NULL,
    [IsHandled]      BIT            NULL,
    CONSTRAINT [PK_SystemLogs] PRIMARY KEY CLUSTERED ([Id] DESC) WITH (STATISTICS_NORECOMPUTE = ON)
);

