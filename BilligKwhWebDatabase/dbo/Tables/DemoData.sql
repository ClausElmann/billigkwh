CREATE TABLE [dbo].[DemoData] (
    [ID]    INT        IDENTITY (1, 1) NOT NULL,
    [Dag]   INT        NOT NULL,
    [Vægt]  FLOAT (53) NOT NULL,
    [Foder] FLOAT (53) NOT NULL,
    [Vand]  FLOAT (53) NOT NULL,
    [Mix]   INT        CONSTRAINT [DF_DemoData_Mix] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DemoData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

