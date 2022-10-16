
CREATE PROCEDURE [dbo].[GetSmsLogMergeModelForSmsByStatus]
(
	@StatusCode int,
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

	declare @NextStatusCode int

	IF @StatusCode = 103 -- Ready for gateway
	BEGIN
		SET @NextStatusCode = 202
	END
	ELSE IF @StatusCode = 1212 -- Ready for 1st retry with gateway
	BEGIN
		SET @NextStatusCode = 231
	END
	ELSE IF @StatusCode = 1213 -- Ready for 2nd retry with gateway
	BEGIN
		SET @NextStatusCode = 232
	END

	UPDATE TOP (@Top) sl
		SET StatusCode = @NextStatusCode
		OUTPUT
			inserted.Id AS Id, 
			inserted.PhoneCode AS PhoneCode,
			inserted.PhoneNumber AS Phone,
			c.Id AS CustomerId,
			c.NumberOfDaysToLockSmsReplyNumber, 
			c.ForwardingNumber AS ForwardingNumber,
			isnull(inserted.Text, sg.Message) AS [Text],
			sgimf.GroupItemId,
			a.Street,
			a.City,
			a.Number,
			a.Letter,
			a.Meters,
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
			isnull(inserted.SmsSendAs, sg.SmsSendAs) AS SmsSendAs,
			sg.ID AS SmsGroupId,
			sg.CountryId AS CountryId,
			sg.[ReceiveSmsReply] AS RecieveSmsReply,
			srn.[Number] AS ReplyNumber, 
			inserted.Email AS EmailTo, 
            c.[Name] as Company,
			inserted.TestMode as TestMode,
			inserted.StatusCode as StatusCode,
			sgi.StandardReceiverId AS StandardReceiverId,
			sgi.StandardReceiverGroupId as StandardReceiverGroupId,
			inserted.ProfileId
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