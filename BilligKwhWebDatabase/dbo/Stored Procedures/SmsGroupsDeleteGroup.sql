
CREATE PROCEDURE [dbo].[SmsGroupsDeleteGroup]
(
	@SmsGroupId INT = -1
)
AS

BEGIN
    SET NOCOUNT ON

    BEGIN TRANSACTION
		DELETE FROM dbo.WebMessages WHERE SmsGroupId = @SmsGroupId -- IMPORTANT. Must delete from keyed tables first
		DELETE FROM dbo.SmsLogs WHERE SmsGroupId = @SmsGroupId
		DELETE FROM dbo.SmsGroupItemMergeFields WHERE SmsGroupId = @SmsGroupId
		DELETE FROM dbo.SmsGroupItems WHERE SmsGroupId = @SmsGroupId
		DELETE FROM dbo.SmsLogsNoPhoneAddresses WHERE SmsGroupId = @SmsGroupId
		DELETE FROM dbo.SmsGroups WHERE Id = @SmsGroupId
	COMMIT TRANSACTION

END