﻿CREATE TABLE [dbo].[Schedules] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [Date]           DATE     NOT NULL,
    [DeviceId]       INT      NOT NULL,
    [LastUpdatedUtc] DATETIME NOT NULL,
    [LastReadUtc]    DATETIME NULL,
    [H00]            BIT      NOT NULL,
    [H01]            BIT      NOT NULL,
    [H02]            BIT      NOT NULL,
    [H03]            BIT      NOT NULL,
    [H04]            BIT      NOT NULL,
    [H05]            BIT      NOT NULL,
    [H06]            BIT      NOT NULL,
    [H07]            BIT      NOT NULL,
    [H08]            BIT      NOT NULL,
    [H09]            BIT      NOT NULL,
    [H10]            BIT      NOT NULL,
    [H11]            BIT      NOT NULL,
    [H12]            BIT      NOT NULL,
    [H13]            BIT      NOT NULL,
    [H14]            BIT      NOT NULL,
    [H15]            BIT      NOT NULL,
    [H16]            BIT      NOT NULL,
    [H17]            BIT      NOT NULL,
    [H18]            BIT      NOT NULL,
    [H19]            BIT      NOT NULL,
    [H20]            BIT      NOT NULL,
    [H21]            BIT      NOT NULL,
    [H22]            BIT      NOT NULL,
    [H23]            BIT      NOT NULL,
    CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED ([Id] DESC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Schedules]
    ON [dbo].[Schedules]([Date] DESC, [DeviceId] DESC);

