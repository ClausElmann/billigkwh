CREATE TABLE [dbo].[SmartDevices] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Uniqueidentifier] NVARCHAR (250) NOT NULL,
    [CustomerId]       INT            NULL,
    [CreatedUtc]       DATETIME       NOT NULL,
    [LatestContactUtc] DATETIME       NOT NULL,
    [Location]         NVARCHAR (200) CONSTRAINT [DF_Print_Lokation] DEFAULT ('') NOT NULL,
    [ZoneId]           INT            CONSTRAINT [DF_Print_ZoneId] DEFAULT ((1)) NOT NULL,
    [MaxRate]          DECIMAL (4, 2) CONSTRAINT [DF_Print_MaxRate] DEFAULT ((2)) NOT NULL,
    [Deleted]          DATETIME       NULL,
    [Comment]          NVARCHAR (MAX) CONSTRAINT [DF_Print_Kommentar] DEFAULT ('') NOT NULL,
    [Delay] INT NOT NULL DEFAULT 0, 
    [DebugMinutes] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Print] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [CX_Print]
    ON [dbo].[SmartDevices]([Uniqueidentifier] ASC);

