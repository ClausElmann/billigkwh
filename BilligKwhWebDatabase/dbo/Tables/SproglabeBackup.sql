CREATE TABLE [dbo].[SproglabeBackup] (
    [ID]                           INT             NOT NULL,
    [TypeID]                       INT             NOT NULL,
    [Bemærkning]                   NVARCHAR (250)  COLLATE SQL_Danish_Pref_CP1_CI_AS NOT NULL,
    [Tekst]                        NVARCHAR (600)  COLLATE SQL_Danish_Pref_CP1_CI_AS NOT NULL,
    [Norsk]                        NVARCHAR (600)  COLLATE SQL_Danish_Pref_CP1_CI_AS NULL,
    [Svensk]                       NVARCHAR (600)  COLLATE SQL_Danish_Pref_CP1_CI_AS NULL,
    [Engelsk]                      NVARCHAR (600)  COLLATE SQL_Danish_Pref_CP1_CI_AS NULL,
    [DanskSidstRettetAfBrugerID]   INT             NULL,
    [NorskSidstRettetAfBrugerID]   INT             NULL,
    [SvenskSidstRettetAfBrugerID]  INT             NULL,
    [EngelskSidstRettetAfBrugerID] INT             NULL,
    [Moduler]                      NVARCHAR (1000) COLLATE SQL_Danish_Pref_CP1_CI_AS NULL,
    [DanskSidstRettetUtc]          DATETIME        NULL,
    [NorskSidstRettetUtc]          DATETIME        NULL,
    [SvenskSidstRettetUtc]         DATETIME        NULL,
    [EngelskSidstRettetUtc]        DATETIME        NULL
);

