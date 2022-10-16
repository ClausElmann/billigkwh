
CREATE PROCEDURE [dbo].[SearchMessagesSentToAddress]
(
	@CountryId INT,
	@SuperAdmin INT,
	@Zip INT,
	@Street NVARCHAR(100),
	@Number INT,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@ProfileIds IntTableType readonly
)
AS
BEGIN
	SELECT 
		smslog.Name as ReceiverName,
		CASE WHEN sg.DateDelayToUtc IS NULL THEN sg.DateCreatedUtc ELSE sg.DateDelayToUtc END AS SmsGroupSentUtc,
		sg.Id AS SmsGroupId,
		p.Name AS Profile, 
		c.Name AS Customer,
		a.Zipcode AS Zipcode,
		a.City AS City,
		a.LocationName AS LocationName,
		smslog.StatusCode,
		smsStatus.Status AS Status,
		--smsStatus.TranslationKey AS StatusTranslationKey,
		CASE WHEN smsStatus.TranslationKey <> '' THEN smsStatus.TranslationKey ELSE 'statusCode.NoResult' END AS StatusTranslationKey,
		smslog.DateRowUpdatedUtc AS LastUpdatedUtc,
		smslog.PhoneNumber AS Phone,
		smslog.Email AS Email,
		sgi.StandardReceiverId AS StandardReceiverId,
		a.Street AS Street,
		a.Number AS Number,
		a.Letter AS Letter,
		a.[Floor] AS Floor,
		a.Door AS Door,
		a.Kvhx AS Kvhx,
		a.Meters AS Meters,
		CASE WHEN sg.Message <> '' then sg.Message else smslog.text end as Message,
		smslog.DateGeneratedUtc
		FROM dbo.SmsLogs smslog
			INNER join dbo.SmsGroupItems sgi on sgi.Id = smslog.SmsGroupItemId
			INNER join dbo.SmsGroups sg  ON sg.ID = smslog.SmsGroupId
			INNER JOIN dbo.SmsStatuses smsStatus  on smsStatus.Id = smslog.StatusCode
			INNER JOIN dbo.Profiles p on p.Id = smslog.ProfileId
			INNER JOIN dbo.Customers c on c.Id = p.CustomerId
			INNER JOIN dbo.Addresses a ON a.Kvhx = smslog.Kvhx 
		WHERE a.CountryId = @CountryId 
            AND a.Zipcode = @Zip
			AND a.Street = @Street
			AND a.Number = isnull(@Number, a.Number)
			AND smslog.DateGeneratedUtc >= @FromDate 
			AND smslog.DateGeneratedUtc <= @ToDate
			AND ((@SuperAdmin = 1) OR (sg.ProfileId IN (SELECT * FROM @ProfileIds)))
	union all
	select 
		'Ikke fundet',
		CASE WHEN sg.DateDelayToUtc IS NULL THEN sg.DateCreatedUtc ELSE sg.DateDelayToUtc END AS SmsGroupSentUtc,
		sg.Id AS SmsGroupId,
		p.Name AS Profile, 
		c.Name AS Customer,
		a.Zipcode AS Zipcode,
		a.City AS City,
		a.LocationName AS LocationName,
		450,
		'',
		'',
		smslog.DateGeneratedUtc,
		null,
		null,
		null,
		a.Street AS Street,
		a.Number AS Number,
		a.Letter AS Letter,
		a.[Floor] AS Floor,
		a.Door AS Door,
		a.Kvhx AS Kvhx,
		a.Meters AS Meters,
		sg.Message,
		smslog.DateGeneratedUtc
		FROM SmsLogsNoPhoneAddresses smslog
			INNER join dbo.SmsGroups sg  ON sg.ID = smslog.SmsGroupId
			INNER JOIN dbo.Addresses a ON a.Kvhx = smslog.Kvhx 
			INNER JOIN dbo.Profiles p on p.Id = sg.ProfileId
			INNER JOIN dbo.Customers c on c.Id = p.CustomerId
		WHERE a.CountryId = @CountryId 
            AND a.Zipcode = @Zip
			AND a.Street = @Street
			AND a.Number = isnull(@Number, a.Number)
			AND smslog.DateGeneratedUtc >= @FromDate 
			AND smslog.DateGeneratedUtc <= @ToDate
			AND ((@SuperAdmin = 1) OR (sg.ProfileId IN (SELECT * FROM @ProfileIds)))
		ORDER BY DateGeneratedUtc DESC
END