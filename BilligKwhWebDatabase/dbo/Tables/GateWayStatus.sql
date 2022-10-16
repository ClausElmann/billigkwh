CREATE TABLE [dbo].[GateWayStatus] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]      DATE          CONSTRAINT [DF_GateWayStatus_Dato] DEFAULT (getdate()) NOT NULL,
    [Time]      INT           NOT NULL,
    [DeviceID]  NVARCHAR (25) NOT NULL,
    [IFVersion] SMALLINT      NOT NULL,
    [Modtaget]  DATETIME      CONSTRAINT [DF_GateWayStatus_Modtaget] DEFAULT (getdate()) NOT NULL,
    [FR]        SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax6] DEFAULT ((-1)) NOT NULL,
    [FT]        SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax5] DEFAULT ((-1)) NOT NULL,
    [TT]        SMALLINT      CONSTRAINT [DF_GateWayStatus_CR1] DEFAULT ((-1)) NOT NULL,
    [CS]        SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax3] DEFAULT ((-1)) NOT NULL,
    [WTO]       SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax2] DEFAULT ((-1)) NOT NULL,
    [WCF]       SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax1] DEFAULT ((-1)) NOT NULL,
    [WIF]       SMALLINT      CONSTRAINT [DF_GateWayStatus_WIF5] DEFAULT ((-1)) NOT NULL,
    [WMT]       SMALLINT      CONSTRAINT [DF_GateWayStatus_WIF4] DEFAULT ((-1)) NOT NULL,
    [WMR]       SMALLINT      CONSTRAINT [DF_GateWayStatus_WIF2] DEFAULT ((-1)) NOT NULL,
    [VDR]       SMALLINT      CONSTRAINT [DF_GateWayStatus_WIF1] DEFAULT ((-1)) NOT NULL,
    [SDR]       SMALLINT      CONSTRAINT [DF_GateWayStatus_VaegtMax] DEFAULT ((-1)) NOT NULL,
    [BDR]       SMALLINT      CONSTRAINT [DF_GateWayStatus_SDR2] DEFAULT ((-1)) NOT NULL,
    [SS]        SMALLINT      CONSTRAINT [DF_GateWayStatus_SS] DEFAULT ((-1)) NOT NULL,
    [WSS]       SMALLINT      CONSTRAINT [DF_GateWayStatus_SS1] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_GateWayStatus] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_GateWayStatus]
    ON [dbo].[GateWayStatus]([Dato] ASC, [DeviceID] ASC, [Time] ASC);

