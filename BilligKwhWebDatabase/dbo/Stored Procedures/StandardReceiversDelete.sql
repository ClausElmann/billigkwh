
-- =============================================
-- Author:      Peter Kristensen
-- Create Date: 26-08-2020
-- Description: Deletes all standard receivers on a customer including all group-- and prrfile mappings
-- =============================================
CREATE PROCEDURE [dbo].[StandardReceiversDelete]
(
    @CustomerId INT
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

  -- Get all receiver Ids on customer and save to temp table
	SELECT * INTO #stdReceiverIds
	FROM (SELECT Id FROM dbo.StandardReceivers WHERE CustomerId = @CustomerId) X;

	DELETE FROM dbo.StandardReceiverGroupMappings
	WHERE StandardReceiverId IN (SELECT * FROM #stdReceiverIds);
	
	DELETE FROM dbo.StandardReceiverProfileMappings
	WHERE StandardReceiverId IN (SELECT * FROM #stdReceiverIds);
	
	DELETE FROM dbo.StandardReceivers
	WHERE Id IN (SELECT * FROM #stdReceiverIds);
	
	DROP TABLE #stdReceiverIds;
END