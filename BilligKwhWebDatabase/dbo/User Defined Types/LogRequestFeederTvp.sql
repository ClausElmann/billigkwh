CREATE TYPE [dbo].[LogRequestFeederTvp] AS TABLE (
    [ID]         BIGINT         NULL,
    [Tidspunkt]  DATETIME       NULL,
    [TicksCall]  INT            NULL,
    [TicksAll]   INT            NULL,
    [UniqueID]   NVARCHAR (50)  NULL,
    [BeskedLbNr] SMALLINT       NULL,
    [Metode]     NVARCHAR (150) NULL,
    [Request]    NVARCHAR (MAX) NULL,
    [Host]       NVARCHAR (50)  NULL,
    [ErrInfo]    NVARCHAR (MAX) NULL,
    [Done]       BIT            NULL);

