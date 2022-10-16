
CREATE PROCEDURE [dbo].[GetSmsLogSelectVoiceToSendByStatus]
(
	@sDateTime NVARCHAR(40),
	@StatusCode INT
)
AS
BEGIN
    SET NOCOUNT ON
	--Top ændret af Henrik K. Madsen grundet der i koden bruges et array af SmsLogIds til at opdaterer statuses...
	--Dette kaster: The server supports a maximum of 2100 parameters 
	SELECT TOP (2000)
			sl.Id AS ID, 
			sl.PhoneCode AS PhoneCode,
			sl.PhoneNumber AS Phone, 
			dbo.MergeFieldsMessageText(
			sg.CountryId,
			sl.Text, 
			sg.Message, 
			sgimf.GroupItemId,
			@sDateTime,
			a.Street,
			a.City,
			a.Number,
			a.Letter,
			sgimf.MergeFieldName1,
			sgimf.MergeFieldValue1,
			sgimf.MergeFieldName2,
			sgimf.MergeFieldValue2,
			sgimf.MergeFieldName3,
			sgimf.MergeFieldValue3,
			sgimf.MergeFieldName4,
			sgimf.MergeFieldValue4,
			sgimf.MergeFieldName5,
			sgimf.MergeFieldValue5,
			sgi.StandardReceiverId,
			sgi.StandardReceiverGroupId) AS [Text], 
			CASE WHEN sg.VoiceSendAs IS NULL 
					THEN sl.SmsSendAs 
					ELSE sg.VoiceSendAs 
			END AS SmsSendAs,
			sg.Id AS SmsGroupId,
			sg.CountryId AS CountryId,
			sl.Email AS EmailTo, 
            c.[Name] as Company,
			sl.TestMode as TestMode,
			sl.StatusCode as StatusCode,
			sgi.StandardReceiverId AS StandardReceiverId,
			sgi.StandardReceiverGroupId as StandardReceiverGroupId,
			sl.Details AS Details
			FROM dbo.SmsLogs sl
			LEFT JOIN dbo.Addresses a ON sl.Kvhx = a.Kvhx
			INNER JOIN dbo.SmsGroupItems sgi ON sl.SmsGroupItemId = sgi.Id 
			INNER JOIN dbo.SmsGroups sg ON sgi.SmsGroupId = sg.Id 
			LEFT OUTER JOIN dbo.SmsGroupItemMergeFields sgimf ON sgimf.GroupItemId = sgi.Id
			LEFT OUTER JOIN  dbo.Profiles p ON p.Id = sl.ProfileId
			LEFT OUTER JOIN dbo.Customers c ON c.Id = p.CustomerId 
			WHERE (@StatusCode = sl.StatusCode)
			AND (sg.DateDelayToUtc IS NULL OR sg.DateDelayToUtc < GETUTCDATE() )
			AND sg.Active = 1 
			AND sg.IsLookedUp = 1
			--AND (ISNULL(sg.SendVoice, 1) = 1) 
END