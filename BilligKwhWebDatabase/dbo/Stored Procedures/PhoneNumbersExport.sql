

CREATE PROCEDURE [dbo].[PhoneNumbersExport]
(
	@CountryId INT,
	@MunicipalityCode SMALLINT
)
AS

SELECT	PhoneNumbers.SubscriberId, PhoneNumbers.NumberIdentifier AS DanishTelephoneNumberIdentifier, 
		PhoneNumbers.PersonGivenName, PhoneNumbers.PersonSurname, PhoneNumbers.DisplayName, 
        Addresses.Street AS StreetName, 
        Addresses.Number AS StreetBuildingNumber, 
        Addresses.Letter AS StreetBuildingLetter, 
        Addresses.Floor AS FloorIdentifier, 
        Addresses.Door AS SuiteIdentifierDirection, 
		Addresses.Door AS SuiteIdentifierDoor, 
		'' AS DistrictSubdivisionIdentifier,
        Addresses.Zipcode AS PostCodeIdentifier, 
		Addresses.City AS DistrictName, 
        Addresses.MunicipalityCode AS MunicipalCode, 
		PhoneNumbers.BusinessIndicator AS BusinessIndicator, 
		Addresses.Kvhx, 
        Addresses.StreetCode AS StreetCode
FROM            dbo.PhoneNumbers AS PhoneNumbers INNER JOIN
                dbo.Addresses AS Addresses ON PhoneNumbers.Kvhx = Addresses.Kvhx
WHERE
				Addresses.CountryId = @CountryId AND
				PhoneNumbers.CountryId = @CountryId AND
				Addresses.MunicipalityCode = @MunicipalityCode AND
				PhoneNumbers.MunicipalityCode = @MunicipalityCode AND
				Addresses.Kvhx IS NOT NULL AND
				PhoneNumbers.PhoneNumberType = 1 AND
				PhoneNumbers.NumberIdentifier < 2147483647

-- Exec PhoneNumbersExport 1, 751

--SELECT COUNT(*) FROM dbo.PhoneNumbers WHERE MunicipalityCode=751 AND (kvhx IS NOT NULL OR kvh IS NOT NULL)