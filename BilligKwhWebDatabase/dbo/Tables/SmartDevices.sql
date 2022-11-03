CREATE TABLE [dbo].[SmartDevices] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Uniqueidentifier] NVARCHAR (250) NOT NULL,
    [CustomerId]       INT            NULL,
    [CreatedUtc]       DATETIME       NOT NULL,
    [LatestContactUtc] DATETIME       NOT NULL,
    [Location]         NVARCHAR (200) CONSTRAINT [DF_SmartDevice_Lokation] DEFAULT ('') NOT NULL,
    [ZoneId]           INT            CONSTRAINT [DF_SmartDevice_ZoneId] DEFAULT ((1)) NOT NULL,
    [MaxRate]          DECIMAL (4, 2) CONSTRAINT [DF_SmartDevice_MaxRate] DEFAULT ((2)) NOT NULL,
    [Deleted]          DATETIME       NULL,
    [Comment]          NVARCHAR (MAX) CONSTRAINT [DF_SmartDevice_Kommentar] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SmartDevice] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [CX_SmartDevice]
    ON [dbo].[SmartDevices]([Uniqueidentifier] ASC);

