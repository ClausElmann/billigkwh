CREATE TABLE [dbo].[BroendMaaler] (
    [ID]                    NVARCHAR (25)  CONSTRAINT [DF_BroendMaaler_DeviceUid] DEFAULT ('') NOT NULL,
    [ServerID]              INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_BroendMaaler_Beskrivelse] DEFAULT ('') NOT NULL,
    [GensendFraID]          INT            CONSTRAINT [DF_BroendMaaler_HentFraID] DEFAULT ((0)) NULL,
    [OpsaetningData]        NVARCHAR (MAX) CONSTRAINT [DF_BroendMaaler_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]    DATETIME       CONSTRAINT [DF_BroendMaaler_SidstRettet1] DEFAULT (getdate()) NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_BroendMaaler_OrdreNr] DEFAULT ((0)) NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_BroendMaaler_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_BroendMaaler_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_BroendMaaler_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BroendMaaler] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BroendMaaler_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_BroendMaaler_Device] FOREIGN KEY ([ID]) REFERENCES [dbo].[Device] ([DeviceID]),
    CONSTRAINT [FK_BroendMaaler_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BroendMaaler]
    ON [dbo].[BroendMaaler]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_BroendMaaler]
    ON [dbo].[BroendMaaler]([KundeID] ASC, [ID] ASC);

