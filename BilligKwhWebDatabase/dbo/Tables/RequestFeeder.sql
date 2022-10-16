CREATE TABLE [dbo].[RequestFeeder] (
    [ID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Tidspunkt] DATETIME       NOT NULL,
    [PathClass] NVARCHAR (150) CONSTRAINT [DF_RequestFeeder_PathClass] DEFAULT ('') NOT NULL,
    [Metode]    NVARCHAR (150) CONSTRAINT [DF_RequestFeeder_Metode] DEFAULT ('') NOT NULL,
    [Request]   NVARCHAR (MAX) CONSTRAINT [DF_RequestFeeder_Request] DEFAULT ('') NOT NULL,
    [ErrInfo]   NVARCHAR (MAX) CONSTRAINT [DF_RequestFeeder_ErrInfo] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_RequestFeeder] PRIMARY KEY CLUSTERED ([ID] ASC)
);

