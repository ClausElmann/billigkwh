CREATE TABLE [dbo].[FareProcessCheck] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [ProcessID]             INT            NOT NULL,
    [SkabelonCheckID]       INT            NOT NULL,
    [LbNr]                  SMALLINT       NOT NULL,
    [Kommentar]             NVARCHAR (MAX) CONSTRAINT [DF_ProcessCheck_Beskrivelse] DEFAULT ('') NOT NULL,
    [Checked]               BIT            NULL,
    [CheckedAfBrugerID]     INT            NULL,
    [CheckedTid]            DATETIME       NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_ProcessCheck_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_ProcessCheck_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_ProcessCheck_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcessCheck] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcessCheck_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_ProcessCheck_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_ProcessCheck_Process] FOREIGN KEY ([ProcessID]) REFERENCES [dbo].[FareProcess] ([ID]),
    CONSTRAINT [FK_ProcessCheck_ProcessSkabelonCheck] FOREIGN KEY ([SkabelonCheckID]) REFERENCES [dbo].[FareProcessSkabelonCheck] ([ID])
);

