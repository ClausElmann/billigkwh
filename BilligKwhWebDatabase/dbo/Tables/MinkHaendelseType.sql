CREATE TABLE [dbo].[MinkHaendelseType] (
    [ID]            INT           NOT NULL,
    [Navn]          NVARCHAR (50) NOT NULL,
    [Label]         NVARCHAR (6)  CONSTRAINT [DF_HaendelseType_Label] DEFAULT ('') NOT NULL,
    [AarsagListeID] INT           CONSTRAINT [DF_HaendelseType_ListeID] DEFAULT ((0)) NOT NULL,
    [Faktor]        INT           CONSTRAINT [DF_MinkHaendelseType_Faktor] DEFAULT ((1)) NOT NULL,
    [ErBeholdning]  BIT           CONSTRAINT [DF_MinkHaendelseType_Erbeholdning] DEFAULT ((0)) NOT NULL,
    [SidstRettet]   DATETIME      CONSTRAINT [DF_HaendelseType_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]       BIT           CONSTRAINT [DF_HaendelseType_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkHaendelseType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

