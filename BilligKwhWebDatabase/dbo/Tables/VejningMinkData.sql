CREATE TABLE [dbo].[VejningMinkData] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [UniqueID]   NVARCHAR (50)  NOT NULL,
    [VejeholdNr] SMALLINT       NOT NULL,
    [Sekund]     INT            CONSTRAINT [DF_VejningMinkData_Sekund] DEFAULT ((0)) NOT NULL,
    [LbNr]       INT            NOT NULL,
    [KundeID]    INT            CONSTRAINT [DF_VejningMinkData_KundeID] DEFAULT ((0)) NOT NULL,
    [IFVersion]  SMALLINT       NOT NULL,
    [Dato]       DATETIME       CONSTRAINT [DF_VejningMinkData_Dato] DEFAULT (getdate()) NOT NULL,
    [Data]       NVARCHAR (MAX) NOT NULL,
    [Modtaget]   DATETIME       CONSTRAINT [DF_VejningMinkData_Modtaget] DEFAULT (getdate()) NOT NULL,
    [Importeret] BIT            CONSTRAINT [DF_VejningMinkData_Importeret] DEFAULT ((0)) NULL,
    [TimeNrID]   SMALLINT       CONSTRAINT [DF_VejningMinkData_TimeNrID] DEFAULT ((0)) NOT NULL,
    [VejeholdID] INT            CONSTRAINT [DF_VejningMinkData_VejeholdID] DEFAULT ((0)) NOT NULL,
    [Vaegt]      INT            CONSTRAINT [DF_VejningMinkData_Vaegt] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_VejningMinkData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

