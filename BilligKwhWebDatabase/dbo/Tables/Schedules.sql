CREATE TABLE [dbo].[Schedules] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [Date]           DATE     NOT NULL,
    [DeviceId]       INT      NOT NULL,
    [LastUpdatedUtc] DATETIME NOT NULL,
    [LastReadUtc]    DATETIME NULL,
    [H00]            INT      NOT NULL,
    [H01]            INT      NOT NULL,
    [H02]            INT      NOT NULL,
    [H03]            INT      NOT NULL,
    [H04]            INT      NOT NULL,
    [H05]            INT      NOT NULL,
    [H06]            INT      NOT NULL,
    [H07]            INT      NOT NULL,
    [H08]            INT      NOT NULL,
    [H09]            INT      NOT NULL,
    [H10]            INT      NOT NULL,
    [H11]            INT      NOT NULL,
    [H12]            INT      NOT NULL,
    [H13]            INT      NOT NULL,
    [H14]            INT      NOT NULL,
    [H15]            INT      NOT NULL,
    [H16]            INT      NOT NULL,
    [H17]            INT      NOT NULL,
    [H18]            INT      NOT NULL,
    [H19]            INT      NOT NULL,
    [H20]            INT      NOT NULL,
    [H21]            INT      NOT NULL,
    [H22]            INT      NOT NULL,
    [H23]            INT      NOT NULL,
    CONSTRAINT [PK_ScheduleNew] PRIMARY KEY CLUSTERED ([Id] DESC)
);



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SchedulesNew]
    ON [dbo].[Schedules]([Date] DESC, [DeviceId] DESC);

