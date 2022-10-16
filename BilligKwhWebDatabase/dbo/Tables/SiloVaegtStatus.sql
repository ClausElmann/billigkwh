CREATE TABLE [dbo].[SiloVaegtStatus] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]      DATE          CONSTRAINT [DF_SiloVaegtStatus_Dato] DEFAULT (getdate()) NOT NULL,
    [Time]      INT           NOT NULL,
    [DeviceID]  NVARCHAR (25) NOT NULL,
    [IFVersion] SMALLINT      NOT NULL,
    [Modtaget]  DATETIME      CONSTRAINT [DF_SiloVaegtStatus_Modtaget] DEFAULT (getdate()) NOT NULL,
    [FR]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax6] DEFAULT ((-1)) NOT NULL,
    [FT]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax5] DEFAULT ((-1)) NOT NULL,
    [CS]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax3] DEFAULT ((-1)) NOT NULL,
    [VN]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax2] DEFAULT ((-1)) NOT NULL,
    [VS]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax] DEFAULT ((-1)) NOT NULL,
    [SS]        SMALLINT      CONSTRAINT [DF_SiloVaegtStatus_VaegtMax4] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_SiloVaegtStatus] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SiloVaegtStatus_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_SiloVaegtStatus]
    ON [dbo].[SiloVaegtStatus]([Dato] ASC, [DeviceID] ASC, [Time] ASC);

