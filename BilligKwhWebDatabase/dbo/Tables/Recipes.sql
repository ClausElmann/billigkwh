CREATE TABLE [dbo].[Recipes] (
    [Id]                      INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]              INT            NOT NULL,
    [DeviceId]                INT            NOT NULL,
    [LastUpdatedUtc]          DATETIME       NOT NULL,
    [Priority]                INT            CONSTRAINT [DF_Recipes_Priority] DEFAULT ((0)) NOT NULL,
    [DayTypeId]               INT            CONSTRAINT [DF_Recipes_DaysTypeId] DEFAULT ((0)) NOT NULL,
    [MaxRate]                 DECIMAL (4, 2) NOT NULL,
    [FromHour]                INT            CONSTRAINT [DF_Recipes_FromHour] DEFAULT ((0)) NOT NULL,
    [ToHour]                  INT            CONSTRAINT [DF_Recipes_ToHour] DEFAULT ((23)) NOT NULL,
    [MinHours]                INT            CONSTRAINT [DF_Recipes_MinHours] DEFAULT ((0)) NULL,
    [MinTemperature]          INT            NULL,
    [MaxRateAtMinTemperature] DECIMAL (4, 2) NULL,
    CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED ([Id] ASC)
);









