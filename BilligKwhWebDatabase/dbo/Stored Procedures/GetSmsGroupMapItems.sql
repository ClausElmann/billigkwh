
CREATE PROCEDURE [dbo].[GetSmsGroupMapItems]
(
	@SmsGroupId INT
)
AS
BEGIN
SELECT 
Addresses.Zipcode, 
addresses.City, 
Addresses.Kvh, 
isnull(Addresses.LocationName,'NULL') AS LocationName, 
Addresses.Street, 
Addresses.Number, 
Addresses.Letter, 
Addresses.Meters, 
Addresses.Latitude, 
Addresses.Longitude, 
count(*) AS NumberOfFloorDoors
  FROM [dbo].[SmsGroupItems] 
  inner join Addresses ON (
  Addresses.City = SmsGroupItems.City and 
  Addresses.Zipcode = SmsGroupItems.zip and 
  Addresses.Street = SmsGroupItems.StreetName and 
  Addresses.Number = SmsGroupItems.FromNumber and
  addresses.Letter = replace(ISNULL(SmsGroupItems.Letter, ''),'0','')
  )
  WHERE [SmsGroupId] = @SmsGroupId AND Addresses.DateDeletedUtc IS NULL
  GROUP BY Addresses.Zipcode, 
  addresses.City, 
  Addresses.Kvh, 
  ISNULL(Addresses.LocationName,'NULL'),
  Addresses.Street, 
  Addresses.Number, 
  Addresses.Letter,
  Addresses.Meters, 
  Addresses.Latitude, 
  Addresses.Longitude



END