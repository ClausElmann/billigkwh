CREATE TABLE [dbo].[ApplicationSettings] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationSettingTypeId] INT            NOT NULL,
    [ApplicationSettingName]   NVARCHAR (50)  NULL,
    [Setting]                  NVARCHAR (MAX) NOT NULL,
    [DateLastUpdatedUtc]       DATETIME       CONSTRAINT [DF_ApplicationSettings_DateLastEditUtc] DEFAULT (getutcdate()) NOT NULL,
    [Description]              NVARCHAR (100) NULL,
    CONSTRAINT [PK_ApplicationSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);



