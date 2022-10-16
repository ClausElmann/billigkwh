
CREATE PROCEDURE [dbo].[SmsGroupsItemsDelete]
(
	@SmsGroupId INT = -1
)
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	BEGIN TRAN
		DECLARE @Rowcount INT = 1

		WHILE @Rowcount > 0
		BEGIN
			DELETE TOP (10000) FROM dbo.SmsGroupItems WHERE SmsGroupId = @SmsGroupId
			SET @Rowcount = @@ROWCOUNT
		END

		SET @Rowcount = 1
		WHILE @Rowcount > 0
		BEGIN
			DELETE TOP (10000) FROM dbo.SmsGroupItemMergeFields WHERE SmsGroupId = @SmsGroupId
			SET @Rowcount = @@ROWCOUNT
		END
	COMMIT TRAN

END