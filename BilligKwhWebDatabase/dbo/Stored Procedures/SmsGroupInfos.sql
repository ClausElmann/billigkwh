
CREATE PROCEDURE [dbo].[SmsGroupInfos]
(
	@SmsGroupId INT = -1
)
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	DECLARE @ItemsCount INT = (SELECT COUNT(*) FROM dbo.SmsGroupItems WHERE SmsGroupId = @SmsGroupId)
	DECLARE @LogsCount INT = (SELECT COUNT(*) FROM dbo.SmsLogs WHERE SmsGroupId = @SmsGroupId)

	-- Gruppeinfo
	DECLARE	@ProfileId INT = NULL
	DECLARE	@CustomerId INT = NULL
	
	DECLARE @SendSMS BIT
	
	DECLARE @LookupBusiness BIT 
	DECLARE @LookupPrivate BIT
	DECLARE @LookupOwner BIT
	DECLARE @LookupAddress BIT
	

	-- Profil roller
	DECLARE @HaveNoSendRestrictions BIT
	DECLARE @UseMunicipalityPolList BIT

	DECLARE @DontLookUpNumbers BIT
	DECLARE @RobinsonCheck BIT
	DECLARE @NameMatch BIT
	DECLARE @DeadNumberCheck BIT
	DECLARE @BulkSMS BIT
	DECLARE @UnwireBulkSMS BIT
	DECLARE @GatewayAPI BIT
	DECLARE @GatewayAPIBulk BIT
	DECLARE @MasterGatewaySwitch BIT
	DECLARE @NoDuplicateCheck BIT
	DECLARE @OverruleBlockedNumber BIT

	DECLARE @LookupMaxNumbers SMALLINT

	SELECT TOP 1 
		@ProfileId = ProfileId,
		
		@LookupPrivate = LookupPrivate,
		@LookupBusiness = LookupBusiness,
		@LookupOwner = SendToOwner,
		@LookupAddress = SendToAddress,
		@SendSMS = SendSMS
	FROM dbo.SmsGroups  WHERE Id = @SmsGroupId

	EXEC @HaveNoSendRestrictions = [dbo].[ProfileInRole] @ProfileId, @RoleName='HaveNoSendRestrictions'				
	EXEC @UseMunicipalityPolList = [dbo].[ProfileInRole] @ProfileId, @RoleName='UseMunicipalityPolList'				

	SELECT 
		g.Active,
		g.IsLookedUp,
		@ItemsCount AS NgroupItems,
		@LogsCount AS Nlogs,
		g.DateCreatedUtc AS CreateTime,
		g.DateLookupTimeUtc AS LookupTime,
		g.DateDelayToUtc AS DelayTo,
		LEFT(g.Message, 40) AS Besked,
		g.GroupName,
		p.Name AS ProfileName,
		p.Id AS ProfileId,
		@LookupBusiness AS LookupBusiness,
		@LookupPrivate AS LookupPrivate,
		@LookupOwner AS LookupOwner,
		@LookupAddress AS LookupAddress,

		c.Name AS CustomerName,
		c.Id AS CustomerId
	FROM dbo.SmsGroups g 
	INNER JOIN dbo.Profiles p ON (g.ProfileId = p.Id)
	INNER JOIN dbo.Customers c ON (p.CustomerId = c.Id)
	WHERE g.Id = @SmsGroupId;

	-- PosListe infos
	IF (@HaveNoSendRestrictions = 1)
	BEGIN
		SELECT 'PosList: HaveNoSendRestrictions' AS PoslisteRettighed, 0
	END
	ELSE
	IF (@UseMunicipalityPolList = 1)
	BEGIN
		SELECT 'PosList: UseMunicipalityPolList' AS PoslisteRettighed, 
		plm.MunicipalityCode, m.MunicipalityName, m.Region 
		FROM dbo.ProfilePosListMunicipalityCodes plm  
		INNER JOIN dbo.Municipalities m ON plm.MunicipalityCode = m.MunicipalityCode
		WHERE plm.ProfileId = @ProfileId 

	END
	ELSE
	BEGIN
		SELECT 'PosList: Normal' AS PoslisteRettighed, (SELECT COUNT(*) FROM dbo.ProfilePositiveLists pl WHERE pl.ProfileId = @ProfileId) AS AntalAddrPoslist

		SELECT a.Zipcode, a.City, COUNT(*) AS AntalAddresser 
		FROM dbo.ProfilePositiveLists pl 
		INNER JOIN dbo.Addresses a ON pl.Kvhx = a.Kvhx
		WHERE pl.ProfileId = @ProfileId
		GROUP BY a.Zipcode, a.City
		
	END
	
	-- Standard modtagere
	SELECT sr.* 
	FROM dbo.StandardReceivers sr 
	INNER JOIN dbo.SmsGroupItems sgi ON sr.Id = sgi.StandardReceiverId
	WHERE sgi.SmsGroupId = @SmsGroupId and sgi.StandardReceiverId IS NOT null

	-- Profile roles
	SELECT pr.Name AS ProfileRole, pr.Description as Beskrivelse 
	FROM dbo.ProfileRoleMappings prm
	INNER JOIN dbo.ProfileRoles pr ON pr.Id = prm.ProfileRoleId
	WHERE prm.ProfileId = @ProfileId
	

	
END

--EXEC dbo.SmsGroupInfos 784