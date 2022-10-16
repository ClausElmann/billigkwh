CREATE TABLE [dbo].[MinkVejeholdStatus] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]      DATE          CONSTRAINT [DF_MinkVejeholdStatus_Dato] DEFAULT (getdate()) NOT NULL,
    [Time]      INT           NOT NULL,
    [DeviceID]  NVARCHAR (25) NOT NULL,
    [IFVersion] SMALLINT      NOT NULL,
    [Modtaget]  DATETIME      CONSTRAINT [DF_MinkVejeholdStatus_Modtaget] DEFAULT (getdate()) NOT NULL,
    [FR]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax6] DEFAULT ((-1)) NOT NULL,
    [FT]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax5] DEFAULT ((-1)) NOT NULL,
    [CS]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax3] DEFAULT ((-1)) NOT NULL,
    [V1]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax2] DEFAULT ((-1)) NOT NULL,
    [V2]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax1] DEFAULT ((-1)) NOT NULL,
    [V3]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_WIF5] DEFAULT ((-1)) NOT NULL,
    [V4]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_WIF4] DEFAULT ((-1)) NOT NULL,
    [V5]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_WIF2] DEFAULT ((-1)) NOT NULL,
    [V6]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_WIF1] DEFAULT ((-1)) NOT NULL,
    [VS]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax] DEFAULT ((-1)) NOT NULL,
    [SS]        SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_VaegtMax4] DEFAULT ((-1)) NOT NULL,
    [GWSS]      SMALLINT      CONSTRAINT [DF_MinkVejeholdStatus_SS1] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkVejeholdStatus] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVejeholdStatus_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkVejeholdStatus]
    ON [dbo].[MinkVejeholdStatus]([Dato] ASC, [DeviceID] ASC, [Time] ASC);

