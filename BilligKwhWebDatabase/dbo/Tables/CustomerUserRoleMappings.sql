CREATE TABLE [dbo].[CustomerUserRoleMappings] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [CustomerId]      INT      NOT NULL,
    [UserRoleId]      INT      NOT NULL,
    [DateCreatedUtc]  DATETIME CONSTRAINT [DF_CustomerUserRoleMappings_DateCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [DefaultSelected] BIT      CONSTRAINT [DF_CustomerUserRoleMappings_DefaultSelected] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CustomerUserRoleMappings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

