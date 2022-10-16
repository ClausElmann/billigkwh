CREATE PROCEDURE [dbo].[StatusReport]
(
	@SmsGroupId INT,
	@IsAdmin BIT = 1,
	@ProfileId INT = NULL,
	@CountryId INT = 1
)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	DECLARE @FFoundTable dbo.StatusReportTableType
	DECLARE @HaveNoSendRestrictions BIT
	DECLARE @UseMunicipalityPolList BIT
	DECLARE @DateLookupTimeUtc DATETIME
	DECLARE @DateStartToSendTime DATETIME
	DECLARE @CanSeePhoneAndEmail BIT

	IF (@ProfileId IS NULL)
		BEGIN
			SELECT TOP (1) @CountryId = sg.CountryId, @ProfileId = sg.ProfileId FROM dbo.SmsGroups sg WHERE sg.Id = @SmsGroupId
		END

	SELECT TOP (1) 
		@DateLookupTimeUtc = sg.DateLookupTimeUtc, 
		@DateStartToSendTime = CASE WHEN sg.DateDelayToUtc is null THEN sg.DateCreatedUtc ELSE sg.DateDelayToUtc END,
		@CanSeePhoneAndEmail = CASE WHEN (@isAdmin = 0 and (@DateStartToSendTime > GETUTCDATE())) THEN 0 ELSE 1 END
	FROM dbo.SmsGroups sg 
	WHERE sg.Id = @SmsGroupId

	EXEC @HaveNoSendRestrictions = [dbo].[ProfileInRole] @ProfileId, @RoleName='HaveNoSendRestrictions'				
	EXEC @UseMunicipalityPolList = [dbo].[ProfileInRole] @ProfileId, @RoleName='UseMunicipalityPolList'				

	-- 'Found addresses' from SmsLogs
	INSERT INTO @FFoundTable (SmsLogId,Phone,Email,StatusCode,StatusText,StartToSendTime,StatusUpdated,Kvhx,Zipcode,Street,Number,Letter,Floor,Door,Meters,Name,City,LocationName,GroupItemId,ExternalRefId,Description, Level6)
		SELECT
			sl.Id AS SmsLogId, 
			-- If you're not super admin and the message has been delayed to a datetime later than now, user is not allowed to get phone
			(CASE WHEN @CanSeePhoneAndEmail = 0 THEN NULL ELSE sl.PhoneNumber END) AS Phone,
			(CASE WHEN @CanSeePhoneAndEmail = 0 THEN NULL ELSE sl.Email END) AS Email,  
			sl.StatusCode,
			COALESCE(es.Status, ss.TranslationKey, 'statusCode.NoResult') AS StatusText,
			@DateStartToSendTime AS StartToSendTime,  
			CASE WHEN sl.DateStatusUpdatedUtc is null THEN 
				sl.DateRowUpdatedUtc 
			ELSE 
				sl.DateStatusUpdatedUtc 
			END AS StatusUpdated, 
			a.Kvhx as Kvhx,
			a.Zipcode AS Zipcode,
			a.Street AS Street,
			a.Number AS Number,
			a.Letter AS Letter,
			a.Floor AS Floor,
			a.Door AS Door,
			a.Meters AS Meters,
			sl.Name AS Name,
			a.City AS City,
			a.LocationName  AS LocationName,
			sgi.Id AS GroupItemId,
			sl.ExternalRefId AS ExternalRefId,
			CASE WHEN sgi.StandardReceiverId > 0 THEN 
				'shared.StandardReceiver'
			ELSE 
				'' 
			END,
			lc.Level6
			FROM dbo.SmsLogs sl
			INNER JOIN dbo.SmsGroupItems sgi ON sl.SmsGroupItemId = sgi.Id
			INNER JOIN dbo.SmsGroups sg ON sl.SmsGroupId = sg.Id
			LEFT JOIN dbo.Addresses a ON sl.Kvhx = a.Kvhx
			LEFT JOIN dbo.EmailStatuses es ON es.StatusCode = sl.StatusCode and sl.Email is not null
			LEFT JOIN dbo.SmsStatuses ss ON ss.Id = sl.StatusCode and sl.Email is null
			LEFT JOIN (
				dbo.ProfilePosListLevelCombinationListings lcl 
				INNER JOIN dbo.ProfilePosListLevelCombinations lc on lc.Id = lcl.LevelCombinationId
			) on sl.Kvhx = lcl.Kvhx and sg.ProfileId = lc.ProfileId
			WHERE sg.ID = @SmsGroupID AND sl.ProfileId = @ProfileId

	-- Tilføj addresser uden lookup resultat
	SELECT	u.*, 
			mf.MergeFieldName1, mf.MergeFieldValue1, 
			mf.MergeFieldName2, mf.MergeFieldValue2, 
			mf.MergeFieldName3, mf.MergeFieldValue3, 
			mf.MergeFieldName4, mf.MergeFieldValue4, 
			mf.MergeFieldName5, mf.MergeFieldValue5 
	FROM 
		(
				SELECT 
					0 AS Id,
					NULL AS Phone,
					NULL AS Email,
					0 AS StatusCode,
					'statusCode.NoResult' AS StatusText,
					NULL AS StartToSendTime,
					@DateLookupTimeUtc AS StatusUpdated,
					a.Kvhx AS AddressKvhx,
					a.Zipcode AS AddressZip,
					a.Street AS AddressStreet,
					a.Number AS AddressNumber,
					a.Letter AS AddressLetter,
					a.[Floor] AS AddressFloor,
					a.Door AS AddressDoor,
					a.Meters AS AddressMeters,
					NULL AS Name,
					a.City AS AddressCity,
					a.LocationName  AS AddressLocationName,
					noa.SmsGroupItemId GroupItemId,
					NULL ExternalRefId,
					NULL AS Description,
					lc.Level6 AS Level6
				FROM dbo.SmsLogsNoPhoneAddresses noa
				INNER JOIN dbo.SmsGroups sg on sg.Id = noa.SmsGroupId
				INNER JOIN dbo.Addresses a ON a.Kvhx = noa.Kvhx
				LEFT JOIN (
					dbo.ProfilePosListLevelCombinationListings lcl 
					INNER JOIN dbo.ProfilePosListLevelCombinations lc on lc.Id = lcl.LevelCombinationId
				) on noa.Kvhx = lcl.Kvhx and sg.ProfileId = lc.ProfileId
				WHERE noa.SmsGroupId = @SmsGroupId
			UNION
				SELECT 
					a.SmsLogId AS Id,
					a.Phone,
					a.Email,
					a.StatusCode,
					a.StatusText COLLATE Danish_Norwegian_CI_AI AS StatusText,
					a.StartToSendTime,
					a.StatusUpdated,
					a.Kvhx AS AddressKvhx,
					a.Zipcode AS AddressZip,
					a.Street AS AddressStreet,
					a.Number AS AddressNumber,
					a.Letter AS AddressLetter,
					a.Floor AS AddressFloor,
					a.Door AS AddressDoor,
					a.Meters AS AddressMeters,
					a.Name AS Name,
					a.City AS AddressCity,
					a.LocationName  AS AddressLocationName,
					a.GroupItemId,
					a.ExternalRefId,
					a.Description,
					a.Level6 collate Danish_Norwegian_CI_AI
				FROM @FFoundTable a
			UNION
				SELECT 
					0 AS Id,
					null as Phone,
					null as Email,
					240 as StatusCode,
					'statusCode.BrokenAddress' AS StatusText,
					null as StartToSendTime,
					@DateLookupTimeUtc as StatusUpdated,
					null AS AddressKvhx,
					sgi.Zip AS AddressZip,
					sgi.StreetName AS AddressStreet,
					sgi.FromNumber AS AddressNumber,
					CASE sgi.Letter WHEN '0' THEN '' ELSE sgi.Letter END AS AddressLetter,
					CASE sgi.[Floor] WHEN '0' THEN '' ELSE sgi.[Floor] END AS AddressFloor,
					CASE sgi.Door WHEN '0' THEN '' ELSE sgi.Door END  AS AddressDoor,
					sgi.Meters AS AddressMeters,
					sgi.Name AS Name,
					sgi.City AS AddressCity,
					'' AS AddressLocationName,
					sgi.Id as GroupItemId,
					sgi.ExternalRefId,
					'' as Description,
					'' as Level6
				FROM dbo.SmsGroupItems sgi
					where SmsGroupId = @SmsGroupId
						and StandardReceiverGroupId is null 
						and StandardReceiverId is null
						and NOT isnull(sgi.StatusCode,0) in (100,120) 
						and not exists (select null from dbo.SmsLogs where SmsGroupId = @SmsGroupId and SmsGroupItemId = sgi.Id)
						and not exists (select null
							from dbo.SmsLogsNoPhoneAddresses npa
							INNER JOIN dbo.Addresses a on a.Kvhx = npa.Kvhx
							where npa.SmsGroupId = sgi.SmsGroupId
								AND sgi.StreetName = a.Street
								AND ( sgi.FromNumber IS NULL OR sgi.FromNumber <= a.Number OR sgi.FromNumber = 0 AND a.Number IS NULL)
								AND ( sgi.ToNumber IS NULL OR sgi.ToNumber >= a.Number OR sgi.ToNumber = 0 AND a.Number IS NULL)
								-- letter, floor and door are all set to '0' if indicating a specific address. e.g. '0' as letter when a clean house number is specified
								AND (ISNULL(sgi.Letter, '') = '' OR sgi.Letter = a.Letter OR (sgi.Letter = '0' and a.Letter = ''))
								AND (ISNULL(sgi.[Floor], '') = '' OR sgi.[Floor] = a.[Floor] OR  sgi.[Floor] = '0' AND a.[Floor] = '')
								AND (ISNULL(sgi.Door, '') = '' OR sgi.Door = a.Door OR  sgi.Door = '0' AND a.Door = '')
								AND isnull(sgi.Meters, 0) = isnull(a.Meters, 0)
						)

			UNION
					SELECT 
						0 AS Id,
						null as Phone,
						null as Email,
						120 as StatusCode,
						'statusCode.Processing' AS StatusText,
						null as StartToSendTime,
						@DateLookupTimeUtc as StatusUpdated,
						null AS AddressKvhx,
						sgi.Zip AS AddressZip,
						sgi.StreetName AS AddressStreet,
						sgi.FromNumber AS AddressNumber,
						CASE sgi.Letter WHEN '0' THEN '' ELSE sgi.Letter END AS AddressLetter,
						CASE sgi.[Floor] WHEN '0' THEN '' ELSE sgi.[Floor] END AS AddressFloor,
						CASE sgi.Door WHEN '0' THEN '' ELSE sgi.Door END  AS AddressDoor,
						sgi.Meters AS AddressMeters,
						sgi.Name AS Name,
						sgi.City AS AddressCity,
						'' AS AddressLocationName,
						sgi.Id as GroupItemId,
						sgi.ExternalRefId,
						'' as Description,
						'' as Level6
					FROM dbo.SmsGroupItems sgi
						where SmsGroupId = @SmsGroupId 
							AND StandardReceiverGroupId IS NULL 
							AND StandardReceiverId IS NULL
							AND StatusCode = 120
		) u
	LEFT OUTER JOIN dbo.SmsGroupItemMergeFields mf ON mf.GroupItemId = u.GroupItemId
	ORDER BY u.AddressZip, u.AddressStreet, u.AddressNumber, u.AddressLetter, u.AddressFloor, u.AddressDoor, u.Name
END