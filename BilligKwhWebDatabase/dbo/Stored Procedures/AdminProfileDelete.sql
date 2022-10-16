
CREATE PROCEDURE [dbo].[AdminProfileDelete]
(
	@ProfileId INT
)
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	-- Validation: Required that Profiles.Deleted is True 
	DECLARE @IsDeleted BIT = (SELECT TOP(1) Deleted FROM dbo.Profiles WHERE Id = @ProfileId)

	IF (ISNULL(@IsDeleted, 0) = 1)
	BEGIN

		BEGIN TRANSACTION

				-- First delete from related datatables.
				-- NB. Tables ImportFiles, ImportRows and ImportColumns will be handled in seperate job (ProfileId/UserId)

				
				-- Std. receiver stuff
				DELETE FROM dbo.StandardReceiverGroupProfileMappings WHERE ProfileId = @ProfileId
				DELETE FROM dbo.StandardReceiverProfileMappings WHERE ProfileId = @ProfileId
				
				DELETE FROM dbo.FtpSettings WHERE ProfileId = @ProfileId
				DELETE FROM dbo.OperationalMessageDismisseds WHERE ProfileId = @ProfileId
				DELETE FROM dbo.SocialMediaAccountProfileMappings WHERE ProfileId = @ProfileId
				DELETE FROM dbo.StatstidendeSubscriptions WHERE ProfileId = @ProfileId
				DELETE FROM dbo.TemplateProfileMappings WHERE ProfileId = @ProfileId

				-- WEB module settings
				DELETE FROM dbo.WebMessageMapModuleProfiles WHERE ProfileId = @ProfileId
				DELETE FROM dbo.WebMessageModuleProfileMappings WHERE ProfileId = @ProfileId
				DELETE FROM dbo.WebMessageModuleProfiles WHERE ProfileId = @ProfileId
				DELETE FROM dbo.WebMessages WHERE ProfileId = @ProfileId

				-- Pos. lists stuff
				DELETE FROM dbo.ProfilePosListMunicipalityCodes WHERE ProfileId = @ProfileId
				DELETE FROM dbo.ProfilePositiveLists WHERE ProfileId = @ProfileId
				DELETE FROM dbo.ProfilePositiveListFiles WHERE ProfileId = @ProfileId
				DELETE FROM dbo.HierarchyPoslistGroups WHERE ProfileId = @ProfileId

				DELETE FROM dbo.ProfilePosListInvalidRows 
				WHERE ProfilePositiveListFileId IN
				(
					SELECT ir.ProfilePositiveListFileId
					FROM dbo.ProfilePosListInvalidRows ir 
						INNER JOIN dbo.ProfilePositiveListFiles f ON
						ir.ProfilePositiveListFileId = f.Id
					WHERE f.ProfileId = @ProfileId
				)

				DELETE FROM dbo.BenchmarkReports WHERE ProfileId = @ProfileId
				
				DELETE FROM dbo.ProfileRoleMappings WHERE ProfileId = @ProfileId
				DELETE FROM dbo.ProfileUserMappings WHERE ProfileId = @ProfileId

				-- Finally Delete the Profile
				DELETE FROM dbo.Profiles WHERE Id = @ProfileId

				-- Log it
				INSERT INTO dbo.Logs (LogLevelId,ShortMessage,DateCreatedUtc,Module) VALUES (20, 'Deleted ProfileId ' + CAST(@ProfileId AS NVARCHAR(8)),GETUTCDATE(),'AdminProfileDelete')

				-- And commit when all done! 
		
		COMMIT TRANSACTION
		SELECT @ProfileId
	END
	ELSE
	BEGIN
		PRINT 'Not deleted anything! Profile must be marked with Deleted.'
		SELECT -1
	END

END