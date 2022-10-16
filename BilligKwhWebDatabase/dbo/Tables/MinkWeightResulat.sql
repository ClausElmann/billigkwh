CREATE TABLE [dbo].[MinkWeightResulat] (
    [Dato]      DATE CONSTRAINT [DF_MinkWeightResulat_Dato] DEFAULT (getdate()) NOT NULL,
    [Han1]      INT  NULL,
    [Hun1]      INT  NULL,
    [Han2]      INT  NULL,
    [Hun2]      INT  NULL,
    [Han1Vejet] INT  NULL,
    [Hun1Vejet] INT  NULL,
    [Han2Vejet] INT  NULL,
    [Hun2Vejet] INT  NULL,
    CONSTRAINT [PK_MinkWeightResulat] PRIMARY KEY CLUSTERED ([Dato] ASC)
);

