
CREATE PROCEDURE [dbo].[AddProfileRoleMapping]
(
	@ProfileId INT = 0,
	@ProfileRoleName NVARCHAR(50)
)
AS
BEGIN
SET NOCOUNT ON;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
  BEGIN TRAN
 
	DECLARE @ProfileRoleId INT = (SELECT TOP(1) Id FROM dbo.ProfileRoles WHERE LOWER([Name]) = LOWER(@ProfileRoleName))

    IF NOT EXISTS ( SELECT * FROM dbo.ProfileRoleMappings WITH (UPDLOCK) WHERE ProfileId = @ProfileId AND ProfileRoleId = @ProfileRoleId)
	BEGIN
		INSERT dbo.ProfileRoleMappings(ProfileRoleId, ProfileId )
		VALUES (  @ProfileRoleId, @ProfileId )
	END
	-- Returnere til app.
	SELECT @@ROWCOUNT

  COMMIT


END