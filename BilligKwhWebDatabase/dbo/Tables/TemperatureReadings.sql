CREATE TABLE [dbo].[TemperatureReadings] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [DatetimeUtc] DATETIME       NOT NULL,
    [DeviceId]    INT            NOT NULL,
    [Temperature] DECIMAL (4, 1) NOT NULL,
    [IsRunning]   BIT            CONSTRAINT [DF_TemperatureReadings_IsRunning] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TemperatureReadings] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);






GO
CREATE UNIQUE CLUSTERED INDEX [CX_TemperatureReadings]
    ON [dbo].[TemperatureReadings]([DatetimeUtc] DESC, [DeviceId] DESC);

