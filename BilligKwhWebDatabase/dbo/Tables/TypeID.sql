CREATE TABLE [dbo].[TypeID] (
    [ID]         SMALLINT      IDENTITY (1, 1) NOT NULL,
    [TypeTypeID] SMALLINT      CONSTRAINT [DF_Type_TypeID] DEFAULT ((0)) NOT NULL,
    [Navn]       NVARCHAR (50) CONSTRAINT [DF_Type_Navn] DEFAULT ('') NOT NULL,
    [Label]      NVARCHAR (6)  CONSTRAINT [DF_Type_Label] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Type_TypeType] FOREIGN KEY ([TypeTypeID]) REFERENCES [dbo].[TypeType] ([ID])
);

