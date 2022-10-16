
CREATE PROCEDURE [dbo].[GetMapAddressByProfile]
(
	@CountryId INT,
	@ProfileId INT,
	@SearchAllAddresses BIT,
	@UseMunicipalityPolList BIT,
	@ZipCode INT = NULL
)
AS
BEGIN

	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	IF (ISNULL(@SearchAllAddresses, 0) = 0)
	BEGIN
		IF (ISNULL(@UseMunicipalityPolList, 0) = 0)
		BEGIN
			SELECT a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude, COUNT(*) AS NumberofFloorDoors
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePositiveLists pl ON a.Kvhx = pl.Kvhx
			WHERE (a.CountryId = @CountryId) AND (pl.ProfileId = @ProfileId) AND
				  (@ZipCode IS NULL OR a.Zipcode = @ZipCode)
			GROUP BY a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude
			ORDER BY a.CountryId, a.Zipcode, a.Street, a.Number, a.Letter, a.Meters
		END
		ELSE 
		BEGIN
			SELECT DISTINCT a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude, COUNT(*) AS NumberofFloorDoors
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePosListMunicipalityCodes pmc ON a.MunicipalityCode = pmc.MunicipalityCode
			WHERE (a.CountryId = @CountryId) AND (pmc.ProfileId = @ProfileId) AND
				  (@ZipCode IS NULL OR a.Zipcode = @ZipCode)
			GROUP BY a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude		
			ORDER BY a.CountryId, a.Zipcode, a.Street, a.Number, a.Letter, a.Meters
		END
	END 
	ELSE
		BEGIN 
			SELECT a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude, COUNT(*) AS NumberofFloorDoors
			FROM dbo.Addresses a
			WHERE (a.CountryId = @CountryId) AND
				  (@ZipCode IS NULL OR a.Zipcode = @ZipCode)
			GROUP BY a.CountryId, a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Meters, a.Kvh, a.Latitude, a.Longitude
			ORDER BY a.CountryId, a.Zipcode, a.Street, a.Number, a.Letter, a.Meters
		END
END