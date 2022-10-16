CREATE TABLE [dbo].[HaendelseType] (
    [ID]              INT           NOT NULL,
    [Navn]            NVARCHAR (50) NOT NULL,
    [Label]           NVARCHAR (6)  CONSTRAINT [DF_EventType_Label] DEFAULT ('') NOT NULL,
    [AarsagListeID]   INT           CONSTRAINT [DF_EventType_ListeID] DEFAULT ((0)) NOT NULL,
    [VisFjernetVaegt] BIT           CONSTRAINT [DF_EventType_VisFjernetGaltgris1] DEFAULT ((0)) NOT NULL,
    [VisFjernetAntal] BIT           CONSTRAINT [DF_EventType_VisFjernetGaltgris1_1] DEFAULT ((0)) NOT NULL,
    [VisMaengde]      BIT           CONSTRAINT [DF_EventType_RequireVaegtAntal2] DEFAULT ((0)) NOT NULL,
    [SidstRettet]     DATETIME      CONSTRAINT [DF_EventType_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]         BIT           CONSTRAINT [DF_EventType_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

