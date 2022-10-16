
CREATE PROCEDURE [dbo].[GetBroadcastingMessages]
(
    -- Add the parameters for the stored procedure here
    @ProfileId int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT * FROM (
		-- Sent
		SELECT TOP (50) 
			smsGroup.Id, 
			smsGroup.ProfileId, 
			[user].[Name] AS UserName,
			smsGroup.GroupName, 
			smsGroup.[Message], 
			smsGroup.DateCreatedUtc, 
			smsGroup.DateUpdatedUtc, 
			smsGroup.DateDelayToUtc, 
			smsGroup.Active, 
			smsGroup.IsLookedUp, 
			smsGroup.SendMethod, 
			smsGroup.TestMode,
			'Sent' as BroadcastingCategory 
		FROM dbo.SmsGroups smsGroup
			LEFT JOIN dbo.Users [user] ON [user].Id = smsGroup.LastUpdatedByUserId
		WHERE ProfileId = @ProfileId AND 
		Active = 1 AND 
		(DateDelayToUtc IS NULL OR DateDelayToUtc < GETUTCDATE())
		ORDER BY smsGroup.Id DESC
		-- Planned
		UNION ALL
		SELECT TOP (50) 
			smsGroup.Id, 
			smsGroup.ProfileId, 
			[user].[Name] AS UserName,
			smsGroup.GroupName, 
			smsGroup.[Message], 
			smsGroup.DateCreatedUtc, 
			smsGroup.DateUpdatedUtc, 
			smsGroup.DateDelayToUtc, 
			smsGroup.Active, 
			smsGroup.IsLookedUp, 
			smsGroup.SendMethod, 
			smsGroup.TestMode,
			'Planned' as BroadcastingCategory 
		FROM dbo.SmsGroups smsGroup
			LEFT JOIN dbo.Users [user] ON [user].Id = smsGroup.LastUpdatedByUserId
		WHERE ProfileId = @ProfileId AND
		Active = 1 AND 
		DateDelayToUtc IS NOT NULL AND 
		DateDelayToUtc > GETUTCDATE()
		ORDER BY smsGroup.Id DESC
		UNION ALL
		-- Unfinished
		SELECT TOP (50) 
			smsGroup.Id, 
			smsGroup.ProfileId, 
			[user].[Name] AS UserName,
			smsGroup.GroupName, 
			smsGroup.[Message], 
			smsGroup.DateCreatedUtc, 
			smsGroup.DateUpdatedUtc, 
			smsGroup.DateDelayToUtc, 
			smsGroup.Active, 
			smsGroup.IsLookedUp, 
			smsGroup.SendMethod, 
			smsGroup.TestMode,
			'Unfinished' as BroadcastingCategory  
		FROM dbo.SmsGroups smsGroup
			LEFT JOIN dbo.Users [user] ON [user].Id = smsGroup.LastUpdatedByUserId
		WHERE ProfileId = @ProfileId AND
		ISNULL(Active, 0) = 0 
		AND isnull(smsGroup.ForScheduledBroadcast,0) = 0
		ORDER BY smsGroup.Id DESC
	) AS Broadcasting 
END