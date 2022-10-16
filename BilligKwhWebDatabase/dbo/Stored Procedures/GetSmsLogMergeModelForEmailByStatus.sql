

CREATE PROCEDURE [dbo].[GetSmsLogMergeModelForEmailByStatus]
(
	@StatusCode INT,
	@Top int = 500
)
AS
BEGIN
    SET NOCOUNT ON

	IF @Top IS NULL OR @Top = 0
	BEGIN
		SET @Top = 500;
	END

	UPDATE TOP (@Top) sl
		SET StatusCode = 202
		OUTPUT
		inserted.ID AS Id, 
		inserted.PhoneCode AS PhoneCode,
		inserted.PhoneNumber AS Phone, 
		sg.CountryId, 
		inserted.Text, 
		sg.Message, 
		sgimf.GroupItemId,
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
		isnull(inserted.SmsSendAs, sg.SmsSendAs) AS SmsSendAs,
		sg.ID AS SmsGroupId,
		sg.CountryId AS CountryId,
		sg.[ReceiveSmsReply] AS recieveSmsReply,
		srn.[Number] AS replyNumber, 
		inserted.Email AS EmailTo, 
        c.[Name] as Company,
		c.NumberOfDaysToLockSmsReplyNumber,
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
		AND (sg.DateDelayToUtc < GETUTCDATE() OR sg.DateDelayToUtc IS NULL)
		AND sg.Active = 1
		AND sg.IsLookedUp = 1
        AND (ISNULL(sg.SendEmail, 1) = 1)
END