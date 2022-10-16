CREATE TABLE [dbo].[UserRoleMappings] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [UserId]     INT NOT NULL,
    [UserRoleId] INT NOT NULL,
    [CustomerId] INT NOT NULL,
    CONSTRAINT [PK__Users_In__3214EC078DF87398] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_UserInRoles_UserRoles] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRoles] ([Id]),
    CONSTRAINT [FK_UserInRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
ALTER TABLE [dbo].[UserRoleMappings] NOCHECK CONSTRAINT [FK_UserInRoles_UserRoles];


GO
ALTER TABLE [dbo].[UserRoleMappings] NOCHECK CONSTRAINT [FK_UserInRoles_Users];




GO
ALTER TABLE [dbo].[UserRoleMappings] NOCHECK CONSTRAINT [FK_UserInRoles_UserRoles];


GO


