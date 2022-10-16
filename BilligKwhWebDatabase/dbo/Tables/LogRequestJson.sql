CREATE TABLE [dbo].[LogRequestJson] (
    [ID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Tidspunkt] DATETIME       NOT NULL,
    [TicksCall] INT            CONSTRAINT [DF_LogRequestJson_Ticks] DEFAULT ((0)) NOT NULL,
    [TicksAll]  INT            CONSTRAINT [DF_LogRequestJson_Ticks1] DEFAULT ((0)) NOT NULL,
    [Metode]    NVARCHAR (150) CONSTRAINT [DF_LogRequestJson_Metode] DEFAULT ('') NOT NULL,
    [Request]   NVARCHAR (MAX) CONSTRAINT [DF_LogRequestJson_Request] DEFAULT ('') NOT NULL,
    [ErrInfo]   NVARCHAR (MAX) CONSTRAINT [DF_LogRequestJson_ErrInfo] DEFAULT ('') NOT NULL,
    [Done]      BIT            NULL,
    CONSTRAINT [PK_LogRequestJson] PRIMARY KEY CLUSTERED ([ID] ASC)
);

