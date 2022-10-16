CREATE PROCEDURE [dbo].[LookupCreateSmsLog]
(
	@ProfileId int,
	@CountryId int,
	@SendSMS bit,
	@SendEmail bit,
	@SendVoice bit,
	@SendToAddress bit,
	@SendToOwnerAddress bit,
	@MessageText nvarchar(1600), 
	@StandardReceiverText nvarchar(max) = null,
	@VoiceMessageText nvarchar(max) = null,
	@SmsSendAs nvarchar(20),
	@VoiceSendAs nvarchar(20),
	@LookupBusiness bit,
	@LookupPrivate bit,
	@AllAddrTable LookupTableType READONLY,
	@SmsGroupId bigint = null
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	DECLARE @SmsLogsTable dbo.SmsLogsTableType

	DECLARE @UseMunicipalityPolList BIT
	DECLARE @HaveNoSendRestrictions BIT
	DECLARE @DontLookUpNumbers BIT
	DECLARE	@CustomerId INT = NULL
	DECLARE @LookupMaxNumbers SMALLINT
	DECLARE @OverruleBlockedNumber BIT
	DECLARE @RobinsonCheck BIT
	DECLARE @NameMatch BIT
	DECLARE @DuplicateCheckWithKvhx BIT
	DECLARE @NewLine nvarchar(2) = '
';

	--Declare constants
	DECLARE @StatusCode_Single INT = 103 --Ændret fra 100 til 103 af Henrik 03-02-2020
	DECLARE @StatusCode_Bulk INT =	103
	DECLARE @StatusCode_Email INT = 130
	DECLARE @StatusCode_Voice INT = 500
	DECLARE @StatusCode_Voice_Error INT = 555
	DECLARE @PhoneCode INT = (CASE @CountryId WHEN 1 THEN 45 WHEN 2 THEN 46 WHEN 4 THEN 358 WHEN 5 THEN 47 ELSE 45 END)

	-- Start by removing NULL from @SmsGroupId
	SET @SmsGroupId = ISNULL(@SmsGroupId,0);

	-- Hent profil info
	SELECT TOP 1 
		@LookupMaxNumbers = LookupMaxNumbers,
		@CustomerId = CustomerId
	FROM dbo.Profiles  WHERE Id = @ProfileId

	SET @RobinsonCheck = CASE WHEN EXISTS 
						(
							SELECT TOP(1) pr.Name 
							FROM dbo.ProfileRoleMappings prm 
							INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
							WHERE prm.ProfileId = @ProfileId AND pr.Name = 'RobinsonCheck'
						) 
						THEN 1 ELSE 0 END;

	SET @UseMunicipalityPolList = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'UseMunicipalityPolList'
							) 
							THEN 1 ELSE 0 END;

	SET @HaveNoSendRestrictions = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'HaveNoSendRestrictions'
							) 
							THEN 1 ELSE 0 END;

	SET @DontLookUpNumbers = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'DontLookUpNumbers'
							) 
							THEN 1 ELSE 0 END;

	SET @OverruleBlockedNumber = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'OverruleBlockedNumber'
							) 
							THEN 1 ELSE 0 END;

	SET @NameMatch = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'NameMatch'
							) 
							THEN 1 ELSE 0 END;
	
	SET @DuplicateCheckWithKvhx = CASE WHEN EXISTS 
							(
								SELECT TOP(1) pr.Name 
								FROM dbo.ProfileRoleMappings prm 
								INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
								WHERE prm.ProfileId = @ProfileId AND pr.Name = 'DuplicateCheckWithKvhx'
							) 
							THEN 1 ELSE 0 END;


	--Split uploaded GroupItems phone/e-mails into @SmsLogsTable
	--Jeg har detsværre været nødt til at pille den kode ud af LookupSplitGroupItems og indsætte den her grundet denne fejl: INSERT EXEC statement cannot be nested
	--Man kan ikke have en nested Insert. :-(
	IF ISNULL(@SendSMS, 1) = 1 OR ISNULL(@SendEmail, 1) = 1
	BEGIN
		--Split uploade to GroupItems
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
		-- First create SmsLogs for std. receivers with a phone number
		SELECT
		@ProfileId,
		sgi.Id,
		null,
		case
			when sgi.StatusCode = 4 then 4
			else
				@StatusCode_Single 
		end As StatusCode,
		sgi.PhoneCode as PhoneCode,
		sr.Phone,
		'' as PersonGivenName,
		'' as PersonSurname,
		sr.Name,
		0 as BusinessIndicator,
		NULL As Email,
		@MessageText + case when len(@StandardReceiverText) > 0 then @NewLine + @StandardReceiverText else '' end As Text, -- No voice messages to standard receivers
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		'Standard-modtager'
		from StandardReceivers sr
			inner join StandardReceiverGroupMappings srgm on sr.Id = srgm.StandardReceiverId
			inner join SmsGroupItems sgi on sgi.StandardReceiverGroupId = srgm.StandardReceiverGroupId and sgi.StandardReceiverId is null
		where sgi.SmsGroupId = @SmsGroupId and sr.Phone is not null

		UNION ALL
		-- Create SmsLogs for std. receivers with an email
		SELECT
		@ProfileId,
		sgi.Id,
		null,
		@StatusCode_Email,
		0 as PhoneCode,
		null,
		'' as PersonGivenName,
		'' as PersonSurname,
		sr.Name,
		0 as BusinessIndicator,
		sr.Email As Email,
		@MessageText + case when len(@StandardReceiverText) > 0 then @NewLine + @StandardReceiverText else '' end As Text,  -- Ignore @VoiceMessageText because this is for email only
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		'Standard-modtager'
		from StandardReceivers sr
			inner join StandardReceiverGroupMappings srgm on sr.Id = srgm.StandardReceiverId
			inner join SmsGroupItems sgi on sgi.StandardReceiverGroupId = srgm.StandardReceiverGroupId and sgi.StandardReceiverId is null
		where sgi.SmsGroupId = @SmsGroupId and isnull(sr.Email, '') <> ''
		
		UNION ALL
		-- Create SmsLogs for group items having a specified phone number
		SELECT 
		@ProfileId, 
		sgi.Id,
		ISNULL((SELECT TOP (1) Kvhx FROM dbo.Addresses a WHERE 
				(a.Zipcode = sgi.Zip) AND
				((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName = a.Street)) AND
					(sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber OR sgi.FromNumber = 0 AND a.Number IS NULL) AND -- Some addresses have no house number which we indicate with a 0 
					(sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber OR sgi.ToNumber = 0 AND a.Number IS NULL) AND
					(sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) AND
					((ISNULL(sgi.Letter, '') = '') OR (sgi.Letter = a.Letter) OR ((sgi.Letter = '0') AND (a.Letter = ''))  ) AND
					((ISNULL(sgi.Floor, '') = '') OR (sgi.Floor = a.Floor) OR ((sgi.Floor = '0') AND (a.Floor = ''))) AND
					((ISNULL(sgi.Door, '') = '') OR (sgi.Door = a.Door) OR ((sgi.Door = '0') AND (a.Door = ''))) AND
					(sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) ), NULL) ,
		case -- Voice
			when sgi.StatusCode = 4 then 4
			when ISNULL(ph.PhoneNumberType,1) = 0 then
				case
					when @SendVoice = 1 then
						@StatusCode_Voice
					else
						@StatusCode_Voice_Error
					end
			else
				@StatusCode_Single 
		end As StatusCode,
		sgi.PhoneCode,
		sgi.Phone As Phone,
		'' As PersonGivenName,
		'' As PersonSurname,
		sgi.Name As DisplayName,
		0 As BusinessIndicator,
		NULL As Email,
		case 
			when isnull(sgi.StandardReceiverId,0) = 0 and isnull(ph.PhoneNumberType, 1) = 0 then isnull(@VoiceMessageText, @MessageText) -- No voice message for standard receivers.
			else @MessageText + case when sgi.StandardReceiverId > 0 and len(@StandardReceiverText) > 0 then @NewLine + @StandardReceiverText else '' end
		end As Text, 
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		CASE 
			WHEN ISNULL(sgi.StandardReceiverId,0) > 0 THEN 
				'Standard-modtager' 
			ELSE 
				'Medsendt nummer'  -- HVIS denne tekst laves om så husk også at ændre det stunt der er lavet i "Block number SQL" og "navne tjek", hvor disse medsendte numre er undtaget.
		END As Details
		FROM dbo.SmsGroupItems sgi 
		OUTER APPLY (SELECT TOP(1) p.PhoneNumberType FROM dbo.PhoneNumbers p WHERE p.CountryId = @CountryId AND p.NumberIdentifier = sgi.Phone) AS ph
		WHERE sgi.SmsGroupId = @SmsGroupId 
			AND sgi.Phone > 0 
			AND @SendSMS = 1 

		UNION ALL

		--Create SmsLogs for GroupItems e-mails
		SELECT 
		@ProfileId, 
		sgi.Id,
		ISNULL((SELECT TOP (1) Kvhx FROM dbo.Addresses a WHERE 
				(a.Zipcode = sgi.Zip) AND
				((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName = a.Street)) AND
					(sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber OR sgi.FromNumber = 0 AND a.Number IS NULL) AND -- Some addresses have no house number which we indicate with a 0 
					(sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber OR sgi.ToNumber = 0 AND a.Number IS NULL) AND
					(sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) AND
					((ISNULL(sgi.Letter, '') = '') OR (sgi.Letter = a.Letter) OR ((sgi.Letter = '0') AND (a.Letter = ''))  ) AND
					((ISNULL(sgi.Floor, '') = '') OR (sgi.Floor = a.Floor) OR ((sgi.Floor = '0') AND (a.Floor = ''))) AND
					((ISNULL(sgi.Door, '') = '') OR (sgi.Door = a.Door) OR ((sgi.Door = '0') AND (a.Door = ''))) AND
					(sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) ), NULL) ,

		@StatusCode_Email As StatusCode,
		sgi.PhoneCode,
		NULL As Phone,
		'' As PersonGivenName,
		'' As PersonSurname,
		sgi.Name As DisplayName,
		0 As BusinessIndicator,
		sgi.Email As Email,
		@MessageText + case when sgi.StandardReceiverId > 0 and len(@StandardReceiverText) > 0 then @NewLine + @StandardReceiverText else '' end As Text,   -- Ignore @VoiceMessageText because this is for email only
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		CASE 
			WHEN ISNULL(sgi.StandardReceiverId,0) > 0 THEN 
				'Standard-modtager' 
			ELSE 
				'Medsendt e-mail'  -- HVIS denne tekst laves om så husk også at ændre det stunt der er lavet i "Block number SQL" og "navne tjek", hvor disse medsendte numre er undtaget.
		END As Details
		FROM dbo.SmsGroupItems sgi 
		WHERE 
			sgi.SmsGroupId = @SmsGroupId  
			AND @SendEmail = 1 
			AND ISNULL(sgi.Email,'') > ''
	END


	-- Insert teledata 
	IF (@SendSMS = 1 AND @SendToAddress = 1 AND @DontLookUpNumbers = 0)
	BEGIN
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
		SELECT
		@ProfileId, 
		ISNULL(a.GroupItemId,0),
		a.Kvhx,
		a.Kvhx,
		CASE
		WHEN @OverruleBlockedNumber = 0 AND EXISTS
				(SELECT NULL 
				FROM dbo.Subscriptions 
				WHERE		CustomerId = @CustomerId
							AND (IdentifierTypeId = 1 OR SubscriptionTypeId = 1) 
							AND (Deleted = 0) 
							AND (Blocked = 1) 
							AND (PhoneNumber = ph.NumberIdentifier)
				)
		THEN 209
		ELSE @StatusCode_Bulk
		END ,
		ph.PhoneCode,
		ph.NumberIdentifier,
		ph.PersonGivenName,
		ph.PersonSurname,
		ph.DisplayName,
		ph.BusinessIndicator,
		null,
		@MessageText,   -- Ignore @VoiceMessageText because this is for mobile only
		@SmsSendAs,
		a.ExternalRefId,
		'' As Description
		FROM @AllAddrTable a
		LEFT JOIN dbo.PhoneNumbers ph ON (ph.Kvhx = a.Kvhx)
		WHERE	(a.Kvhx IS NOT NULL) 
				AND (ph.PhoneNumberType = 1) 
				AND	( 
						(@LookupBusiness = 1 AND ph.BusinessIndicator = 1) 
						OR  
						(@LookupPrivate = 1 AND ph.BusinessIndicator = 0)
					)				

		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
		SELECT
		@ProfileId, 
		ISNULL(a.GroupItemId,0),
		a.Kvhx,
		a.Kvhx,
		CASE
		WHEN @OverruleBlockedNumber = 0 AND EXISTS
				(SELECT NULL 
				FROM dbo.Subscriptions 
				WHERE		CustomerId = @CustomerId
							AND (IdentifierTypeId = 1 OR SubscriptionTypeId = 1) 
							AND (Deleted = 0) 
							AND (Blocked = 1) 
							AND (PhoneNumber = ph.NumberIdentifier)
				)
		THEN 209
		ELSE @StatusCode_Bulk
		END ,
		ph.PhoneCode,
		ph.NumberIdentifier,
		ph.PersonGivenName,
		ph.PersonSurname,
		ph.DisplayName,
		ph.BusinessIndicator,
		null,
		@MessageText,   -- Ignore @VoiceMessageText because this is for mobile only
		@SmsSendAs,
		a.ExternalRefId,
		'' As Description
		FROM @AllAddrTable a
		LEFT JOIN dbo.PhoneNumbers ph ON (ph.Kvh = a.Kvh)
		WHERE	(ph.Kvh IS NOT NULL AND ph.Kvhx IS NULL) 
				AND (ph.PhoneNumberType = 1) 
				AND	( 
						(@LookupBusiness = 1 AND ph.BusinessIndicator = 1) 
						OR  
						(@LookupPrivate = 1 AND ph.BusinessIndicator = 0)
					)
	END

	IF (@SendSMS = 1 AND @SendToAddress = 1)
	BEGIN
		-- Subscriptions - PhoneNumber
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			cs.IdentifierKey,
			cs.IdentifierKey,
			@StatusCode_Single,
			@PhoneCode,
			cs.PhoneNumber,
			dbo.Fornavn(cs.SubscriberName),
			ISNULL(cs.SubscriberName,''),
			ISNULL(cs.SubscriberName,''),
			@MessageText,   -- Ignore @VoiceMessageText because this is for mobile only
			@SmsSendAs,
			a.ExternalRefId,
			'Tilføjet nummer'
			FROM dbo.Subscriptions cs
			INNER JOIN @AllAddrTable a ON (cs.IdentifierKey = a.Kvhx 
				AND (cs.IdentifierTypeId = 1 OR cs.SubscriptionTypeId = 1) 
				AND  cs.PhoneNumberType = 1 AND 
				cs.CustomerId = @CustomerId AND 
					cs.PhoneNumber > 0 AND
					(ISNULL(cs.Blocked, 0) = 0 OR @OverruleBlockedNumber=1) AND
					ISNULL(cs.Deleted, 0) = 0 )
	END

	IF (@SendEmail = 1 AND @SendToAddress = 1)
	BEGIN
		-- Subscriptions - Email
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, Email, PersonGivenName, PersonSurname, DisplayName, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			cs.IdentifierKey,
			cs.IdentifierKey,
			@StatusCode_Email,
			cs.Email,
			dbo.Fornavn(cs.SubscriberName),
			ISNULL(cs.SubscriberName,''),
			ISNULL(cs.SubscriberName,''),
			@MessageText,   -- Ignore @VoiceMessageText because this is for email only
			@SmsSendAs,
			a.ExternalRefId,
			'Tilføjet e-mail'
			FROM dbo.Subscriptions cs
			INNER JOIN @AllAddrTable a ON (cs.IdentifierKey = a.Kvhx 
			AND (cs.IdentifierTypeId = 1 OR cs.SubscriptionTypeId = 1)  AND 
					cs.CustomerId = @CustomerId AND 
					ISNULL(cs.Email, '') > '' AND
					(ISNULL(cs.Blocked, 0) = 0 OR @OverruleBlockedNumber=1) AND
					ISNULL(cs.Deleted, 0) = 0) 
	END

	-- Insert owner 
	IF (@SendSMS = 1 AND @SendToOwnerAddress = 1 AND @DontLookUpNumbers = 0)
	BEGIN

		DECLARE @LookupMaxOwnerNumbers INT = 10 -- ISNULL(@LookupMaxNumbers, 10)  -- SET @LookupMaxOwnerNumbers = 10

		IF (@CountryId = 2)  -- Swedish model: match on 1) (Firstname and Lastname)   else 2) Firstname, like the Danish model
		BEGIN
			INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, OwnerKvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			a.Kvhx,
			ao.OwnerAddressKvhx,
			ao.OwnerAddressKvhx,
			CASE
			WHEN @OverruleBlockedNumber = 0 AND EXISTS
					(SELECT NULL 
					FROM dbo.Subscriptions 
					WHERE		CustomerId = @CustomerId
								AND (IdentifierTypeId = 1 OR SubscriptionTypeId = 1) 
								AND (Deleted = 0) 
								AND (Blocked = 1) 
								AND (PhoneNumber = ph.NumberIdentifier)
					)
			THEN 209
			WHEN ISNULL(ph.PhoneNumberType,1) = 0 then @StatusCode_Voice
			ELSE @StatusCode_Bulk
			END ,
			ph.PhoneCode,
			ph.NumberIdentifier,
			ph.PersonGivenName,
			ph.PersonSurname,
			CONCAT(ao.OwnerName, ' (Eier)'),
			ph.BusinessIndicator,
			case 
				when isnull(ph.PhoneNumberType, 1) = 0 then isnull(@VoiceMessageText, @MessageText)
				else @MessageText
			end,
			@SmsSendAs,
			a.ExternalRefId,
			'Owner'
			FROM @AllAddrTable a
			LEFT JOIN dbo.AddressOwners ao ON ao.Kvhx = a.Kvhx  
			INNER JOIN dbo.Addresses aao ON aao.Kvhx = ao.OwnerAddressKvhx 
			OUTER APPLY 
				(SELECT TOP (@LookupMaxOwnerNumbers) ph.DisplayName, ph.PhoneCode, ph.NumberIdentifier, ph.PhoneNumberType, ph.PersonGivenName,ph.PersonSurname,ph.BusinessIndicator
					FROM  dbo.PhoneNumbers ph
					WHERE ph.Kvhx = ao.OwnerAddressKvhx AND ((dbo.Efternavn(ph.DisplayName) = dbo.Efternavn(ao.OwnerName) AND dbo.Fornavn(ph.DisplayName) = dbo.Fornavn(ao.OwnerName) )  )
				) AS ph
			WHERE (a.Kvhx IS NOT null) AND (@SendVoice = 1 OR isnull(ph.PhoneNumberType, 1) = 1) AND (ph.NumberIdentifier IS NOT NULL) AND
			( (@LookupBusiness = 1 AND ph.BusinessIndicator = 1) OR  
				(@LookupPrivate = 1 AND ph.BusinessIndicator = 0))
		END
		ELSE
		BEGIN  -- Danish model : match on Firstname
			INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, OwnerKvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			a.Kvhx,
			ao.OwnerAddressKvhx,
			ao.OwnerAddressKvhx,
			CASE
			WHEN @OverruleBlockedNumber = 0 AND EXISTS
					(SELECT NULL 
					FROM dbo.Subscriptions 
					WHERE		CustomerId = @CustomerId
								AND (IdentifierTypeId = 1 OR SubscriptionTypeId = 1) 
								AND (Deleted = 0) 
								AND (Blocked = 1) 
								AND (PhoneNumber = ph.NumberIdentifier)
					)
			THEN 209
			when ISNULL(ph.PhoneNumberType,1) = 0 then @StatusCode_Voice
			ELSE @StatusCode_Bulk
			END ,
			ph.PhoneCode,
			ph.NumberIdentifier,
			ph.PersonGivenName,
			ph.PersonSurname,
			CONCAT(ao.OwnerName, ' (Ejer)'),
			ph.BusinessIndicator,
			case 
				when isnull(ph.PhoneNumberType, 1) = 0 then isnull(@VoiceMessageText, @MessageText)
				else @MessageText
			end,
			@SmsSendAs,
			a.ExternalRefId,
			'Owner'
			FROM @AllAddrTable a
			LEFT JOIN dbo.AddressOwners ao ON ao.Kvhx = a.Kvhx
			INNER JOIN dbo.Addresses aao ON aao.Kvhx = ao.OwnerAddressKvhx 
			OUTER APPLY 
				(SELECT TOP (@LookupMaxOwnerNumbers) ph.DisplayName, ph.PhoneCode, ph.NumberIdentifier, ph.PhoneNumberType, ph.PersonGivenName,ph.PersonSurname,ph.BusinessIndicator
					FROM  dbo.PhoneNumbers ph
					WHERE ph.Kvhx = ao.OwnerAddressKvhx AND dbo.Fornavn(ph.DisplayName) = dbo.Fornavn(ao.OwnerName) 
				) AS ph
			WHERE (a.Kvhx IS NOT null) AND (@SendVoice = 1 OR isnull(ph.PhoneNumberType, 1) = 1) AND (ph.NumberIdentifier IS NOT NULL) AND
			( (@LookupBusiness = 1 AND ph.BusinessIndicator = 1) OR  
				(@LookupPrivate = 1 AND ph.BusinessIndicator = 0))
		END
	END

	-- Voice
	IF (@SendVoice = 1 AND @SendToAddress = 1 AND ISNULL(@VoiceSendAs, '') > '')
	BEGIN
		IF (@DontLookUpNumbers = 0)
		BEGIN
			INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			a.Kvhx,
			a.Kvhx,
			CASE
			WHEN @OverruleBlockedNumber = 0 AND EXISTS
					(SELECT NULL 
					FROM dbo.Subscriptions 
					WHERE		CustomerId = @CustomerId
								AND (IdentifierTypeId = 1 OR SubscriptionTypeId = 1) 
								AND (Deleted = 0) 
								AND (Blocked = 1) 
								AND (PhoneNumber = ph.NumberIdentifier)
					)
			THEN 209
			ELSE @StatusCode_Voice
			END ,
			ph.PhoneCode,
			ph.NumberIdentifier,
			ph.PersonGivenName,
			ph.PersonSurname,
			ph.DisplayName,
			ph.BusinessIndicator,
			null,
			isnull(@VoiceMessageText, @MessageText), 
			@SmsSendAs,
			a.ExternalRefId,
			'' As Description
			FROM @AllAddrTable a
			INNER JOIN dbo.PhoneNumbers ph ON (ph.Kvhx = a.Kvhx)
			WHERE (ph.PhoneNumberType = 0) 
				AND -- only landline
				( 
					(@LookupBusiness = 1 AND ph.BusinessIndicator = 1) 
					OR (@LookupPrivate = 1 AND ph.BusinessIndicator = 0)
				)
		END	
	
		-- Subscriptions- VOICE - PhoneNumber
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, Text, SmsSendAs, ExternalRefId, Details)
			SELECT
			@ProfileId, 
			ISNULL(a.GroupItemId,0),
			cs.IdentifierKey,
			cs.IdentifierKey,
			@StatusCode_Voice,
			@PhoneCode,
			cs.PhoneNumber,
			dbo.Fornavn(cs.SubscriberName),
			ISNULL(cs.SubscriberName,''),
			ISNULL(cs.SubscriberName,''),
			isnull(@VoiceMessageText, @MessageText), 
			@SmsSendAs,
			a.ExternalRefId,
			'Tilføjet nummer voice'
			FROM dbo.Subscriptions cs 
			INNER JOIN @AllAddrTable a ON (cs.IdentifierKey = a.Kvhx 
											AND (cs.IdentifierTypeId = 1 OR cs.SubscriptionTypeId = 1)
											AND cs.PhoneNumberType = 0 
											AND cs.CustomerId = @CustomerId 
											AND cs.PhoneNumber > 0 
											AND	(ISNULL(cs.Blocked, 0) = 0 OR @OverruleBlockedNumber=1) 
											AND	ISNULL(cs.Deleted, 0) = 0 )
	END
		
	--Frasorteret via robinson, status 207
	IF(@RobinsonCheck = 1)
	BEGIN
		UPDATE @SmsLogsTable SET StatusCode = 207
		FROM @SmsLogsTable sl
		WHERE 
		(ISNULL(sl.Details,'') <> 'Standard-modtager') AND
		(ISNULL(sl.Details,'') <> 'Medsendt nummer') AND
		(ISNULL(sl.Details,'') <> 'Tilføjet nummer') AND
		(ISNULL(sl.Details,'') <> 'Tilføjet nummer voice') AND
		ISNULL(sl.PersonGivenName, '') > '' AND 
		sl.StatusCode in (102,103,104,105,106,107,108,110,111,444) AND
		EXISTS
		(
			SELECT TOP 1 * FROM dbo.Robinsons rob 
			WHERE 
			rob.Kvhx = sl.Kvhx AND
			(
				(CHARINDEX (dbo.Fornavn(sl.PersonGivenName), rob.PersonName) > 0) OR  -- NYT Navnetjek på Robinson
				(CHARINDEX (dbo.Fornavn(sl.DisplayName), rob.PersonName) > 0)  
			)
		)
	END

	--Frasorteret via navne tjek, status 208
	--det er ikke gyldigt at bruge en ZipStreetUserRelPosListMunicipalityCode pos. list og have navne tjek
	IF (@NameMatch = 1 AND @HaveNoSendRestrictions <> 1 AND @UseMunicipalityPolList <> 1)
	BEGIN
		UPDATE @SmsLogsTable SET StatusCode = 208
		FROM @SmsLogsTable sl
		WHERE 
			sl.Kvhx IS NOT null -- Standard-modtagere har ingen kvhx
			AND	ISNULL(sl.Details,'') <> 'Medsendt nummer' 
			AND	ISNULL(sl.Details,'') <> 'Medsendt e-mail' 
			AND	ISNULL(sl.Details,'') <> 'Tilføjet nummer' 
			AND ISNULL(sl.Details,'') <> 'Tilføjet nummer voice'
			AND	ISNULL(sl.Details,'') <> 'Tilføjet e-mail' 
			AND	NOT EXISTS
					(
						SELECT 0 FROM dbo.ProfilePositiveLists pos 
						WHERE 
						pos.ProfileId = @ProfileId 
						AND pos.Kvhx = sl.Kvhx 
						AND	(
								(
									sl.BusinessIndicator = 0 
									AND 
										(
											ISNULL(sl.PersonGivenName,'') = '' 
											OR LOWER(pos.Name) LIKE LOWER(dbo.Fornavn(sl.PersonGivenName))+'%' 
										) 
								)
								OR
								(
									sl.BusinessIndicator = 1 
									AND 
										(
											ISNULL(sl.DisplayName,'') = '' 
											OR LOWER(pos.Name) LIKE LOWER(dbo.Fornavn(sl.DisplayName))+'%' 
										)
								)
							)
					)
	END

	-- Duplicate checks
	IF (@DuplicateCheckWithKvhx < 1)
	BEGIN   
		-- Duplicate check on SMS
		UPDATE @SmsLogsTable SET StatusCode = 204
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Phone, ROW_NUMBER() OVER(PARTITION BY y.Phone ORDER BY y.Phone,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Phone, COUNT(*) AS CountOf 
											FROM @SmsLogsTable
											WHERE StatusCode in (@StatusCode_Single,@StatusCode_Bulk)
											GROUP BY Phone
											HAVING COUNT(*)>1
										) dt ON y.Phone=dt.Phone 
						WHERE y.Phone > 0
					) dt2 ON sl.Id = dt2.Id
		WHERE dt2.RowRank <> 1 

		-- Duplicate check on VOICE
		UPDATE @SmsLogsTable SET StatusCode = 204
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Phone, ROW_NUMBER() OVER(PARTITION BY y.Phone ORDER BY y.Phone,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Phone, COUNT(*) AS CountOf 
											FROM @SmsLogsTable
											WHERE StatusCode in (@StatusCode_Voice)
											GROUP BY Phone
											HAVING COUNT(*)>1
										) dt ON y.Phone=dt.Phone 
						WHERE y.Phone > 0
					) dt2 ON sl.Id = dt2.Id
		WHERE dt2.RowRank <> 1 

		-- Duplicate check on Email
		UPDATE @SmsLogsTable set StatusCode = 303
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Email, ROW_NUMBER() OVER(PARTITION BY y.Email ORDER BY y.Email,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Email, COUNT(*) AS CountOf
											FROM @SmsLogsTable
											GROUP BY Email
											HAVING COUNT(*)>1
										) dt ON y.Email=dt.Email
						WHERE ISNULL(y.Email, '') > ''
					) dt2 ON sl.Id=dt2.Id
		WHERE dt2.RowRank!=1 		
	END
	ELSE
	BEGIN
		-- Duplicate check on SMS
		UPDATE @SmsLogsTable SET StatusCode = 204
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Phone, y.Kvhx, ROW_NUMBER() OVER(PARTITION BY y.Phone, y.Kvhx ORDER BY y.Phone, y.Kvhx,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Kvhx, Phone, COUNT(*) AS CountOf 
											FROM @SmsLogsTable
											WHERE StatusCode in (@StatusCode_Single,@StatusCode_Bulk)
											GROUP BY Kvhx, Phone
											HAVING COUNT(*)>1
										) dt ON y.Phone=dt.Phone AND y.Kvhx=dt.Kvhx
						WHERE y.Phone > 0 AND y.Kvhx is not null
					) dt2 ON sl.Id = dt2.Id
		WHERE dt2.RowRank <> 1 

		-- Duplicate check on VOICE
		UPDATE @SmsLogsTable SET StatusCode = 204
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Phone, y.Kvhx, ROW_NUMBER() OVER(PARTITION BY y.Phone, y.Kvhx ORDER BY y.Phone, y.Kvhx,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Kvhx, Phone, COUNT(*) AS CountOf 
											FROM @SmsLogsTable
											WHERE StatusCode in (@StatusCode_Voice)
											GROUP BY Kvhx, Phone
											HAVING COUNT(*)>1
										) dt ON y.Phone=dt.Phone AND y.Kvhx=dt.Kvhx
						WHERE y.Phone > 0 AND y.Kvhx is not null
					) dt2 ON sl.Id = dt2.Id
		WHERE dt2.RowRank <> 1 

		-- Duplicate check on Email
		UPDATE @SmsLogsTable set StatusCode = 303
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT
						y.Id,y.Email, y.Kvhx, ROW_NUMBER() OVER(PARTITION BY y.Email, y.Kvhx ORDER BY y.Email, y.Kvhx,y.Id) AS RowRank
						FROM @SmsLogsTable y
							INNER JOIN (SELECT
											Kvhx, Email, COUNT(*) AS CountOf
											FROM @SmsLogsTable
											GROUP BY Kvhx, Email
											HAVING COUNT(*)>1
										) dt ON y.Email=dt.Email AND y.Kvhx=dt.Kvhx
						WHERE ISNULL(y.Email, '') > '' AND y.Kvhx is not null
					) dt2 ON sl.Id=dt2.Id
		WHERE dt2.RowRank!=1 
	END

	-- Maks antal numre pr adresse
	IF (ISNULL(@LookupMaxNumbers,0)>0)
	BEGIN
		UPDATE @SmsLogsTable SET StatusCode = 214
		FROM @SmsLogsTable sl
		INNER JOIN (SELECT y.Id, y.TargetKvhx, ROW_NUMBER() OVER(PARTITION BY y.TargetKvhx ORDER BY y.TargetKvhx, y.Id) AS RowRank
					FROM @SmsLogsTable y
					INNER JOIN (
									SELECT
									TargetKvhx, COUNT(*) AS CountOf
									FROM @SmsLogsTable
									GROUP BY TargetKvhx
									HAVING COUNT(*) > @LookupMaxNumbers
								) dt ON y.TargetKvhx = dt.TargetKvhx
					WHERE y.TargetKvhx IS NOT NULL -- Standard-modtagere har ingen kvhx
							AND	(ISNULL(y.Details,'') <> 'Medsendt nummer') 
							AND	(ISNULL(y.Details,'') <> 'Medsendt e-mail') 
							AND (ISNULL(y.Details,'') <> 'Tilføjet nummer') 
							AND	(ISNULL(y.Details,'') <> 'Tilføjet nummer voice') 
							AND (ISNULL(y.Details,'') <> 'Tilføjet e-mail') 
							AND NOT ISNULL(y.StatusCode,0) IN (204,207,208,209,303) --Duplicate check(204), Frasorteret via robinson(207), Frasorteret via navne tjek (208), blocked numbers (209) skal ikke tælles med og Duplicate check on Email(303).
					) dt2 ON sl.Id = dt2.Id
		WHERE (dt2.RowRank > @LookupMaxNumbers)
	END


	SELECT 
		ProfileId, 
		SmsGroupItemId, 
		Kvhx, 
		TargetKvhx As OwnerAddressKvhx, 
		StatusCode, 
		PhoneCode, 
		Phone As PhoneNumber, 
		PersonGivenName, 
		PersonSurname, 
		DisplayName  As [Name], 
		BusinessIndicator, 
		Email, 
		Text, 
		SmsSendAs, 
		ExternalRefId, 
		Details
	From @SmsLogsTable
END