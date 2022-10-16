CREATE TABLE [dbo].[LogRequestFeeder] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Tidspunkt]  DATETIME       NOT NULL,
    [TicksCall]  INT            CONSTRAINT [DF_LogRequestFeeder_Ticks] DEFAULT ((0)) NOT NULL,
    [TicksAll]   INT            CONSTRAINT [DF_LogRequestFeeder_Ticks1] DEFAULT ((0)) NOT NULL,
    [UniqueID]   NVARCHAR (50)  CONSTRAINT [DF_LogRequestFeeder_UniqueID] DEFAULT ('') NOT NULL,
    [BeskedLbNr] SMALLINT       CONSTRAINT [DF_LogRequestFeeder_MessageID] DEFAULT ((0)) NOT NULL,
    [Metode]     NVARCHAR (150) CONSTRAINT [DF_LogRequestFeeder_Metode] DEFAULT ('') NOT NULL,
    [Request]    NVARCHAR (MAX) CONSTRAINT [DF_LogRequestFeeder_Request] DEFAULT ('') NOT NULL,
    [Host]       NVARCHAR (50)  CONSTRAINT [DF_LogRequestFeeder_UniqueID1] DEFAULT ('') NOT NULL,
    [ErrInfo]    NVARCHAR (MAX) CONSTRAINT [DF_LogRequestFeeder_ErrInfo] DEFAULT ('') NOT NULL,
    [Done]       BIT            NULL,
    CONSTRAINT [PK_LogRequestFeeder] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [missing_index_225060_225059_LogRequestFeeder]
    ON [dbo].[LogRequestFeeder]([Metode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LogRequestFeeder]
    ON [dbo].[LogRequestFeeder]([UniqueID] ASC);

