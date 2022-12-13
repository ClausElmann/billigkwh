CREATE TABLE [dbo].[ElectricityPrices] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [HourDK]     DATETIME       CONSTRAINT [DF_Elpris_DateDk] DEFAULT (getdate()) NOT NULL,
    [HourDKNo]   INT            NOT NULL,
    [HourUTC]    DATETIME       NOT NULL,
    [HourUTCNo]  INT            CONSTRAINT [DF_ElectricityPrice_HourUTCNo] DEFAULT ((0)) NOT NULL,
    [Dk1]        DECIMAL (4, 2) NOT NULL,
    [Dk2]        DECIMAL (4, 2) NOT NULL,
    [UpdatedUtc] DATETIME       NOT NULL,
    CONSTRAINT [PK_ElectricityPrices] PRIMARY KEY CLUSTERED ([HourDK] DESC)
);








GO


