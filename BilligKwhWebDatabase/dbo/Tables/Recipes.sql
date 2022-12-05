CREATE TABLE [dbo].[Recipes] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]     INT            NOT NULL,
    [DeviceId]       INT            NOT NULL,
    [LastUpdatedUtc] DATETIME       NOT NULL,
    [MaxRate]        DECIMAL (4, 2) NOT NULL,
    [ZoneId]         INT            CONSTRAINT [DF_Recipe_Zone] DEFAULT ((1)) NOT NULL,
    [MinTempOveride]       INT            NULL,
    [MaxTempOveride]       INT            NULL
    CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED ([Id] ASC)
);



