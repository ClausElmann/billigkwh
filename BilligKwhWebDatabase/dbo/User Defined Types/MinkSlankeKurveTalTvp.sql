CREATE TYPE [dbo].[MinkSlankeKurveTalTvp] AS TABLE (
    [ID]                    INT            NULL,
    [SlankeKurveID]         INT            NULL,
    [Dag]                   INT            NULL,
    [UgeDag]                SMALLINT       NULL,
    [Uge]                   SMALLINT       NULL,
    [Procent]               DECIMAL (7, 4) NULL,
    [SidstRettet]           DATETIME       NULL,
    [SidstRettetAfBrugerID] INT            NULL,
    [Slettet]               BIT            NULL);

