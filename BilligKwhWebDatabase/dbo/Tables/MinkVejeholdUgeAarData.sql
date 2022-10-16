CREATE TABLE [dbo].[MinkVejeholdUgeAarData] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [VejeholdID] INT NOT NULL,
    [UgeAarID]   INT NOT NULL,
    [HanGns]     INT NULL,
    [HunGns]     INT NULL,
    CONSTRAINT [PK_MinkVejeholdUgeAarData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkVejeholdUgeAarData_MinkVejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[MinkVejehold] ([ID]),
    CONSTRAINT [FK_MinkVejeholdUgeAarData_UgeAar] FOREIGN KEY ([UgeAarID]) REFERENCES [dbo].[UgeAar] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_MinkVejeholdUgeAarData]
    ON [dbo].[MinkVejeholdUgeAarData]([UgeAarID] ASC, [VejeholdID] ASC);

