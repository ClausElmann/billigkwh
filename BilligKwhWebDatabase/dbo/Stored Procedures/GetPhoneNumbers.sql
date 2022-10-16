
CREATE PROCEDURE [dbo].[GetPhoneNumbers]
(
	@ProfileId INT,
	@CountryId INT,
	@Zipcode INT,
	@Street  NVARCHAR(200),
	@Number INT,
	@Letter NVARCHAR(3),
	@Floor NVARCHAR(3),
	@Door NVARCHAR(3),
	@Meters INT,
	@IncludeLandline BIT = 1
)
AS
BEGIN
SET NOCOUNT ON;

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @UseMunicipalityPolList BIT = 0

EXEC @UseMunicipalityPolList = dbo.ProfileInRole @ProfileId = @ProfileId, @RoleName = N'UseMunicipalityPolList'


-- Kommune-posliste
IF (@UseMunicipalityPolList = 1)
BEGIN
	
DECLARE @MunicipalityCodeTabel dbo.MunCodesTableType

-- Fill our muncode table
INSERT INTO @MunicipalityCodeTabel 
SELECT DISTINCT MunicipalityCode FROM dbo.ProfilePosListMunicipalityCodes WHERE ProfileId = @ProfileId


	SELECT
		pn.NumberIdentifier as NumberIdentifier,
		null as Email,
		pn.DisplayName as DisplayName,
		pn.PhoneNumberType,
		a.Id,
		a.Zipcode,
		a.City, 
		a.LocationName,
		a.Kvhx,
		a.Latitude,
		a.Longitude,
		a.Street,
		a.Number as StreetBuildingNumber,
		a.Letter,
		a.Floor, 
		a.Door,
		a.Meters
	FROM 
		dbo.PhoneNumbers pn
	INNER JOIN 
		dbo.Addresses a  ON a.Kvhx = pn.Kvhx
	WHERE
		a.CountryId = @CountryId AND
		a.MunicipalityCode IN (SELECT MunicipalityCode FROM @MunicipalityCodeTabel) AND
        (@Zipcode IS NULL OR a.ZipCode = @Zipcode) AND
		(@Street IS NULL OR a.Street = @Street) AND
		(@Number IS NULL OR @Number = a.Number) AND
		(@Letter IS NULL OR @Letter = a.Letter OR (@Letter = '0' AND ISNULL(a.Letter, '') = '')) AND
		(@Floor IS NULL OR @Floor = a.Floor) AND
		(@Door IS NULL OR @Door = a.Door) AND
		(@Meters IS NULL OR @Meters = a.Meters) AND
		(@IncludeLandline = 1 OR pn.PhoneNumberType = 1)
	ORDER BY 
		a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Floor, a.Door
END
ELSE
BEGIN
	SELECT
		pn.NumberIdentifier as NumberIdentifier,
		null as Email,
		pn.DisplayName as DisplayName,
		pn.PhoneNumberType,
		a.Id,
		a.Zipcode,
		a.City, 
		a.LocationName,
		a.Kvhx,
		a.Latitude,
		a.Longitude,
		a.Street,
		a.Number as StreetBuildingNumber,
		a.Letter,
		a.Floor, 
		a.Door,
		a.Meters
	FROM 
		dbo.PhoneNumbers pn
	INNER JOIN 
		dbo.Addresses a  ON a.Kvhx = pn.Kvhx
	INNER JOIN 
		dbo.ProfilePositiveLists pl ON (pl.Kvhx = pn.Kvhx)
	WHERE
		a.CountryId = @CountryId AND
		pl.ProfileId = @ProfileId AND
		(@Zipcode IS NULL OR a.ZipCode = @Zipcode) AND
		(@Street IS NULL OR a.Street = @Street) AND
		(@Number IS NULL OR @Number = a.Number) AND
		(@Letter IS NULL OR @Letter = a.Letter OR (@Letter = '0' AND ISNULL(a.Letter, '') = '')) AND
		(@Floor IS NULL OR @Floor = a.Floor) AND
		(@Door IS NULL OR @Door = a.Door) AND
		(@Meters IS NULL OR @Meters = a.Meters) 

	ORDER BY 
		a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Floor, a.Door
END


END