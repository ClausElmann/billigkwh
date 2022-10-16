CREATE TABLE [dbo].[DeviceType] (
    [ID]                    SMALLINT       NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Label]                 NVARCHAR (6)   CONSTRAINT [DF_DeviceType_Label] DEFAULT ('') NOT NULL,
    [Kode]                  NCHAR (2)      CONSTRAINT [DF_DeviceType_Kode] DEFAULT ('dd') NOT NULL,
    [Konfiguration]         NVARCHAR (MAX) CONSTRAINT [DF_DeviceType_Configuration] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_DeviceType_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_DeviceType_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_DeviceType_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DeviceType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

