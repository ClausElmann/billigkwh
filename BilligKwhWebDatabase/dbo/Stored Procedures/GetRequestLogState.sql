CREATE PROC [dbo].[GetRequestLogState]

AS

SELECT TOP 1 LoggingOn, DenyBots, MinDuration FROM dbo.LogStatus