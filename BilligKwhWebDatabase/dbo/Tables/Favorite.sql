CREATE TABLE [dbo].[Favorite] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]  INT            NOT NULL,
    [BrugerID] INT            NOT NULL,
    [WindowID] INT            NOT NULL,
    [Navn]     NVARCHAR (50)  NOT NULL,
    [JsonData] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Favorite] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Favorite_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Favorite_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Favorite]
    ON [dbo].[Favorite]([KundeID] ASC, [WindowID] ASC, [BrugerID] ASC);

