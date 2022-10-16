CREATE TABLE [dbo].[WebShellException] (
    [ID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [Timestamp]            DATETIME       NOT NULL,
    [MachineName]          NVARCHAR (50)  CONSTRAINT [DF_WebShellExceptions_MachineName] DEFAULT ('') NOT NULL,
    [KundeID]              INT            NULL,
    [BrugerID]             INT            NULL,
    [Method]               NVARCHAR (200) NOT NULL,
    [Request]              NVARCHAR (MAX) CONSTRAINT [DF_RequestJson_Request] DEFAULT ('') NOT NULL,
    [RequestLength]        BIGINT         CONSTRAINT [DF_WebShellException_RequestLength] DEFAULT ((0)) NOT NULL,
    [RequestContentLength] BIGINT         CONSTRAINT [DF_WebShellException_RequestMessageLength] DEFAULT ((0)) NOT NULL,
    [StackTrace]           NVARCHAR (MAX) CONSTRAINT [DF_WebShellException_StackTrace] DEFAULT ('') NOT NULL,
    [IsDebugRecord]        BIT            CONSTRAINT [DF_WebShellException_IsDebugRecord] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WebShellException] PRIMARY KEY CLUSTERED ([ID] ASC)
);

