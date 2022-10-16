CREATE TABLE [dbo].[UserRefreshTokens] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UserId]         INT            NOT NULL,
    [Token]          NVARCHAR (256) NOT NULL,
    [DateCreatedUtc] DATETIME       CONSTRAINT [DF_UserRefreshTokens_Created] DEFAULT (getutcdate()) NOT NULL,
    [DateExpiresUtc] DATETIME       NOT NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_UserRefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
ALTER TABLE [dbo].[UserRefreshTokens] NOCHECK CONSTRAINT [FK_UserRefreshTokens_Users];




GO





GO


