CREATE TABLE [dbo].[LocaleStringResources] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [LanguageId]    INT            NOT NULL,
    [ResourceName]  NVARCHAR (200) NOT NULL,
    [ResourceValue] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_LocaleStringResources] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_LocaleStringResources_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ([Id])
);


GO
ALTER TABLE [dbo].[LocaleStringResources] NOCHECK CONSTRAINT [FK_LocaleStringResources_Languages];


GO
CREATE NONCLUSTERED INDEX [ix_LocaleStringResources_ResourceName_LanguageId]
    ON [dbo].[LocaleStringResources]([ResourceName] ASC, [LanguageId] ASC)
    INCLUDE([ResourceValue]);


GO
CREATE NONCLUSTERED INDEX [ix_LocaleStringResources_LanguageId_ResourceName]
    ON [dbo].[LocaleStringResources]([LanguageId] ASC, [ResourceName] ASC)
    INCLUDE([ResourceValue]);

