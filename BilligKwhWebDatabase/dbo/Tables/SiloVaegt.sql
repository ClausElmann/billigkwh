CREATE TABLE [dbo].[SiloVaegt] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [DeviceID]              NVARCHAR (25) CONSTRAINT [DF_SiloVaegt_DeviceUid] DEFAULT ('') NOT NULL,
    [KundeID]               INT           NOT NULL,
    [BedriftID]             INT           CONSTRAINT [DF_SiloVaegt_BedriftID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME      CONSTRAINT [DF_SiloVaegt_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT           CONSTRAINT [DF_SiloVaegt_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT           CONSTRAINT [DF_SiloVaegt_Slettet] DEFAULT ((0)) NOT NULL,
    [SidstOnline]           DATETIME      NULL,
    [Overvaagning]          BIT           CONSTRAINT [DF_SiloVaegt_Overvaagning] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SiloVaegt] PRIMARY KEY NONCLUSTERED ([DeviceID] ASC),
    CONSTRAINT [FK_SiloVaegt_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_SiloVaegt_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_SiloVaegt_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID]),
    CONSTRAINT [FK_SiloVaegt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_SiloVaegt]
    ON [dbo].[SiloVaegt]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_SiloVaegt]
    ON [dbo].[SiloVaegt]([KundeID] ASC, [DeviceID] ASC);

