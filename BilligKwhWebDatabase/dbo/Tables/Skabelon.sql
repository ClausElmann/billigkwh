CREATE TABLE [dbo].[Skabelon] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT             NOT NULL,
    [Navn]                  NVARCHAR (250)  NOT NULL,
    [DataTypeID]            INT             NOT NULL,
    [Fil]                   VARBINARY (MAX) NOT NULL,
    [Slettet]               BIT             CONSTRAINT [DF_Skabelon_Slettet] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME        NOT NULL,
    [SidstRettetAfBrugerID] INT             NOT NULL,
    [CSV]                   BIT             CONSTRAINT [DF_Skabelon_CSV] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Skabelon] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Skabelon_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fil data', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Skabelon', @level2type = N'COLUMN', @level2name = N'Fil';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Angiver hvilke type af data skabelonen indeholder. f.eks (Tank, Tømning, job osv). Enum i Util.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Skabelon', @level2type = N'COLUMN', @level2name = N'DataTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Navn på skabelonen', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Skabelon', @level2type = N'COLUMN', @level2name = N'Navn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Skabelon', @level2type = N'COLUMN', @level2name = N'ID';

