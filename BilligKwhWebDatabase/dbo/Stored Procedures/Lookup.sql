CREATE PROCEDURE [dbo].[Lookup]
(
	@OnlyhSmsGroupId INT = NULL
)
AS

BEGIN
	SET NOCOUNT ON;
	--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	DECLARE @SmsGroupId INT = NULL 
	SET @SmsGroupId = @OnlyhSmsGroupId
	IF (NOT @SmsGroupId IS NULL)
	BEGIN
		-- Under bearbejding i lookup service.
		UPDATE dbo.SmsGroups set DateLookupTimeUtc = GETUTCDATE() WHERE Id = @SmsGroupId
		UPDATE dbo.SmsGroupItems set StatusCode = 102 
		WHERE SmsGroupId = @SmsGroupId and StatusCode != 4  AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
	
		INSERT INTO dbo.Logs (LogLevelId,ShortMessage,FullMessage,DateCreatedUtc,Module) VALUES (20, 'Start Lookup SmsGroupId ' + CAST(@SmsGroupId AS NVARCHAR(8)),'',GETUTCDATE(),'Lookup')

		DECLARE	@ProfileId INT = NULL
		DECLARE	@CountryId INT = 1
		DECLARE @SmsSendAs NVARCHAR(20)
		DECLARE @SendSMS BIT
		DECLARE @SendEmail BIT
		DECLARE @LookupBusiness BIT 
		DECLARE @LookupPrivate BIT
		DECLARE @SendToAddress BIT
		DECLARE @SendToOwnerAddress BIT
		DECLARE @TestMode bit = 0;
		DECLARE @MessageText NVARCHAR(1600)
		DECLARE @StandardReceiverText NVARCHAR(MAX)
		DECLARE @VoiceMessageText NVARCHAR(MAX)
		DECLARE @HaveNoSendRestrictions BIT
		DECLARE @UseMunicipalityPolList BIT
		DECLARE @StatusCode_Single INT = 103 --Ændret fra 100 til 103 af Henrik 03-02-2020
		DECLARE @StatusCode_Bulk INT =	103
		DECLARE @StatusCode_Email INT = 130
		DECLARE @StatusCode_Voice INT = 500
		DECLARE @StatusCode_Voice_Error INT = 555
		DECLARE @SendVoice INT
		DECLARE @VoiceSendAs NVARCHAR(20)

		-- Hent gruppe info
		SELECT TOP 1 
			@ProfileId = ProfileId,
			@CountryId = CountryId,
			@LookupPrivate = LookupPrivate,
			@LookupBusiness = LookupBusiness,
			@SendToOwnerAddress = SendToOwner,
			@SendToAddress = SendToAddress,
			@SendEmail = SendEmail,
			@SendSMS = SendSMS,
			@TestMode = TestMode,
			@SmsSendAs = SmsSendAs,
			@MessageText = [Message],
			@StandardReceiverText = StandardReceiverText,
			@VoiceMessageText = VoiceMessage,
			@SendVoice = SendVoice,
			@VoiceSendAs = VoiceSendAs		
		FROM dbo.SmsGroups  WHERE Id = @SmsGroupId

		DECLARE @AllAddrTable dbo.LookupTableType
		DECLARE @SmsLogsTable dbo.SmsLogsTableType

		-- Hent første SmsGroupItem, bruges til AddedNumbers
		DECLARE @SmsGroupItemFirst INT = ISNULL( (SELECT TOP 1 sgi.Id FROM dbo.SmsGroupItems sgi WHERE sgi.SmsGroupId = @SmsGroupId  AND ISNULL(StatusCode,0) <> 120 ), 0)	

		--@SmsLogsPhonesInLog and @SmsLogsEmailsInLog is used for safety reasons to avoid not to insert results into SmsLogs twice (if this SP is run twice on the same GroupId) 
		DECLARE @SmsLogsPhonesInLog dbo.LookupTablePhonesInLogType

		INSERT INTO @SmsLogsPhonesInLog(Phone) 
		SELECT Distinct PhoneNumber
		FROM dbo.SmsLogs
		WHERE SmsGroupId = @SmsGroupId AND NOT PhoneNumber IS NULL

		DECLARE @SmsLogsEmailsInLog dbo.LookupTableEmailsInLogType

		INSERT INTO @SmsLogsEmailsInLog(Email) 
		SELECT Distinct Email
		FROM dbo.SmsLogs
		WHERE SmsGroupId = @SmsGroupId AND NOT Email IS NULL

		SET @HaveNoSendRestrictions = CASE WHEN EXISTS 
								(
									SELECT TOP(1) pr.Name 
									FROM dbo.ProfileRoleMappings prm 
									INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId 
									WHERE prm.ProfileId = @ProfileId AND pr.Name = 'HaveNoSendRestrictions'
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


		--Start finding addres range to send to
		IF (ISNULL(@SendSMS, 1) = 1)
		BEGIN
			--Not used only declared to parse it to SP: LookupGetAllAddress
			DECLARE @SmsGroupItemsTable dbo.SmsGroupItemsType 

			INSERT INTO @AllAddrTable (Kvhx,Kvh,Name,GroupItemId,ExternalRefId)
			EXEC [dbo].[LookupGetAllAddress] @ProfileId,@CountryId,@SmsGroupItemsTable,@SmsGroupId
		END
		
		--Denne her har jeg været nødt til at flytte ind i LookupCreateSmsLog, grundet de skal med i den samlede dubblet kontrol.
		--Split uploaded GroupItems phone/e-mails into @SmsLogsTable
		--IF ISNULL(@SendSMS, 1) = 1 OR ISNULL(@SendEmail, 1) = 1
		--BEGIN
		--	--Split uploade to GroupItems
		--	INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
		--	EXEC LookupSplitGroupItems	@SmsGroupId, @CountryId, @ProfileId, @SendSMS, @SendEmail, @SendVoice, @StatusCode_Single, @StatusCode_Voice, @StatusCode_Voice_Error, @StatusCode_Email, @MessageText,	@SmsSendAs
		--END

		-- Insert teledata 
		INSERT INTO @SmsLogsTable (ProfileId, SmsGroupItemId, Kvhx, TargetKvhx, StatusCode, PhoneCode, Phone, PersonGivenName, PersonSurname, DisplayName, BusinessIndicator, Email, Text, SmsSendAs, ExternalRefId, Details)
		EXEC LookupCreateSmsLog @ProfileId, @CountryId, @SendSMS, @SendEmail, @SendVoice, @SendToAddress, @SendToOwnerAddress, @MessageText, @StandardReceiverText, @VoiceMessageText, @SmsSendAs, @VoiceSendAs, @LookupBusiness,	@LookupPrivate,	@AllAddrTable, @SmsGroupId

		-- Gem resultater
		BEGIN TRAN
			DECLARE @Rowcount INT = 1

			SET @Rowcount = 1
			WHILE @Rowcount > 0
			BEGIN
				DELETE TOP (4900) FROM dbo.SmsLogsNoPhoneAddresses WHERE SmsGroupId = @SmsGroupId
				SET @Rowcount = @@ROWCOUNT
			END
		COMMIT TRAN

		DECLARE @id_counter INT = 0
		DECLARE @batchSize INT = 100000
		DECLARE @results INT = 1
	
		WHILE (@results > 0)
		BEGIN
			BEGIN TRAN
				-- Flush temptable to SmsLogs
				INSERT INTO dbo.SmsLogs(ProfileId,SmsGroupId,SmsGroupItemId,Kvhx,OwnerAddressKvhx, StatusCode,DateGeneratedUtc,PhoneCode,PhoneNumber, Email,Text,SmsSendAs,ExternalRefId,Name,TestMode,Details) 
				SELECT 
					ProfileId,
					@SmsGroupId,
					sl.SmsGroupItemId,
					sl.Kvhx,
					sl.OwnerKvhx,
					sl.StatusCode,
					GETUTCDATE(),
					sl.PhoneCode,
					sl.Phone,
					sl.Email,
					sl.Text,
					sl.SmsSendAs,
					sl.ExternalRefId,
					sl.DisplayName,
					@TestMode,
					sl.Details
				FROM @SmsLogsTable sl
				WHERE 
					Id > @id_counter 
					AND id <= @id_counter + @batchSize
					AND NOT ISNULL(sl.Phone,0) IN (SELECT Phone FROM @SmsLogsPhonesInLog) 
					AND NOT ISNULL(sl.Email,'') IN (SELECT Email FROM @SmsLogsEmailsInLog) 

				SET @results = @@ROWCOUNT

			COMMIT TRAN;
		
			SET @id_counter = @id_counter + @batchSize
		END

		BEGIN TRAN
			-- Gem Uden-resultat adresser, til hurtig indlæsning i statusrapporten, og bevarer historikken da poslisten og/eller roller kan ændres.
			INSERT INTO dbo.SmsLogsNoPhoneAddresses (SmsGroupId, Kvhx, DateGeneratedUtc, SmsGroupItemId)
			SELECT 
				@SmsGroupId, 
				a.Kvhx, 
				GETUTCDATE(),
				a.GroupItemId
			FROM @SmsLogsTable sl
			RIGHT JOIN @AllAddrTable a ON a.Kvhx = sl.Kvhx 
			WHERE sl.Kvhx IS NULL
			
			--AddresCounter var is used to update address count on SmsGroup
			DECLARE @AddressCounter int = (SELECT Count(DISTINCT a.Kvhx) FROM @AllAddrTable a);

			UPDATE dbo.SmsGroupItems Set SmsGroupItems.StatusCode = 450 WHERE SmsGroupId = @SmsGroupId  AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
			UPDATE dbo.SmsGroups SET DateLookupTimeUtc = GETUTCDATE(), IsLookedUp=1, AddressCount = @AddressCounter  WHERE Id = @SmsGroupId
			INSERT INTO dbo.Logs (LogLevelId,ShortMessage,FullMessage,DateCreatedUtc,Module) VALUES (20, 'Done Lookup SmsGroupId ' + CAST(@SmsGroupId AS NVARCHAR(8)),'',GETUTCDATE(),'Lookup')
		COMMIT TRAN

	END
END