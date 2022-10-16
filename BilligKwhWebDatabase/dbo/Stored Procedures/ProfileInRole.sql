
CREATE PROCEDURE [dbo].[ProfileInRole]
	(
		@ProfileId          int,
		@RoleName        nvarchar(256)
	)
AS
    IF (@ProfileId IS NULL)
        RETURN(0)

    DECLARE @RoleId int
    SELECT  @RoleId = NULL

    SELECT  @RoleId = Id
    FROM    dbo.ProfileRoles
    WHERE   LOWER(TRIM(Name)) = LOWER(@RoleName) 

    IF (@RoleId IS NULL)
        RETURN(0)

    IF (EXISTS( SELECT * FROM dbo.ProfileRolemappings WHERE ProfileId = @ProfileId AND ProfileRoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)


	/* SET NOCOUNT ON */
	RETURN