CREATE TABLE [dbo].[LanguageHelpCode] (
    [ID]               INT       IDENTITY (1, 1) NOT NULL,
    [LanguageHelpID]   INT       NOT NULL,
    [Code]             NCHAR (8) NOT NULL,
    [Sort]             INT       NOT NULL,
    [NorwegianIsDirty] BIT       CONSTRAINT [DF_Table_1_Norsk] DEFAULT ((1)) NOT NULL,
    [SwedishIsDirty]   BIT       CONSTRAINT [DF_Table_1_Svensk] DEFAULT ((1)) NOT NULL,
    [EnglishIsDirty]   BIT       CONSTRAINT [DF_Table_1_Engelsk] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_LanguageHelpKodeDirty] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LanguageHelpKode_LanguageHelp] FOREIGN KEY ([LanguageHelpID]) REFERENCES [dbo].[LanguageHelp] ([ID])
);

