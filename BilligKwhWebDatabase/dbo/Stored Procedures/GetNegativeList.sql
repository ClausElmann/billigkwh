
CREATE PROCEDURE [dbo].[GetNegativeList]
(
	@CountryId INT,
	@munCodes dbo.MunCodesTableType READONLY,
	@ProfileId INT = NULL
)
AS
BEGIN
   
SET NOCOUNT ON


SELECT a.Id,
 a.MunicipalityCode, 
a.Zipcode, 
a.City, a.LocationName, a.Street, a.Number, a.Letter, a.Floor, a.Door, a.Meters, a.Kvhx, p.Kvhx
FROM dbo.Addresses a 
	LEFT JOIN dbo.ProfilePositiveLists p ON p.Kvhx = a.Kvhx AND p.ProfileId = @ProfileId
WHERE 
	 p.Id IS NULL	 
	 AND a.CountryId = @CountryId
	 AND a.MunicipalityCode IN (SELECT * FROM @munCodes)
ORDER BY a.Zipcode, a.City, a.Street, a.Number, a.Letter, a.Floor, a.Door

END