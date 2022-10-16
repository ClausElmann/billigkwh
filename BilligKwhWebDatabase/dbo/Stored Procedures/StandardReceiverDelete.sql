
CREATE PROCEDURE [dbo].[StandardReceiverDelete]
(
	@StandardReceiverId INT
)
AS

BEGIN

    BEGIN TRANSACTION
		DELETE FROM dbo.StandardReceiverProfileMappings WHERE StandardReceiverId = @StandardReceiverId
		DELETE FROM dbo.StandardReceiverGroupMappings WHERE StandardReceiverId = @StandardReceiverId
		DELETE FROM dbo.StandardReceivers WHERE Id = @StandardReceiverId

	COMMIT TRANSACTION

END