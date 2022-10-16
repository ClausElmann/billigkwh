CREATE TYPE [dbo].[LogRequestJsonTvp] AS TABLE (
    [ID]        BIGINT         NULL,
    [Tidspunkt] DATETIME       NULL,
    [TicksCall] INT            NULL,
    [TicksAll]  INT            NULL,
    [Metode]    NVARCHAR (150) NULL,
    [Request]   NVARCHAR (MAX) NULL,
    [ErrInfo]   NVARCHAR (MAX) NULL,
    [Done]      BIT            NULL);

