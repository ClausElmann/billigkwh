CREATE TABLE [dbo].[SkabelonAnvendt] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [SkabelonID]            INT            NOT NULL,
    [Label]                 NVARCHAR (250) NOT NULL,
    [Sortering]             INT            CONSTRAINT [DF_SkabelonAnvendt_Sortering] DEFAULT ((1000)) NOT NULL,
    [Anvendt]               BIT            CONSTRAINT [DF_SkabelonAnvendt_Anvendt] DEFAULT ((1)) NOT NULL,
    [Modul]                 INT            CONSTRAINT [DF_SkabelonAnvendt_Modul] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       NOT NULL,
    [SidstRettetAfBrugerID] INT            NOT NULL,
    CONSTRAINT [PK_SkabelonAnvendt] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SkabelonAnvendt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_SkabelonAnvendt_Skabelon] FOREIGN KEY ([SkabelonID]) REFERENCES [dbo].[Skabelon] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Se DomainModel.Util.Modul', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SkabelonAnvendt', @level2type = N'COLUMN', @level2name = N'Modul';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fortæller om skabelonen skal vises i dropdownbox for valg af skabelon til export. Kunde setting.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SkabelonAnvendt', @level2type = N'COLUMN', @level2name = N'Anvendt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Giver mulighed for at sortere visningsrækkefølge på posterne.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SkabelonAnvendt', @level2type = N'COLUMN', @level2name = N'Sortering';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Label anvendes til at kunden kan give skabelonen eget navn. Vises i stedet for Navn fra Skabelon.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SkabelonAnvendt', @level2type = N'COLUMN', @level2name = N'Label';

