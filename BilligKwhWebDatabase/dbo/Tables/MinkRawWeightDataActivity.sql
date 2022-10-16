CREATE TABLE [dbo].[MinkRawWeightDataActivity] (
    [ID]     BIGINT   IDENTITY (1, 1) NOT NULL,
    [DataID] BIGINT   NOT NULL,
    [Hour]   SMALLINT CONSTRAINT [DF_Table_1_Dato_1] DEFAULT ((0)) NOT NULL,
    [Antal]  SMALLINT CONSTRAINT [DF_MinkRawWeightDataActivity_Antal] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkRawWeightDataActivity] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkRawWeightDataActivity_MinkRawWeightData] FOREIGN KEY ([DataID]) REFERENCES [dbo].[MinkRawWeightData] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_MinkRawWeightDataActivity]
    ON [dbo].[MinkRawWeightDataActivity]([DataID] ASC, [Hour] ASC);

