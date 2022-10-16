CREATE TABLE [dbo].[MiljoData] (
    [ID]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]          DATE          CONSTRAINT [DF_MiljoDataV1_Dato] DEFAULT (getdate()) NOT NULL,
    [DeviceID]      NVARCHAR (25) NOT NULL,
    [Time]          SMALLINT      NOT NULL,
    [Temperatur]    FLOAT (53)    NULL,
    [LuftFugtighed] FLOAT (53)    NULL,
    [LuftKvalitet]  FLOAT (53)    NULL,
    [LysStyrke]     SMALLINT      NULL,
    [IFVersion]     SMALLINT      NOT NULL,
    [ModtagetStart] DATETIME      CONSTRAINT [DF_MiljoDataV1_Modtaget] DEFAULT (getdate()) NOT NULL,
    [ModtagetSlut]  DATETIME      CONSTRAINT [DF_MiljoDataV1_ModtagetStart1] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MiljoData] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MiljoData]
    ON [dbo].[MiljoData]([Dato] ASC, [DeviceID] ASC, [Time] ASC);

