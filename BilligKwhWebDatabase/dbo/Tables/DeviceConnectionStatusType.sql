CREATE TABLE [dbo].[DeviceConnectionStatusType] (
    [ID]          SMALLINT      NOT NULL,
    [Navn]        NVARCHAR (50) NOT NULL,
    [Label]       NVARCHAR (6)  CONSTRAINT [DF_DeviceConnectionStatusType_Label] DEFAULT ('') NOT NULL,
    [SidstRettet] DATETIME      CONSTRAINT [DF_ConnectionStatusType_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [Slettet]     BIT           CONSTRAINT [DF_ConnectionStatusType_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ConnectionStatusType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

