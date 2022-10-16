
CREATE PROCEDURE [dbo].[SmsLogsDelete]
(
	@SmsGroupId INT = -1
)
AS

BEGIN

	DECLARE @Rowcount INT = 1

	WHILE @Rowcount > 0
	BEGIN
		DELETE TOP (4900) FROM dbo.SmsLogs WHERE SmsGroupId = @SmsGroupId
		SET @Rowcount = @@ROWCOUNT
	END

	SET @Rowcount = 1
	WHILE @Rowcount > 0
	BEGIN
		DELETE TOP (4900) FROM dbo.SmsLogsNoPhoneAddresses WHERE SmsGroupId = @SmsGroupId
		SET @Rowcount = @@ROWCOUNT
	END
	

END