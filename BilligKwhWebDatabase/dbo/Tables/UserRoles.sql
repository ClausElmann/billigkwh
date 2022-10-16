CREATE TABLE [dbo].[UserRoles] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (256) NOT NULL,
    [Description]        NCHAR (256)    NULL,
    [IsSuperAdmin]       BIT            CONSTRAINT [DF__UserRoles__IsSup__6609FC90] DEFAULT ((0)) NOT NULL,
    [UserRoleCategoryId] INT            CONSTRAINT [DF_UserRoles_UserRoleCategoryId] DEFAULT ((0)) NOT NULL,
    [DateCreatedUtc]     DATETIME       CONSTRAINT [DF__UserRoles__DateC__67F24502] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK__User_Rol__3214EC0779DF729F] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON)
);



