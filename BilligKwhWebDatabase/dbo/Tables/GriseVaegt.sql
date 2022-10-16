CREATE TABLE [dbo].[GriseVaegt] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [VaegtNr]               INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [GatewayID]             NVARCHAR (25)  CONSTRAINT [DF_GriseVaegt_BoxID] DEFAULT ((0)) NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_GriseVaegt_Beskrivelse] DEFAULT ('') NOT NULL,
    [StiID]                 INT            CONSTRAINT [DF_GriseVaegt_Sti1ID] DEFAULT ((0)) NOT NULL,
    [GensendFraID]          INT            CONSTRAINT [DF_GriseVaegt_HentFraID] DEFAULT ((0)) NOT NULL,
    [OpsaetningData]        NVARCHAR (MAX) CONSTRAINT [DF_GriseVaegt_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]    DATETIME       NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_GriseVaegt_OrdreNr] DEFAULT ((0)) NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_GriseVaegt_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_GriseVaegt_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_GriseVaegt_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GriseVaegt] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GriseVaegt_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_GriseVaegt_Gateway] FOREIGN KEY ([GatewayID]) REFERENCES [dbo].[Gateway] ([DeviceID]),
    CONSTRAINT [FK_GriseVaegt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_GriseVaegt_Sti1] FOREIGN KEY ([StiID]) REFERENCES [dbo].[Sti] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_GriseVaegt]
    ON [dbo].[GriseVaegt]([KundeID] ASC, [ID] ASC);

