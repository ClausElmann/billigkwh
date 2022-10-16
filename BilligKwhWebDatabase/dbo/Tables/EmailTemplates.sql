CREATE TABLE [dbo].[EmailTemplates] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [LanguageId]     INT            NOT NULL,
    [Name]           NVARCHAR (250) NOT NULL,
    [Subject]        NVARCHAR (500) NOT NULL,
    [Html]           NVARCHAR (MAX) NOT NULL,
    [DateCreatedUtc] DATETIME       CONSTRAINT [DF_EmailTemplates_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [FromEmail]      NVARCHAR (500) NULL,
    [FromName]       NVARCHAR (500) NULL,
    [ReplyTo]        NVARCHAR (100) NULL,
    [BccEmails]      NVARCHAR (500) NULL,
    [UseHtmlFromDb]  BIT            NULL,
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([Id] ASC)
);



