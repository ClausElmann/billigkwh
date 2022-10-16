CREATE TABLE [dbo].[EmailSkabelon] (
    [ID]      INT            NOT NULL,
    [Emne]    NVARCHAR (200) NOT NULL,
    [Indhold] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_EmailSkabelon] PRIMARY KEY CLUSTERED ([ID] ASC)
);

