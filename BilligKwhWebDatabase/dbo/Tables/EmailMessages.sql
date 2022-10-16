CREATE TABLE [dbo].[EmailMessages] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]     INT            CONSTRAINT [DF_EmailMessages_CustomerId] DEFAULT ((0)) NOT NULL,
    [RefTypeID]      INT               NULL,
    [RefID]          INT               NULL,
    [CategoryId]     INT            CONSTRAINT [DF__EmailMess__CateG__22D8BFEE] DEFAULT ((0)) NOT NULL,
    [Subject]        NVARCHAR (200) NOT NULL,
    [FromName]       NVARCHAR (200) NULL,
    [FromEmail]      NVARCHAR (500) NOT NULL,
    [ToEmail]        NVARCHAR (200) NULL,
    [ToName]         NVARCHAR (200) NULL,
    [DateCreatedUtc] DATETIME       CONSTRAINT [DF__tmp_ms_xx__Creat__793CA210] DEFAULT (getutcdate()) NOT NULL,
    [DateSentUtc]    DATETIME       NULL,
    [HasAttachments] BIT            CONSTRAINT [DF_EmailMessagesNew_HasAttachments] DEFAULT ((0)) NOT NULL,
    [BccEmails]      NVARCHAR (500) NULL,
    [UseBcc]         BIT            CONSTRAINT [DF__EmailMess__UseBc__21E49BB5] DEFAULT ((1)) NOT NULL,
    [ReplyTo]        NVARCHAR (200) NOT NULL,
    [Body]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__EmailMess__3214EC07A07B5Z6B] PRIMARY KEY CLUSTERED ([Id] ASC)
);



