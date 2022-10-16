CREATE TABLE [dbo].[PinCodes] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [DateCreatedUtc]  DATETIME       CONSTRAINT [DF_PinCodes_DateCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PhoneNumber]     BIGINT         NULL,
    [Email]           NVARCHAR (100) NULL,
    [SaltedPinSHA256] NCHAR (64)     NOT NULL,
    [Salt]            NVARCHAR (24)  NOT NULL,
    CONSTRAINT [PK_PinCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

