CREATE PROCEDURE [dbo].[SmsLogsUpdateStatusCodeById]
(
	@StatusCode INT,
	@SmsLogId INT	
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.SmsLogs 
	SET
		StatusCode = @StatusCode,
		DateStatusUpdatedUtc = GETUTCDATE()	
	WHERE Id = @SmsLogId	
END