CREATE TABLE [dbo].[SiloRawWeightData] (
    [ID]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]         DATE          CONSTRAINT [DF_SiloRawWeightData_Dato] DEFAULT (getdate()) NOT NULL,
    [Time]         INT           NOT NULL,
    [DeviceID]     NVARCHAR (25) NOT NULL,
    [VaegtMax]     INT           CONSTRAINT [DF_SiloRawWeightData_VaegtMax] DEFAULT ((-1)) NOT NULL,
    [VaegtMin]     INT           CONSTRAINT [DF_SiloRawWeightData_VaegtMin] DEFAULT ((-1)) NOT NULL,
    [VaegtSeneste] INT           CONSTRAINT [DF_SiloRawWeightData_VaegtSeneste] DEFAULT ((-1)) NOT NULL,
    [IFVersion]    SMALLINT      NOT NULL,
    [Modtaget]     DATETIME      CONSTRAINT [DF_SiloRawWeightData_Modtaget] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SiloRawWeightData] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SiloRawWeightData_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_SiloRawWeightData]
    ON [dbo].[SiloRawWeightData]([Dato] ASC, [DeviceID] ASC, [Time] ASC);

