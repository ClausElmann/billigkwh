
CREATE PROCEDURE [dbo].[GetAddressesByProfile]
(
	@CountryId INT,
	@ProfileId INT,
	@UseMunicipalityPolList BIT,
	@ZipCode INT = NULL,
	@City NVARCHAR(100) = NULL,
	@Street NVARCHAR(100) = NULL,
	@FromNumber INT = NULL,
	@ToNumber INT = NULL,
	@EvenOdd BIT = NULL,
	@Search NVARCHAR(200) = NULL
)
AS
BEGIN

	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	IF (ISNULL(@UseMunicipalityPolList, 0) = 0)
	BEGIN
		SELECT a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Floor, a.Door, a.Meters, a.Kvhx, a.Latitude, a.Longitude, a.CountryId
		FROM dbo.Addresses a
		INNER JOIN dbo.ProfilePositiveLists pl ON a.Kvhx = pl.Kvhx
		WHERE 
		(a.CountryId = @CountryId) 
		AND a.DateDeletedUtc IS NULL
		AND (pl.ProfileId = @ProfileId)
		AND (ISNULL(@ZipCode, 0) = 0 OR (@ZipCode = a.Zipcode))
		AND ((@City IS NULL) OR (@City = a.City))
		AND ((@Street IS NULL) OR (@Street = a.Street))
		AND ((@EvenOdd IS NULL) OR (a.Number IS NULL) OR (@EvenOdd = 1 AND a.Number % 2 = 0) OR (@EvenOdd = 0 AND a.Number % 2 = 1))
		AND ((@FromNumber IS NULL) OR (a.Number >= @FromNumber))
		AND ((@ToNumber IS NULL) OR (a.Number <= @ToNumber))
		ORDER BY a.Zipcode, a.Street, a.Number, a.Letter, a.Floor, a.Door, a.Meters
	END
	ELSE 
	BEGIN
		SELECT a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Floor, a.Door, a.Meters, a.Kvhx, a.Latitude, a.Longitude, a.CountryId
		FROM dbo.Addresses a
		INNER JOIN dbo.ProfilePosListMunicipalityCodes pmc ON a.MunicipalityCode = pmc.MunicipalityCode
		WHERE 
		(a.CountryId = @CountryId) 
		AND a.DateDeletedUtc IS NULL
		AND (pmc.ProfileId = @ProfileId)
		AND (ISNULL(@ZipCode, 0) = 0 OR (@ZipCode = a.Zipcode))
		AND ((@City IS NULL) OR (@City = a.City))
		AND ((@Street IS NULL) OR (@Street = a.Street))
		AND ((@EvenOdd IS NULL) OR (a.Number IS NULL) OR (@EvenOdd = 1 AND a.Number % 2 = 0) OR (@EvenOdd = 0 AND a.Number % 2 = 1))
		AND ((@FromNumber IS NULL) OR (a.Number >= @FromNumber))
		AND ((@ToNumber IS NULL) OR (a.Number <= @ToNumber))
		ORDER BY a.Zipcode, a.Street, a.Number, a.Letter, a.Floor, a.Door, a.Meters
	END

END