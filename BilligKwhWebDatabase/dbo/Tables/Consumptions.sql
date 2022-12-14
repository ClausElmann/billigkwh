CREATE TABLE [dbo].[Consumptions] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Date]           DATE           NOT NULL,
    [DeviceId]       INT            NOT NULL,
    [LastUpdatedUtc] DATETIME       NOT NULL,
    [H00]            BIGINT         NOT NULL,
    [H01]            BIGINT         NOT NULL,
    [H02]            BIGINT         NOT NULL,
    [H03]            BIGINT         NOT NULL,
    [H04]            BIGINT         NOT NULL,
    [H05]            BIGINT         NOT NULL,
    [H06]            BIGINT         NOT NULL,
    [H07]            BIGINT         NOT NULL,
    [H08]            BIGINT         NOT NULL,
    [H09]            BIGINT         NOT NULL,
    [H10]            BIGINT         NOT NULL,
    [H11]            BIGINT         NOT NULL,
    [H12]            BIGINT         NOT NULL,
    [H13]            BIGINT         NOT NULL,
    [H14]            BIGINT         NOT NULL,
    [H15]            BIGINT         NOT NULL,
    [H16]            BIGINT         NOT NULL,
    [H17]            BIGINT         NOT NULL,
    [H18]            BIGINT         NOT NULL,
    [H19]            BIGINT         NOT NULL,
    [H20]            BIGINT         NOT NULL,
    [H21]            BIGINT         NOT NULL,
    [H22]            BIGINT         NOT NULL,
    [H23]            BIGINT         NOT NULL,
    [C00]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C00] DEFAULT ((-1)) NOT NULL,
    [C01]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C01] DEFAULT ((-1)) NOT NULL,
    [C02]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C02] DEFAULT ((-1)) NOT NULL,
    [C03]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C03] DEFAULT ((-1)) NOT NULL,
    [C04]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C04] DEFAULT ((-1)) NOT NULL,
    [C05]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C05] DEFAULT ((-1)) NOT NULL,
    [C06]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C06] DEFAULT ((-1)) NOT NULL,
    [C07]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C07] DEFAULT ((-1)) NOT NULL,
    [C08]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C08] DEFAULT ((-1)) NOT NULL,
    [C09]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C09] DEFAULT ((-1)) NOT NULL,
    [C10]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C10] DEFAULT ((-1)) NOT NULL,
    [C11]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C11] DEFAULT ((-1)) NOT NULL,
    [C12]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C12] DEFAULT ((-1)) NOT NULL,
    [C13]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C13] DEFAULT ((-1)) NOT NULL,
    [C14]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C14] DEFAULT ((-1)) NOT NULL,
    [C15]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C15] DEFAULT ((-1)) NOT NULL,
    [C16]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C16] DEFAULT ((-1)) NOT NULL,
    [C17]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C17] DEFAULT ((-1)) NOT NULL,
    [C18]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C18] DEFAULT ((-1)) NOT NULL,
    [C19]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C19] DEFAULT ((-1)) NOT NULL,
    [C20]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C20] DEFAULT ((-1)) NOT NULL,
    [C21]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C21] DEFAULT ((-1)) NOT NULL,
    [C22]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C22] DEFAULT ((-1)) NOT NULL,
    [C23]            DECIMAL (5, 1) CONSTRAINT [DF_Consumptions_C23] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_Consumption] PRIMARY KEY CLUSTERED ([Id] ASC)
);





