-- =============================================
-- Author:      Henrik K. Madsen
-- Create Date: 08-06-2020
-- Description: Bruges til at opsplitte GroupItems phonenumer og e-mail felter så de passer ind i SmsLogs
-- =============================================
CREATE PROCEDURE [dbo].[LookupSplitGroupItems]
(
	@SmsGroupId bigint,
	@CountryId int,
	@ProfileId int,
	@SendSMS bit,
	@SendEmail bit,
	@SendVoice bit,
	@StatusCode_Single int,
	@StatusCode_Voice int,
	@StatusCode_Voice_Error int,
	@StatusCode_Email int,
	@MessageText nvarchar(1600),
	@SmsSendAs nvarchar(20)
)

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	-- Create seperate sms and email in SmsLogs from e-mails and phonenumbers added to GroupItems
			--Create SMS-logs from GroupItems phonenumbers
		SELECT 
		@ProfileId, 
		sgi.Id,
		ISNULL((SELECT TOP (1) Kvhx FROM dbo.Addresses a WHERE 
				(a.Zipcode = sgi.Zip) AND
				((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName = a.Street)) AND
					(sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber) AND
					(sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber) AND
					(sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) AND
					((ISNULL(sgi.Letter, '') = '') OR (sgi.Letter = a.Letter) OR ((sgi.Letter = '0') AND (a.Letter = ''))  ) AND
					((ISNULL(sgi.Floor, '') = '') OR (sgi.Floor = a.Floor)) AND
					((ISNULL(sgi.Door, '') = '') OR (sgi.Door = a.Door)) AND
					(sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) ), NULL) ,
		case -- Voice
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
		@MessageText As Text, 
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		CASE 
			WHEN ISNULL(sgi.StandardReceiverId,0) > 0 THEN 
				'Standard-modtager' 
			ELSE 
				CASE 
					WHEN (NOT sgi.Phone Is NULL AND sgi.Phone <> '')	THEN 
						'Medsendt nummer'  --HVIS denne teskt laves om så husk også at ændre det stunt der er lavet i "Block number SQL" og "navne tjek" , hvor disse medsendte numre er untaget.
					ELSE 
						''
				END 
		END As Details
		FROM dbo.SmsGroupItems sgi 
		OUTER APPLY (SELECT TOP(1) p.PhoneNumberType FROM dbo.Phonenumbers p WHERE p.CountryId = @CountryId AND p.NumberIdentifier = sgi.Phone) AS ph
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
					(sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber) AND
					(sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber) AND
					(sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) AND
					((ISNULL(sgi.Letter, '') = '') OR (sgi.Letter = a.Letter) OR ((sgi.Letter = '0') AND (a.Letter = ''))  ) AND
					((ISNULL(sgi.Floor, '') = '') OR (sgi.Floor = a.Floor)) AND
					((ISNULL(sgi.Door, '') = '') OR (sgi.Door = a.Door)) AND
					(sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) ), NULL) ,

		@StatusCode_Email As StatusCode,
		sgi.PhoneCode,
		NULL As Phone,
		'' As PersonGivenName,
		'' As PersonSurname,
		sgi.Name As DisplayName,
		0 As BusinessIndicator,
		sgi.Email As Email,
		@MessageText As Text, 
		@SmsSendAs As SmsSendAs,
		sgi.ExternalRefId,
		'Medsendt e-mail' As Details
		FROM dbo.SmsGroupItems sgi 
		WHERE 
			sgi.SmsGroupId = @SmsGroupId  
			AND @SendEmail = 1 
			AND ISNULL(sgi.Email,'') > ''
END