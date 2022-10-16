CREATE TABLE [dbo].[DyreGruppeType] (
    [ID]          SMALLINT      NOT NULL,
    [Navn]        NVARCHAR (50) NOT NULL,
    [Label]       NVARCHAR (6)  NOT NULL,
    [ListepostID] INT           NULL,
    CONSTRAINT [PK_DyregruppeType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

