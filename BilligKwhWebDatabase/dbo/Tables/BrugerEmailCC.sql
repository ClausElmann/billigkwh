CREATE TABLE [dbo].[BrugerEmailCC] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [BrugerID] INT            NOT NULL,
    [Email]    NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_BrugerCCEmail] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BrugerEmailCC_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);

