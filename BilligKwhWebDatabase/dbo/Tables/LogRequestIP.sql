CREATE TABLE [dbo].[LogRequestIP] (
    [ID]                   INT           IDENTITY (1, 1) NOT NULL,
    [RemoteIP]             NVARCHAR (50) NOT NULL,
    [SidstBrugtAfBrugerID] INT           CONSTRAINT [DF_LogRequestIP_SidstBrugtAfBrugerID] DEFAULT ((0)) NOT NULL,
    [SidstBrugt]           DATETIME      CONSTRAINT [DF_LogRequestIP_SidstBrugt] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LogRequestIP] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_LogRequestIP_SidstBrugtAfBruger] FOREIGN KEY ([SidstBrugtAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);

