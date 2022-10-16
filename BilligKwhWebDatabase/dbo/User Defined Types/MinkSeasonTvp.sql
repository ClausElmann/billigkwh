CREATE TYPE [dbo].[MinkSeasonTvp] AS TABLE (
    [ID]                    INT      NULL,
    [BedriftID]             INT      NULL,
    [SeasonAarID]           INT      NULL,
    [Forstedag]             DATETIME NULL,
    [SidstRettet]           DATETIME NULL,
    [SidstRettetAfBrugerID] INT      NULL,
    [Sidstedag]             DATETIME NULL);

