
CREATE PROCEDURE [dbo].[SearchSentMessages]
(
	@CountryId INT,
	@SuperAdmin INT,
	@Phone INT,
	@Email NVARCHAR(200),
	@FromDate DATETIME,
	@ToDate DATETIME,
	@ProfileIds nvarchar(300)
)
AS
BEGIN
	SELECT smslog.Name as ReceiverName,
		sg.Id AS SmsGroupId,
		CASE WHEN sg.DateDelayToUtc IS NULL THEN sg.DateCreatedUtc ELSE sg.DateDelayToUtc END AS SmsGroupSentUtc,
		p.Name AS Profile, 
		c.Name AS Customer,
		a.Zipcode AS Zipcode,
		a.City AS City,
		a.LocationName AS LocationName,
		smsStatus.Status AS Status, 
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
		CASE WHEN sg.Message <> '' and p.ProfileTypeId <> 13 then sg.Message else smslog.text end as Message
		FROM dbo.SmsLogs smslog
			INNER join dbo.SmsGroupItems sgi on sgi.Id = smslog.SmsGroupItemId
			INNER join dbo.SmsGroups sg  ON sg.ID = sgi.SmsGroupId
			INNER JOIN dbo.SmsStatuses smsStatus  on smsStatus.Id = smslog.StatusCode
			INNER JOIN dbo.Profiles p  on p.Id = smslog.ProfileId
			INNER JOIN dbo.Customers c on c.Id = p.CustomerId
			LEFT JOIN dbo.Addresses a ON a.CountryId = @CountryId  and a.Kvhx = smslog.Kvhx 
		WHERE ((ISNULL(@Phone, 0) > 0) AND (smslog.PhoneNumber = @Phone)) OR ((smslog.Email = @Email) AND (ISNULL(@Email, '') <> ''))
            AND p.CountryId = @CountryId 
			AND smslog.DateGeneratedUtc >= @FromDate 
			AND smslog.DateGeneratedUtc <= @ToDate
			AND ((@SuperAdmin = 1) OR (sg.ProfileId IN (@ProfileIds)))
		ORDER BY smslog.DateGeneratedUtc DESC

END