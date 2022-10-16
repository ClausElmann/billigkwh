CREATE TABLE [dbo].[LogRequest] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [ServerNr]        INT            NULL,
    [Duration]        FLOAT (53)     CONSTRAINT [DF_Requests_Duration] DEFAULT ((0)) NOT NULL,
    [RequestPageID]   INT            NULL,
    [RequestQuery]    NVARCHAR (MAX) NULL,
    [RefererPageID]   INT            NULL,
    [RefererQuery]    NVARCHAR (MAX) NULL,
    [RequestDateTime] DATETIME       CONSTRAINT [DF_Requests_RequestDateTime] DEFAULT (getdate()) NOT NULL,
    [RemoteIp]        NVARCHAR (50)  NULL,
    [RemoteIpID]      INT            CONSTRAINT [DF_LogRequest_RemoteIpID] DEFAULT ((33)) NOT NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_LogRequest_LogRequestIP] FOREIGN KEY ([RemoteIpID]) REFERENCES [dbo].[LogRequestIP] ([ID]),
    CONSTRAINT [FK_LogRequests_LogRequestPages] FOREIGN KEY ([RequestPageID]) REFERENCES [dbo].[LogRequestPage] ([ID]),
    CONSTRAINT [FK_LogRequests_LogRequestPages1] FOREIGN KEY ([RefererPageID]) REFERENCES [dbo].[LogRequestPage] ([ID])
);

