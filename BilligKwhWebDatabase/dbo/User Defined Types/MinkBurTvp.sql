CREATE TYPE [dbo].[MinkBurTvp] AS TABLE (
    [ID]                    INT            NULL,
    [KundeID]               INT            NULL,
    [RaekkeID]              INT            NULL,
    [BurNr]                 SMALLINT       NULL,
    [Slettet]               BIT            NULL,
    [Beskrivelse]           NVARCHAR (MAX) NULL,
    [SidstRettet]           DATETIME       NULL,
    [SidstRettetAfBrugerID] INT            NULL);

