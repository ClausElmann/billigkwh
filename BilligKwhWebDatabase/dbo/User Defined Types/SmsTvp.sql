CREATE TYPE [dbo].[SmsTvp] AS TABLE (
    [ID]                 BIGINT         NULL,
    [KundeID]            INT            NULL,
    [ModtagerNr]         CHAR (8)       NULL,
    [ModtagerAdresse]    NVARCHAR (60)  NULL,
    [Besked]             NVARCHAR (MAX) NULL,
    [Oprettet]           DATETIME       NULL,
    [OprettetAfBrugerID] INT            NULL,
    [Sendt]              DATETIME       NULL,
    [ErrorLog]           NVARCHAR (MAX) NULL,
    [SendTidligst]       DATETIME       NULL,
    [AfsenderNavn]       NVARCHAR (11)  NULL);

