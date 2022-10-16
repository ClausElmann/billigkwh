CREATE TABLE [dbo].[DemoDataTimer] (
    [ID]    INT IDENTITY (1, 1) NOT NULL,
    [Time]  INT NOT NULL,
    [vand]  INT NOT NULL,
    [foder] INT NOT NULL,
    CONSTRAINT [PK_DemoDataTimer] PRIMARY KEY CLUSTERED ([ID] ASC)
);

