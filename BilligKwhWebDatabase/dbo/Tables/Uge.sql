CREATE TABLE [dbo].[Uge] (
    [Dato]  DATE NOT NULL,
    [UgeID] INT  NOT NULL,
    [UgeNr] INT  NOT NULL,
    [Aar]   INT  NOT NULL,
    CONSTRAINT [PK_Week] PRIMARY KEY CLUSTERED ([Dato] ASC)
);

