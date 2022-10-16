
CREATE PROCEDURE [dbo].[StandardReceiverGroupDelete]
(
	@GroupId INT,
	@CustomerId INT
)
AS

BEGIN

    BEGIN TRANSACTION
		DELETE FROM dbo.StandardReceiverGroupMappings WHERE StandardReceiverGroupId = @GroupId
		DELETE FROM dbo.StandardReceiverGroupProfileMappings WHERE StandardReceiverGroupId = @GroupId
		DELETE FROM dbo.StandardReceiverGroups WHERE CustomerId = @CustomerId AND Id = @GroupId
	COMMIT TRANSACTION

END