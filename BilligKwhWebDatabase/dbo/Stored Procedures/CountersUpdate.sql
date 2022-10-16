CREATE PROCEDURE [dbo].[CountersUpdate]
(
	@CountryId INT,
	@CounterName NVARCHAR(50)
)
AS
BEGIN

	IF (@CounterName = 'Addresses')
	BEGIN
		UPDATE dbo.Counters SET Counter = (SELECT COUNT(*) FROM dbo.Addresses WHERE CountryId = @CountryId), DateLastUpdatedUtc = GETUTCDATE() 
		WHERE Counters.CountryId = @CountryId AND [Name] = @CounterName
	END
	ELSE IF (@CounterName = 'MobileNumbers')
	BEGIN
		UPDATE dbo.Counters SET Counter = (SELECT COUNT(*) FROM dbo.PhoneNumbers WHERE CountryId = @CountryId AND PhoneNumberType = 1), DateLastUpdatedUtc = GETUTCDATE()  
		WHERE Counters.CountryId = @CountryId AND [Name] = @CounterName
	END
	ELSE IF (@CounterName = 'LandlineNumbers')
	BEGIN
		UPDATE dbo.Counters SET Counter = (SELECT COUNT(*) FROM dbo.PhoneNumbers WHERE CountryId = @CountryId AND PhoneNumberType = 0), DateLastUpdatedUtc = GETUTCDATE()  
		WHERE Counters.CountryId = @CountryId AND [Name] = @CounterName
	END
	ELSE IF (@CounterName = 'AddressOwners')
	BEGIN
		UPDATE dbo.Counters SET Counter = (SELECT COUNT(*) FROM dbo.AddressOwners WHERE CountryId = @CountryId), DateLastUpdatedUtc = GETUTCDATE()  
		WHERE Counters.CountryId = @CountryId AND [Name] = @CounterName
	END




END