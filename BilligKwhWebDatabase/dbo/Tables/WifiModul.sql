CREATE TABLE [dbo].[WifiModul] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [UniqueID]              NVARCHAR (50) NOT NULL,
    [FoderVaegtID]          INT           CONSTRAINT [DF_FoderVaegtWifiModul_FoderVaegtID] DEFAULT ((0)) NOT NULL,
    [GatewayID]             NVARCHAR (25) CONSTRAINT [DF_WifiModul_OpsamlingBoxID] DEFAULT ((0)) NOT NULL,
    [KundeID]               INT           CONSTRAINT [DF_FoderVaegtWifiModul_KundeID] DEFAULT ((0)) NOT NULL,
    [Offline]               BIT           CONSTRAINT [DF_FoderVaegtWifiModul_Offline] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME      NOT NULL,
    [SidstRettetAfBrugerID] INT           NOT NULL,
    [Slettet]               BIT           NOT NULL,
    [GriseVaegtBoxID]       INT           CONSTRAINT [DF_FoderVaegtWifiModul_GriseVaegtBoxID] DEFAULT ((0)) NULL,
    [MinkVaegtBoxID]        INT           CONSTRAINT [DF_FoderVaegtWifiModul_MinkVaegtBoxID] DEFAULT ((0)) NULL,
    [GensendDataCount]      INT           NULL,
    CONSTRAINT [PK_FoderVaegtWifiModul] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderVaegtWifiModul_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderVaegtWifiModul_FoderVaegt] FOREIGN KEY ([FoderVaegtID]) REFERENCES [dbo].[FoderVaegt] ([ID]),
    CONSTRAINT [FK_FoderVaegtWifiModul_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_WifiModul_Gateway] FOREIGN KEY ([GatewayID]) REFERENCES [dbo].[Gateway] ([DeviceID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_FoderVaegtWifiModul]
    ON [dbo].[WifiModul]([UniqueID] ASC);

