


CREATE PROCEDURE [dbo].[SearchPropertiesOwnedByPeopleOnAddress]
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
			aao.Kvhx as OwnerAddressKvhx,
			aao.MunicipalityCode AS OwnerMunicipalityCode,
			aao.Zipcode AS OwnerZipcode,
			aao.City AS OwnerCity,
			aao.LocationName AS OwnerLocationName,
			aao.StreetCode AS OwnerStreetCode,
			aao.Street AS OwnerStreet,
			aao.Number AS OwnerNumber,
			aao.Letter AS OwnerLetter,
			aao.Floor AS OwnerFloor,
			aao.Door AS OwnerDoor,
			aao.Kvhx AS OwnerKvhx,
			aao.Meters AS OwnerMeters,
			ao.OwnerName AS [Name],
			'' AS Email,
			'Owners_Reverse' AS [Type]
			FROM dbo.AddressOwners ao
			INNER JOIN dbo.Addresses a ON a.Kvhx = ao.Kvhx
			INNER JOIN dbo.Addresses aao ON aao.Kvhx = ao.OwnerAddressKvhx 
			WHERE 
				aao.CountryId = @CountryId AND
				aao.Zipcode = @Zip AND
				aao.Street = @Street AND
				(ISNULL(@Number, 0) = 0 OR aao.Number = @Number)
END