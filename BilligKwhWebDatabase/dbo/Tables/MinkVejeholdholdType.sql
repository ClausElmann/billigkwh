CREATE TABLE [dbo].[MinkVejeholdholdType] (
    [ID]               SMALLINT      NOT NULL,
    [Navn]             NVARCHAR (50) NOT NULL,
    [Label]            NVARCHAR (6)  NOT NULL,
    [VinterHold]       BIT           CONSTRAINT [DF_MinkVejeholdholdType_VinterHold] DEFAULT ((0)) NOT NULL,
    [Placering]        SMALLINT      CONSTRAINT [DF_MinkVejeholdholdType_Sort] DEFAULT ((0)) NOT NULL,
    [HoldSaesonTypeID] SMALLINT      CONSTRAINT [DF_MinkVejeholdholdType_HoldSaesonTypeID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkVejeholdholdType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

