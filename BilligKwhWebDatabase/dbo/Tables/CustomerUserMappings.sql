CREATE TABLE [dbo].[CustomerUserMappings] (
    [Id]                 INT      IDENTITY (1, 1) NOT NULL,
    [CustomerId]         INT      NOT NULL,
    [UserId]             INT      NOT NULL,
    [DateCreatedUtc]     DATETIME CONSTRAINT [DF_CustomerUserMappings_DateCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [DateLastUpdatedUtc] DATETIME DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_CustomerUserMappings] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_CustomerUserMappings_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
);


GO
ALTER TABLE [dbo].[CustomerUserMappings] NOCHECK CONSTRAINT [FK_CustomerUserMappings_Customers];




GO
ALTER TABLE [dbo].[CustomerUserMappings] NOCHECK CONSTRAINT [FK_CustomerUserMappings_Customers];


GO





GO



GO





GO



GO


