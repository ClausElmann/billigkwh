-- =============================================
-- Author:      Henrik K. Madsen
-- Create Date: 07-09-2020
-- Description: Delets old broardcasts older than the customer setteing "MonthToDeleteBroadcast"
-- =============================================
CREATE PROCEDURE [dbo].[SmsGroupsDeleteOldByCustomerSetting]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	DECLARE @GroupId bigint
	DECLARE GroupCursor CURSOR FOR	SELECT g.Id
									FROM SmsGroups as g
									INNER JOIN Profiles p on p.Id =  g.ProfileId
									INNER JOIN Customers c on c.Id = p.CustomerId
									WHERE ISNULL(g.DateDelayToUtc, g.DateCreatedUtc) < DATEADD(month, - c.MonthToDeleteBroadcast,getutcdate())

	OPEN GroupCursor

	FETCH NEXT FROM GroupCursor INTO @GroupId
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		--Update Benchmark GroupId to NULL
		Update Benchmarks set SmsGroupId = 0 Where SmsGroupId = @GroupId

		EXEC SmsGroupsDeleteGroup @GroupId
		FETCH NEXT FROM GroupCursor INTO @GroupId
	END

	CLOSE GroupCursor
	DEALLOCATE GroupCursor

END