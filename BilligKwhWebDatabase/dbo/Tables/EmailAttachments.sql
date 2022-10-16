CREATE TABLE [dbo].[EmailAttachments] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [MessageId]      INT             NOT NULL,
    [FileContent]    VARBINARY (MAX) NOT NULL,
    [FileName]       NVARCHAR (255)  NOT NULL,
    [DateCreatedUtc] DATETIME        CONSTRAINT [DF__EmailAtta__DateC__29BAC7A7] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK__EmailAtt__3214EC070F185261] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Email_Attachments_Messages] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[EmailMessages] ([Id])
);

