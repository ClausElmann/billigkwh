CREATE TABLE [dbo].[SensitivePageLoads] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [IP]              NVARCHAR (50)  NOT NULL,
    [PageNameId]      NVARCHAR (150) NOT NULL,
    [LoadDateTimeUtc] DATETIME       CONSTRAINT [DF_Captcha_LoadDateTimeUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Captcha] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_SensitivePageLoads_IP]
    ON [dbo].[SensitivePageLoads]([IP] ASC)
    INCLUDE([PageNameId], [LoadDateTimeUtc]);

