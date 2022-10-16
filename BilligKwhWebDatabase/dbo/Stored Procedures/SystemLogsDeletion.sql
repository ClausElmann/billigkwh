

CREATE PROCEDURE [dbo].[SystemLogsDeletion]

AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

	/*
	public enum LogLevel
	{
		Debug = 10,
		Information = 20,
		Warning = 30,
		Error = 40,
		Fatal = 50
	*/

    SET NOCOUNT ON

	DECLARE @DebugInfo DATETIME = (SELECT DATEADD(month,-1, GETUTCDATE()))
	DECLARE @Warnings DATETIME = (SELECT DATEADD(month, -3, GETUTCDATE()))
	DECLARE @Errors DATETIME = (SELECT DATEADD(month, -6, GETUTCDATE()))
	DECLARE @Fatals DATETIME = (SELECT DATEADD(month, -12, GETUTCDATE()))

    DECLARE @Rowcount INT = 1
	WHILE @Rowcount > 0
	BEGIN
	DELETE TOP (5000) 
	  FROM dbo.Logs 
		WHERE 
		(
			(LogLevelId IN (10,20) AND DateCreatedUtc < @DebugInfo) OR
			(LogLevelId = 30 AND DateCreatedUtc < @Warnings) OR
			(LogLevelId = 40 AND DateCreatedUtc < @Errors) OR
			(LogLevelId = 50 AND DateCreatedUtc < @Fatals)
		)

	  SET @Rowcount = @@ROWCOUNT

	  WAITFOR DELAY '00:00:00.500'

	END


END