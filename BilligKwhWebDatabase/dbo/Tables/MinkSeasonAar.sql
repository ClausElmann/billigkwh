CREATE TABLE [dbo].[MinkSeasonAar] (
    [ID]       INT           NOT NULL,
    [AarNavn]  NVARCHAR (50) NOT NULL,
    [Aar]      INT           NOT NULL,
    [ErVinter] BIT           NOT NULL,
    CONSTRAINT [PK_MinkSeasonAar] PRIMARY KEY CLUSTERED ([ID] ASC)
);

