CREATE TABLE [dbo].[HoldType] (
    [ID]          SMALLINT      NOT NULL,
    [Navn]        NVARCHAR (50) NOT NULL,
    [Label]       NVARCHAR (6)  NOT NULL,
    [ListePostID] INT           NULL,
    CONSTRAINT [PK_HoldType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

