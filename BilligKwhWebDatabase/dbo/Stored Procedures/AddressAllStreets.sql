 
CREATE PROCEDURE [dbo].[AddressAllStreets]
(
	@CountryId INT,
	@ZipCode INT = NULL
)
AS
BEGIN
SET NOCOUNT ON;

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

-- Select all street names in a zipcode in a country along with information about whether the street has any house numbers or not (some addresses in Sweden do not have house number)
SELECT 
	CountryId, 
	t1.MunicipalityCode, 
	t1.Zipcode, 
	t1.StreetName,
	CASE WHEN Numbers IS NULL THEN 0 ELSE 1 END AS HasNumbers
FROM (
SELECT CountryId,
       MunicipalityCode,
       Zipcode,
       Street AS StreetName,
	   SUM(Number) AS Numbers
FROM dbo.Addresses
WHERE CountryId = @CountryId AND 
      Zipcode = @Zipcode
GROUP BY CountryId,
         MunicipalityCode,
         Zipcode,
         Street) AS t1
ORDER BY Zipcode, StreetName;


END