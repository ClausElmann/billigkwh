CREATE TABLE [dbo].[SiloParameterData] (
    [ID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [Dato]     DATETIME      CONSTRAINT [DF_SiloParameterData_Dato] DEFAULT (getdate()) NOT NULL,
    [DeviceID] NVARCHAR (25) NOT NULL,
    [Modtaget] DATETIME      CONSTRAINT [DF_SiloParameterData_Modtaget] DEFAULT (getdate()) NOT NULL,
    [VA]       INT           CONSTRAINT [DF_SiloParameterData_VaegtMax] DEFAULT ((-2)) NOT NULL,
    [V1SEL]    BIT           NULL,
    [V2SEL]    BIT           NULL,
    [V3SEL]    BIT           NULL,
    [V4SEL]    BIT           NULL,
    [V5SEL]    BIT           NULL,
    [V6SEL]    BIT           NULL,
    [V1ADC]    INT           CONSTRAINT [DF_SiloParameterData_VA1_1] DEFAULT ((-2)) NOT NULL,
    [V2ADC]    INT           CONSTRAINT [DF_SiloParameterData_V1ADC1] DEFAULT ((-2)) NOT NULL,
    [V3ADC]    INT           CONSTRAINT [DF_SiloParameterData_V1ADC1_1] DEFAULT ((-2)) NOT NULL,
    [V4ADC]    INT           CONSTRAINT [DF_SiloParameterData_V3ADC1] DEFAULT ((-2)) NOT NULL,
    [V5ADC]    INT           CONSTRAINT [DF_SiloParameterData_V3ADC1_1] DEFAULT ((-2)) NOT NULL,
    [V6ADC]    INT           CONSTRAINT [DF_SiloParameterData_V3ADC1_2] DEFAULT ((-2)) NOT NULL,
    [SUMADC]   INT           CONSTRAINT [DF_SiloParameterData_SUMADC] DEFAULT ((-2)) NOT NULL,
    [ITEMP]    SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP] DEFAULT ((0)) NOT NULL,
    [ITADVAL]  SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP1_1] DEFAULT ((0)) NOT NULL,
    [ETEMP]    SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP1] DEFAULT ((0)) NOT NULL,
    [ETADVAL]  SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP2] DEFAULT ((0)) NOT NULL,
    [EFUGT]    SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP3] DEFAULT ((0)) NOT NULL,
    [EFADVAL]  SMALLINT      CONSTRAINT [DF_SiloParameterData_ETEMP4] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SiloParameterData] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SiloParameterData_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_SiloParameterData]
    ON [dbo].[SiloParameterData]([DeviceID] ASC, [ID] ASC);

