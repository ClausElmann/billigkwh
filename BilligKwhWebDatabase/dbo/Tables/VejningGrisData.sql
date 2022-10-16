CREATE TABLE [dbo].[VejningGrisData] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [UniqueID]   NVARCHAR (50)  NOT NULL,
    [VejeholdNr] SMALLINT       NOT NULL,
    [Sekund]     INT            CONSTRAINT [DF_VejningData_Sekund] DEFAULT ((0)) NOT NULL,
    [LbNr]       INT            NOT NULL,
    [KundeID]    INT            CONSTRAINT [DF_VejningData_KundeID] DEFAULT ((0)) NOT NULL,
    [IFVersion]  SMALLINT       NOT NULL,
    [Dato]       DATETIME       CONSTRAINT [DF_VejningData_Dato] DEFAULT (getdate()) NOT NULL,
    [Data]       NVARCHAR (MAX) NOT NULL,
    [Modtaget]   DATETIME       CONSTRAINT [DF_VejningData_Modtaget] DEFAULT (getdate()) NOT NULL,
    [Importeret] BIT            CONSTRAINT [DF_VejningData_Importeret] DEFAULT ((0)) NULL,
    [TimeNrID]   SMALLINT       CONSTRAINT [DF_VejningData_TimeNrID] DEFAULT ((0)) NOT NULL,
    [VejeholdID] INT            CONSTRAINT [DF_VejningData_VejeholdID] DEFAULT ((0)) NOT NULL,
    [DyrID]      INT            CONSTRAINT [DF_VejeholdData_Dyr] DEFAULT ((0)) NOT NULL,
    [Vaegt]      INT            CONSTRAINT [DF_VejningData_Vaegt] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_VejningData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

