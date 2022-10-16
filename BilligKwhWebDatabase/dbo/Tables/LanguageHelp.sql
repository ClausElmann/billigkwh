CREATE TABLE [dbo].[LanguageHelp] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [Danish]              NVARCHAR (MAX) NOT NULL,
    [Norwegian]           NVARCHAR (MAX) NULL,
    [Swedish]             NVARCHAR (MAX) NULL,
    [English]             NVARCHAR (MAX) NULL,
    [Note]                NVARCHAR (MAX) CONSTRAINT [DF_Table_1_Bemærkning] DEFAULT ('') NOT NULL,
    [DanishLastEditedUtc] DATETIME       NOT NULL,
    [IsDirty]             BIT            NOT NULL,
    [Approved]            BIT            CONSTRAINT [DF_LanguageHelp_Approved] DEFAULT ((0)) NOT NULL,
    [German]              NVARCHAR (MAX) NULL,
    [Greek]               NVARCHAR (MAX) NULL,
    [Chinese]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SprogHjaelp] PRIMARY KEY CLUSTERED ([ID] ASC)
);

