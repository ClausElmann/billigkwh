CREATE TABLE [dbo].[LogRequestPage] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Page] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_RequestPages] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

