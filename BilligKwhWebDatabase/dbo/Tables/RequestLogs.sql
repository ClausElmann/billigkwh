CREATE TABLE [dbo].[RequestLogs] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]              INT            NULL,
    [Path]                NVARCHAR (200) NOT NULL,
    [QueryString]         NVARCHAR (200) NOT NULL,
    [Method]              NVARCHAR (200) NOT NULL,
    [Payload]             NVARCHAR (MAX) NULL,
    [Response]            NVARCHAR (MAX) NOT NULL,
    [ResponseCode]        NVARCHAR (50)  NOT NULL,
    [RequestedOnUtc]      DATETIME       NOT NULL,
    [Ticks]               INT            NOT NULL,
    [IsSuccessStatusCode] BIT            NOT NULL,
    [IpAddress]           NVARCHAR (15)  NULL,
    CONSTRAINT [PK_RequestLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);



