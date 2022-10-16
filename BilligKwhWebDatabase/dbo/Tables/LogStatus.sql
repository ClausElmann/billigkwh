CREATE TABLE [dbo].[LogStatus] (
    [LoggingOn]   BIT CONSTRAINT [DF_LogStatus_State] DEFAULT ((0)) NOT NULL,
    [DenyBots]    BIT CONSTRAINT [DF_LogStatus_DenyBots] DEFAULT ((0)) NOT NULL,
    [MinDuration] INT NOT NULL
);

