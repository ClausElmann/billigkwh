CREATE TABLE [dbo].[KwhConsumptions] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [DateDK]      DATE           NOT NULL,
    [HourDK]      INT            NOT NULL,
    [DeviceId]    INT            NOT NULL,
    [Counter]     BIGINT         NOT NULL,
    [Consumption] DECIMAL (5, 1) CONSTRAINT [DF_KwhConsumptions_C00] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_KwhConsumption] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE CLUSTERED INDEX [CX_KwhConsumptions]
    ON [dbo].[KwhConsumptions]([DateDK] DESC, [HourDK] DESC, [DeviceId] DESC);
GO

