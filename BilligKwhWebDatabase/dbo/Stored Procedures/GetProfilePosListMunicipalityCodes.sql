
CREATE PROCEDURE [dbo].[GetProfilePosListMunicipalityCodes]
(
	@CountryId INT,
	@ProfileId INT
)
AS
BEGIN
   
   DECLARE @UseMunicipalityPolList BIT

	EXEC @UseMunicipalityPolList = [dbo].[ProfileInRole] @ProfileId, @RoleName='UseMunicipalityPolList'

	IF (@UseMunicipalityPolList = 0)
	BEGIN

		SELECT DISTINCT
		a.MunicipalityCode, 
		a.MunicipalityName,
		m.Region AS RegionName
		FROM dbo.ProfilePositiveLists p 
		INNER JOIN dbo.Addresses a ON p.Kvhx = a.Kvhx
		INNER JOIN dbo.Municipalities m ON (m.CountryId = @CountryId AND m.MunicipalityCode = a.MunicipalityCode)
		WHERE a.CountryId = @CountryId AND p.ProfileId = @ProfileId
	END
	ELSE
	BEGIN
		SELECT ppmc.MunicipalityCode, m.MunicipalityName, m.Region AS RegionName
		FROM dbo.ProfilePosListMunicipalityCodes ppmc
		INNER JOIN dbo.Municipalities m ON ppmc.MunicipalityCode = m.MunicipalityCode
		WHERE m.CountryId = @CountryId AND ppmc.ProfileId = @ProfileId
	END

END