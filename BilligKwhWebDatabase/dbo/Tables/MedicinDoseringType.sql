CREATE TABLE [dbo].[MedicinDoseringType] (
    [ID]                SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Navn]              NVARCHAR (50) CONSTRAINT [DF_MedicinDoseringType_Navn] DEFAULT ('') NOT NULL,
    [Label]             NVARCHAR (6)  CONSTRAINT [DF_MedicinDoseringType_Navn1] DEFAULT ('') NOT NULL,
    [VisDosePrEnhed]    BIT           CONSTRAINT [DF_MedicinDoseringType_VisDosePrEnhed] DEFAULT ((0)) NOT NULL,
    [VisPrEnhedMaengde] BIT           CONSTRAINT [DF_MedicinDoseringType_VisPrEnhedMaengde] DEFAULT ((0)) NOT NULL,
    [SidstRettet]       DATETIME      CONSTRAINT [DF_MedicinDoseringType_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]           BIT           CONSTRAINT [DF_MedicinDoseringType_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MedicinDose] PRIMARY KEY CLUSTERED ([ID] ASC)
);

