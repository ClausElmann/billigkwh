CREATE PROC [dbo].[SetRequestLogState]
@loggingOn bit,
@denyBots  bit

AS


IF NOT EXISTS (SELECT TOP 1 LoggingOn FROM dbo.RequestLog )
BEGIN
INSERT INTO dbo.LogStatus
( LoggingOn,DenyBots) VALUES (@loggingOn, @denyBots)
END
ELSE
BEGIN
UPDATE dbo.LogStatus
SET 
LoggingOn = @loggingOn,
DenyBots = @denyBots
END