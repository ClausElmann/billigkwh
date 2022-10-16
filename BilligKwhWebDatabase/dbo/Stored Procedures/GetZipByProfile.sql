
CREATE PROCEDURE [dbo].[GetZipByProfile]
(
	@CountryId INT,
	@ProfileId INT,
	@UseMunicipalityPolList BIT,
	@ZipCode INT = NULL,
	@City  NVARCHAR(100) = NULL,  --just to avoid failing parameter check
	@Street NVARCHAR(100) = NULL, --just to avoid failing parameter check
	@FromNumber INT = NULL,
	@ToNumber INT = NULL,
	@EvenOdd BIT = NULL,
	@Search VARCHAR(200)
)
AS
BEGIN

--Ja det ser sjovt ud men det øger performance markant. By Henrik K. Madsen
DECLARE @LOCAL_Search VARCHAR(200)
SELECT  @LOCAL_Search = @Search 
 
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	IF (ISNULL(@UseMunicipalityPolList, 0) = 0)
	BEGIN
		IF (@LOCAL_Search IS NULL)
		BEGIN
			SELECT DISTINCT a.Zipcode, a.City, a.Street
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePositiveLists pl ON a.Kvhx = pl.Kvhx
			WHERE (a.CountryId = @CountryId) AND (pl.ProfileId = @ProfileId)
			AND ((@ZipCode IS NULL) OR (a.Zipcode = @ZipCode))
			AND ((@EvenOdd IS NULL) OR (a.Number IS NULL) OR (@EvenOdd = 1 AND a.Number % 2 = 0) OR (@EvenOdd = 0 AND a.Number % 2 = 1))
			AND ((@FromNumber IS NULL) OR (a.Number >= @FromNumber))
			AND ((@ToNumber IS NULL) OR (a.Number <= @ToNumber))
			ORDER BY a.Zipcode, a.Street
		END
		ELSE
		BEGIN
		SET @LOCAL_Search = @LOCAL_Search + '%'
		SELECT DISTINCT a.Zipcode, a.City, a.Street
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePositiveLists pl ON a.Kvhx = pl.Kvhx
			WHERE (a.CountryId = @CountryId) AND (pl.ProfileId = @ProfileId)
			AND ((@ZipCode IS NULL) OR (a.Zipcode = @ZipCode))
			AND ((@EvenOdd IS NULL) OR (a.Number IS NULL) OR (@EvenOdd = 1 AND a.Number % 2 = 0) OR (@EvenOdd = 0 AND a.Number % 2 = 1))
			AND ((@FromNumber IS NULL) OR (a.Number >= @FromNumber))
			AND ((@ToNumber IS NULL) OR (a.Number <= @ToNumber))
			--AND (FREETEXT(a.Street, @LOCAL_Search))
			AND (a.Street LIKE @LOCAL_Search)
			ORDER BY a.Zipcode, a.Street
		END
	END
	ELSE  -- Mun. poslist
	BEGIN
		IF (@LOCAL_Search IS NULL)
		BEGIN

			SELECT DISTINCT a.Zipcode, a.City, a.Street
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePosListMunicipalityCodes pmc ON a.MunicipalityCode = pmc.MunicipalityCode
			WHERE (a.CountryId = @CountryId) AND (pmc.ProfileId = @ProfileId)
			AND ((@ZipCode IS NULL) OR (a.Zipcode = @ZipCode))
			AND ((@EvenOdd IS NULL) OR (a.Number IS NULL) OR (@EvenOdd = 1 AND a.Number % 2 = 0) OR (@EvenOdd = 0 AND a.Number % 2 = 1))
			AND ((@FromNumber IS NULL) OR (a.Number >= @FromNumber))
			AND ((@ToNumber IS NULL) OR (a.Number <= @ToNumber))
			ORDER BY a.Zipcode, a.Street
		END
		ELSE
		BEGIN
			SELECT a.Zipcode, a.City, a.Street
			FROM dbo.Addresses a
			INNER JOIN dbo.ProfilePosListMunicipalityCodes pmc ON (pmc.ProfileId = @ProfileId) AND a.MunicipalityCode = pmc.MunicipalityCode 
			WHERE (a.CountryId = @CountryId) 
			AND ((@ZipCode IS NULL) OR (a.Zipcode = @ZipCode))
			AND a.Street LIKE  @LOCAL_Search + '%'
			AND (
					@EvenOdd IS NULL 
					OR a.Number IS NULL 
					OR (@EvenOdd = 1 AND a.Number % 2 = 0) 
					OR (@EvenOdd = 0 AND a.Number % 2 = 1)
				)
			AND (
					@FromNumber IS NULL 
					OR a.Number >= @FromNumber
				)
			AND 
				(
					@ToNumber IS NULL  
					OR a.Number <= @ToNumber
				)
			Group by a.Zipcode, a.City, a.Street
			ORDER BY a.Zipcode, a.Street
		END
	END
END