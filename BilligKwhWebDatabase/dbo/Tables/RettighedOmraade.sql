CREATE TABLE [dbo].[RettighedOmraade] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [Omraade] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RettighedOmraade] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

