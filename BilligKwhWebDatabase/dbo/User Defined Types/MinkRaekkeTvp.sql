CREATE TYPE [dbo].[MinkRaekkeTvp] AS TABLE (
    [ID]                    INT            NULL,
    [KundeID]               INT            NULL,
    [HusID]                 INT            NULL,
    [RaekkeNr]              SMALLINT       NULL,
    [StartBur]              SMALLINT       NULL,
    [Slutbur]               SMALLINT       NULL,
    [AntalBure]             SMALLINT       NULL,
    [Beskrivelse]           NVARCHAR (MAX) NULL,
    [SidstRettet]           DATETIME       NULL,
    [SidstRettetAfBrugerID] INT            NULL,
    [Slettet]               BIT            NULL);

