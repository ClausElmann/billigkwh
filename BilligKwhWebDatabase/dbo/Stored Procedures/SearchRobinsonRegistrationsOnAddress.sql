
CREATE PROCEDURE [dbo].[SearchRobinsonRegistrationsOnAddress]
(
	@CountryId INT,
	@Zip INT,
	@Street NVARCHAR(100),
	@Number INT	
)
AS
BEGIN
SELECT 
	a.MunicipalityCode AS MunicipalityCode,
	a.Zipcode AS Zipcode,
	a.City AS City,
	a.LocationName AS LocationName,
	a.StreetCode AS StreetCode,
	a.Street AS Street,
	a.Number AS Number,
	a.Letter AS Letter,
	a.Floor AS [Floor],
	a.Door AS Door,
	a.Kvhx AS Kvhx,
	a.Meters AS Meters,
	a.CountryId AS CountryId,
	a.Latitude AS Latitude,
	a.Longitude AS Longitude,
	ro.PersonName AS [Name], 
	NULL AS Phone,
	NULL AS PhoneCode,
	'' AS Email,
	'Robinson' AS [Type]
	FROM dbo.Robinsons ro
	INNER JOIN dbo.Addresses a ON a.Kvhx = ro.Kvhx
	WHERE 
		a.CountryId = @CountryId AND
		a.Zipcode = @Zip AND
		a.Street = @Street AND
		(ISNULL(@Number, 0) = 0 OR a.Number = @Number)
	ORDER BY a.Zipcode, a.Street, a.Number, a.Letter, a.Floor, a.Door
END