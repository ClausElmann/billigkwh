CREATE PROCEDURE [dbo].[LookupGetAllAddress]
(
	@ProfileId int,
	@CountryId int,
	@SmsGroupItems SmsGroupItemsType READONLY,
	@SmsGroupId bigint = null,
	@IgnorePhoneAndEmails bit = 0
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	DECLARE @UseMunicipalityPosList BIT
	DECLARE @HaveNoSendRestrictions BIT

	-- Start by removing NULL from @SmsGroupId
	SET @SmsGroupId = ISNULL(@SmsGroupId,0);

	SET @UseMunicipalityPosList = CASE WHEN EXISTS 
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

	IF (EXISTS (SELECT NULL FROM dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId))
	BEGIN
		-- Cannot come from @SmsGroupItems (except if there are standard receivers)

		DECLARE @LevelGroupItemId int = 0;
		select TOP (1) @LevelGroupItemId = Id from dbo.SmsGroupItems 
		where SmsGroupId = @SmsGroupId and StandardReceiverId is null AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
		order by Id
		
		if (@LevelGroupItemId = 0)
		begin
			insert into dbo.SmsGroupItems (SmsGroupId)
				values (@SmsGroupId)

			select @LevelGroupItemId = SCOPE_IDENTITY()
		end

		-- Save which levels are used in temp variables
		SET NOCOUNT ON
		DECLARE @AnyLevelUsed BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId);
		DECLARE @Level1Used BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 1);
		DECLARE @Level2Used BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 2);
		DECLARE @Level3Used BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 3);
		DECLARE @Level4Used BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 4);
		DECLARE @Level5Used BIT = (select count(*) from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 5);
		SET NOCOUNT OFF

		-- Extract LevelCombinations in temp table
		SELECT *
		INTO #templevelcomb
		FROM dbo.ProfilePosListLevelCombinations
			where ProfileId = @ProfileId
				and @AnyLevelUsed=1
				and (@Level1Used=0 OR (@Level1Used=1 AND Level1 in (select [Value] from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 1)))
				and (@Level2Used=0 OR (@Level2Used=1 AND Level2 in (select [Value] from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 2)))
				and (@Level3Used=0 OR (@Level3Used=1 AND Level3 in (select [Value] from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 3)))
				and (@Level4Used=0 OR (@Level4Used=1 AND Level4 in (select [Value] from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 4)))
				and (@Level5Used=0 OR (@Level5Used=1 AND Level5 in (select [Value] from dbo.SmsGroupLevelFilters where SmsGroupId = @SmsGroupId and Level = 5)))

		select DISTINCT
				a.Kvhx, 	
				a.Kvh,
				null, 
				@LevelGroupItemId AS GroupItemId, 
				null
			FROM dbo.Addresses a 
			INNER JOIN ProfilePosListLevelCombinationListings l on a.Kvhx = l.Kvhx 
			WHERE l.LevelCombinationId in (
				SELECT Id FROM #templevelcomb
			)
	END
	ELSE
		BEGIN			
			IF (@HaveNoSendRestrictions = 1)
				--Ingen posliste
				BEGIN
					WITH sgi as (
						select * from @SmsGroupItems
						union all
						select * from dbo.SmsGroupItems where SmsGroupId = @SmsGroupId AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
					)
					SELECT DISTINCT
						a.Kvhx, 	
						a.Kvh,
						sgi.[Name], 
						sgi.Id AS GroupItemId, 
						sgi.ExternalRefId
					FROM dbo.Addresses a 
					INNER JOIN sgi ON a.Zipcode = sgi.Zip AND a.CountryId = @CountryId  
					AND ((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName  = a.Street)) 
					AND ( sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber OR sgi.FromNumber = 0 AND a.Number IS NULL) -- Some addresses have no house number which we indicate with a 0 
					AND (sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber OR sgi.ToNumber = 0 AND a.Number IS NULL) 
					AND (sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) 
					-- value '0' for letter, floor or door means that only an address without that value is included
				   	AND ((ISNULL(sgi.Letter, '') = '') OR sgi.Letter = a.Letter OR (sgi.Letter = '0' AND a.Letter = '')) 
					AND ((ISNULL(sgi.[Floor], '') = '') OR sgi.[Floor] = a.[Floor] OR (sgi.[Floor]= '0' AND a.[Floor] = '')) 
					AND ((ISNULL(sgi.Door, '') = '') OR sgi.Door = a.Door OR (sgi.Door = '0' AND a.Door = ''))	 
					AND (sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) 
					WHERE @IgnorePhoneAndEmails = 1 or (sgi.Email is null and sgi.Phone is null)
				END
			ELSE
				BEGIN
				IF (@UseMunicipalityPosList = 1)  
					--Kommune posliste
					BEGIN
						WITH sgi as (
							select * from @SmsGroupItems
							union all
							select * from dbo.SmsGroupItems where SmsGroupId = @SmsGroupId AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
						)
						SELECT DISTINCT
							a.Kvhx,
							a.Kvh, 	
							sgi.[Name], 
							sgi.Id AS GroupItemId, 
							sgi.ExternalRefId	
						FROM dbo.Addresses a 
						INNER JOIN dbo.ProfilePosListMunicipalityCodes pmc
							ON a.CountryId = @CountryId AND a.MunicipalityCode = pmc.MunicipalityCode AND pmc.ProfileId = @ProfileId 
						INNER JOIN sgi ON a.Zipcode = sgi.Zip 
							AND ((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName = a.Street)) 
							AND ( sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber OR (sgi.FromNumber = 0 AND a.Number IS NULL)) -- Some addresses have no house number which we indicate with a 0 
							AND ( sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber OR (sgi.ToNumber = 0 AND a.Number IS NULL)) 
							AND (sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0)) 
							-- value '0' for letter, floor or door means that only an address without that value is included
							AND ((ISNULL(sgi.Letter, '') = '') OR sgi.Letter = a.Letter OR (sgi.Letter = '0' AND a.Letter = '')) 
							AND ((ISNULL(sgi.[Floor], '') = '') OR sgi.[Floor] = a.[Floor] OR (sgi.[Floor]= '0' AND a.[Floor] = '')) 
							AND ((ISNULL(sgi.Door, '') = '') OR sgi.Door = a.Door OR (sgi.Door = '0' AND a.Door = ''))	 
							AND (sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) 
						WHERE @IgnorePhoneAndEmails = 1 or (sgi.Email is null and sgi.Phone is null)
					END
				ELSE
					--Almindelig pos.liste
					BEGIN
						WITH sgi as (
							select * from @SmsGroupItems
							union all
							select * from dbo.SmsGroupItems where SmsGroupId = @SmsGroupId AND ISNULL(StatusCode,0) <> 120 -- 120: Afventer phone lookup
						)
						SELECT DISTINCT
							a.Kvhx, 	
							a.Kvh,
							sgi.[Name], 
							sgi.Id AS GroupItemId, 
							sgi.ExternalRefId
						FROM dbo.ProfilePositiveLists pl 
						INNER JOIN dbo.Addresses a ON (pl.ProfileId = @ProfileId AND a.Kvhx = pl.Kvhx)
						INNER JOIN sgi ON a.Zipcode = sgi.Zip AND
						((ISNULL(sgi.StreetName, '') = '') OR (sgi.StreetName = a.Street)) 
							AND ( sgi.FromNumber IS NULL OR a.Number >= sgi.FromNumber OR sgi.FromNumber = 0 AND a.Number IS NULL) -- Some addresses have no house number which we indicate with a 0 
							AND (sgi.ToNumber IS NULL OR a.Number <= sgi.ToNumber OR sgi.ToNumber = 0 AND a.Number IS NULL) 
							AND (sgi.EvenOdd IS NULL OR (sgi.EvenOdd = 0 AND a.Number % 2 = 1) OR (sgi.EvenOdd = 1 AND a.Number % 2 = 0))
							AND ((ISNULL(sgi.Letter, '') = '') OR sgi.Letter = a.Letter OR (sgi.Letter = '0' AND a.Letter = '')) 
							AND ((ISNULL(sgi.[Floor], '') = '') OR sgi.[Floor] = a.[Floor] OR (sgi.[Floor]= '0' AND a.[Floor] = '')) 
							AND ((ISNULL(sgi.Door, '') = '') OR sgi.Door = a.Door OR (sgi.Door = '0' AND a.Door = ''))	 
							AND (sgi.Meters IS NULL OR ISNULL(a.Meters,0) = sgi.Meters) 
						WHERE @IgnorePhoneAndEmails = 1 or (sgi.Email IS NULL AND sgi.Phone IS NULL)
					END
				END
		END
END