CREATE TABLE [dbo].[PowerConsumption] (
    [DateDk]      DATE           NOT NULL,
    [Hour]        INT            NOT NULL,
    [DeviceId]    INT            NOT NULL,
    [Consumption] DECIMAL (4, 1) NOT NULL,
    CONSTRAINT [PK_PowerConsumption] PRIMARY KEY CLUSTERED ([DateDk] DESC, [DeviceId] ASC, [Hour] ASC)
);

