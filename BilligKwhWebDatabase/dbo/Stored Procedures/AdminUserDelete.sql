
CREATE PROCEDURE [dbo].[AdminUserDelete]
(
	@UserId INT
)
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	-- Validation: Required that Users.Deleted is True 
	DECLARE @IsDeleted BIT = (SELECT TOP(1) Deleted FROM dbo.Users WHERE Id = @UserId)

	IF (ISNULL(@IsDeleted, 0) = 1)
	BEGIN

		BEGIN TRANSACTION

				-- First delete from related datatables.
				-- NB. Tables ImportFiles, ImportRows and ImportColumns will be handled in seperate job (ProfileId/UserId)

				DELETE FROM dbo.UserRoleMappings WHERE UserId = @UserId
				DELETE FROM dbo.CustomerUserMappings WHERE UserId = @UserId
				DELETE FROM dbo.ProfileUserMappings WHERE UserId = @UserId
				DELETE FROM dbo.UserRefreshTokens WHERE UserId = @UserId

				DELETE FROM dbo.CustomerLogs WHERE UserId = @UserId
				DELETE FROM dbo.ImportFiles WHERE UserId = @UserId
				DELETE FROM dbo.NewsletterUserMappings WHERE UserId = @UserId
				DELETE FROM dbo.OperationalMessageDismisseds WHERE UserId = @UserId

				

				-- Finally Delete the User
				DELETE FROM dbo.Users WHERE Id = @UserId

				-- Log it
				INSERT INTO dbo.Logs (LogLevelId,ShortMessage,DateCreatedUtc,Module) VALUES (20, 'Deleted UserId ' + CAST(@UserId AS NVARCHAR(8)),GETUTCDATE(),'AdminUserDelete')

				-- And commit when all done! 
		
		COMMIT TRANSACTION
		SELECT @UserId
	END
	ELSE
	BEGIN
		PRINT 'Not deleted anything! User must be marked with Deleted.'
		SELECT -1
	END


END