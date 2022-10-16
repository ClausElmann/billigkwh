CREATE PROCEDURE [dbo].[SearchContactsOnAddress]
(
    @CountryId INT,
    @CustomerId INT,
    @Zip INT,
    @Street NVARCHAR(100),
    @Number INT,
    @DontLookupNumbers BIT
)
AS
BEGIN
 -- First select from teledata (e.i. PhoneNumbers table)
    SELECT a.MunicipalityCode AS MunicipalityCode,
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
           ph.DisplayName AS [Name],
           ph.NumberIdentifier AS Phone,
		   ph.PhoneNumberType,
           ph.PhoneCode AS PhoneCode,
           '' AS Email,
           '' AS [Type],
		   NULL AS SubscriptionId
    FROM dbo.PhoneNumbers ph
        INNER JOIN dbo.Addresses a ON ph.Kvhx = a.Kvhx
		LEFT OUTER JOIN dbo.Subscriptions sub 
            ON (sub.IdentifierTypeId = 1 OR sub.SubscriptionTypeId = 1) 
			    AND sub.IdentifierKey = a.Kvhx 
			    AND sub.PhoneNumber = ph.NumberIdentifier 
			    AND sub.Blocked = 1 AND sub.Deleted = 0 
                AND sub.CustomerId = @CustomerId
    WHERE (ph.Kvhx IS NOT NULL) 
          AND a.CountryId = @CountryId
          AND a.Zipcode = @Zip
          AND a.Street = @Street
          AND
          (
              ISNULL(@Number, 0) = 0
              OR a.Number = @Number
          )
          AND @DontLookupNumbers = 0
		  AND Sub.Blocked IS NULL
	-- Union all subscriptions being phone numbers or emails
    UNION
    SELECT a.MunicipalityCode AS MunicipalityCode,
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
           an.SubscriberName AS [Name],
           an.PhoneNumber AS Phone,
		   an.PhoneNumberType,
           CASE a.CountryId
               WHEN 2 THEN
                   46
               ELSE
                   45
           END AS PhoneCode,
           an.Email,
		   CASE WHEN NOT an.Email IS NULL AND an.Email <> '' THEN
		    'AddedEmail'
		   ELSE
			'AddedNumber'
		   END AS [Type],
		   an.Id AS SubscriptionId
    FROM dbo.Subscriptions an
        INNER JOIN dbo.Addresses a ON an.IdentifierKey = a.Kvhx
    WHERE (an.IdentifierTypeId = 1 OR an.SubscriptionTypeId = 1)
          AND an.Blocked = 0
		  AND an.Deleted = 0
          AND a.CountryId = @CountryId
          AND a.Zipcode = @Zip
          AND a.Street = @Street
          AND
          (
              ISNULL(@Number, 0) = 0
              OR a.Number = @Number
          )
          AND
          (
              ISNULL(@CustomerId, 0) = 0
              OR an.CustomerId = @CustomerId
          )
    ORDER BY a.Zipcode,
             a.Street,
             a.Number,
             a.Letter,
             a.Floor,
             a.Door;
END;