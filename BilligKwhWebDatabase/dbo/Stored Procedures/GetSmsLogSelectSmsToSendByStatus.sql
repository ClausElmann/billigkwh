
CREATE PROCEDURE [dbo].[GetSmsLogSelectSmsToSendByStatus]
(
	@sDateTime NVARCHAR(40),
	@StatusCode INT,
	@Top int = 2000
)
AS
BEGIN
    SET NOCOUNT ON

	-- Grænsen på 15000 må ikke forhøjes da filerne til GatewayApi ikke må overstige 30 MB
	-- Se BI 1063  (Backlog Item 1063 i Azure Devops).
	
	IF @Top IS NULL OR @Top = 0
	BEGIN
		SET @Top = 2000;
	END

	SELECT TOP (@Top)
			sl.ID AS ID, 
			sl.PhoneCode AS PhoneCode,
			sl.PhoneNumber AS Phone,
			c.Id AS CustomerId,
			c.NumberOfDaysToLockSmsReplyNumber, 
			c.ForwardingNumber AS ForwardingNumber,
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
			isnull(sl.SmsSendAs, sg.SmsSendAs) AS SmsSendAs,
			sg.ID AS SmsGroupId,
			sg.CountryId AS CountryId,
			sg.[ReceiveSmsReply] AS recieveSmsReply,
			srn.[Number] AS replyNumber, 
			sl.Email AS EmailTo, 
            c.[Name] as Company,
			sl.TestMode as TestMode,
			sl.StatusCode as StatusCode,
			sgi.StandardReceiverId AS StandardReceiverId,
			sgi.StandardReceiverGroupId as StandardReceiverGroupId 
			FROM dbo.SmsLogs sl
			LEFT JOIN dbo.Addresses a ON sl.Kvhx = a.Kvhx
			INNER JOIN dbo.SmsGroupItems sgi ON sl.SmsGroupItemId = sgi.Id 
			INNER JOIN dbo.SmsGroups sg ON sgi.SmsGroupId = sg.ID 
			LEFT OUTER JOIN dbo.SmsGroupItemMergeFields sgimf ON sgimf.GroupItemId = sgi.Id
			LEFT OUTER JOIN  dbo.Profiles p ON p.Id = sl.ProfileId
			LEFT OUTER JOIN dbo.Customers c ON c.Id = p.CustomerId
			LEFT OUTER JOIN dbo.SmsReplyNumbers srn ON srn.SmsGroupId = sg.Id 
			WHERE (@StatusCode = sl.StatusCode)
			AND (sg.DateDelayToUtc IS NULL OR sg.DateDelayToUtc < GETUTCDATE() )
			AND sg.Active = 1 
			AND sg.IsLookedUp = 1
            AND (ISNULL(sg.SendSMS, 1) = 1)
END