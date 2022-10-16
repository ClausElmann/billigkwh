CREATE TABLE [dbo].[RettighedType] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Navn] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RettighedsType] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

