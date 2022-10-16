CREATE TABLE [dbo].[MinkRawWeightData] (
    [ID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [Dato]              DATE           CONSTRAINT [DF_MinkRawWeightDataV1_Dato] DEFAULT (getdate()) NOT NULL,
    [DeviceID]          NVARCHAR (25)  NOT NULL,
    [CelleNr]           SMALLINT       NOT NULL,
    [VaegtholdID]       INT            NULL,
    [Data]              NVARCHAR (MAX) NULL,
    [Antal]             SMALLINT       CONSTRAINT [DF_MinkRawWeightDataV1_Antal] DEFAULT ((0)) NOT NULL,
    [HanVaegt]          SMALLINT       NULL,
    [HunVaegt]          SMALLINT       NULL,
    [HanVaegtKorigeret] SMALLINT       NULL,
    [HunVaegtKorigeret] SMALLINT       NULL,
    [IFVersion]         SMALLINT       NOT NULL,
    [ModtagetStart]     DATETIME       CONSTRAINT [DF_MinkRawWeightDataV1_Modtaget] DEFAULT (getdate()) NOT NULL,
    [ModtagetSlut]      DATETIME       CONSTRAINT [DF_MinkRawWeightDataV1_ModtagetStart1] DEFAULT (getdate()) NOT NULL,
    [HanBeregnet]       SMALLINT       NULL,
    [HunBeregnet]       SMALLINT       NULL,
    [HanGnsTilv]        SMALLINT       NULL,
    [HunGnsTilv]        SMALLINT       NULL,
    [UgeAarID]          INT            CONSTRAINT [DF_MinkRawWeightData_UgeAarID] DEFAULT ((0)) NOT NULL,
    [Slettet]           BIT            CONSTRAINT [DF_MinkRawWeightData_Kasseret] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkRawWeightData] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkRawWeightData_MinkVejehold] FOREIGN KEY ([VaegtholdID]) REFERENCES [dbo].[MinkVaegthold] ([ID]),
    CONSTRAINT [FK_MinkRawWeightData_UgeAar] FOREIGN KEY ([UgeAarID]) REFERENCES [dbo].[UgeAar] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_VaegtholdID_DeviceIDNulOrMinusOne]
    ON [dbo].[MinkRawWeightData]([VaegtholdID] ASC, [Dato] ASC, [CelleNr] ASC) WHERE ([DeviceID] IN ('0', '-1'));


GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_deviceID_notNulOrMinusOne]
    ON [dbo].[MinkRawWeightData]([DeviceID] ASC, [Dato] ASC, [CelleNr] ASC) WHERE ([DeviceID]<>'0' AND [DeviceID]<>'-1');


GO
CREATE NONCLUSTERED INDEX [IX_MinkRawWeightData]
    ON [dbo].[MinkRawWeightData]([VaegtholdID] ASC, [Slettet] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkRawWeightData]
    ON [dbo].[MinkRawWeightData]([Dato] ASC, [DeviceID] ASC, [CelleNr] ASC, [VaegtholdID] ASC);

