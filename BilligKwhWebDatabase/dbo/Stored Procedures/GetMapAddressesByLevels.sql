-- =============================================
-- Author:      Peter Frost
-- Create Date: 26 JAN 2021
-- Description: Called from /api/Address/GetForMapByLevels
-- =============================================
CREATE PROCEDURE [dbo].[GetMapAddressesByLevels]
(
    @smsGroupId bigint
)
AS
BEGIN

	WITH l1 
		 AS (SELECT value AS Level1 
			 FROM   dbo.SmsGroupLevelFilters 
			 WHERE  smsgroupid = @smsGroupId 
					AND level = 1), 
		 l2 
		 AS (SELECT level1, 
					value AS Level2 
			 FROM   l1 
					FULL OUTER JOIN (SELECT * 
									 FROM   dbo.SmsGroupLevelFilters 
									 WHERE  dbo.SmsGroupLevelFilters.smsgroupid = 
											@smsGroupId 
											AND level = 2) t 
								 ON 1 = 1), 
		 l3 
		 AS (SELECT level1, 
					level2, 
					value AS Level3 
			 FROM   l2 
					FULL OUTER JOIN (SELECT * 
									 FROM   dbo.SmsGroupLevelFilters 
									 WHERE  dbo.SmsGroupLevelFilters.smsgroupid = 
											@smsGroupId 
											AND level = 3) t 
								 ON 1 = 1), 
		 l4 
		 AS (SELECT level1, 
					level2, 
					level3, 
					value AS Level4 
			 FROM   l3 
					FULL OUTER JOIN (SELECT * 
									 FROM   dbo.SmsGroupLevelFilters 
									 WHERE  dbo.SmsGroupLevelFilters.smsgroupid = 
											@smsGroupId 
											AND level = 4) t 
								 ON 1 = 1), 
		 l5 
		 AS (SELECT level1, 
					level2, 
					level3, 
					level4, 
					value AS Level5 
			 FROM   l4 
					FULL OUTER JOIN (SELECT * 
									 FROM   dbo.SmsGroupLevelFilters 
									 WHERE  smsgrouplevelfilters.smsgroupid = 
											@smsGroupId 
											AND level = 5) t 
								 ON 1 = 1) 

	SELECT lc.*
	INTO #templevelcomb
	FROM   dbo.ProfilePosListLevelCombinations lc
		   INNER JOIN dbo.SmsGroups s ON s.ProfileId = lc.ProfileId and s.Id = @smsGroupId
		   INNER JOIN l5 
				   ON Isnull(l5.level1, lc.level1) = lc.level1 
					  AND Isnull(l5.level2, lc.level2) = lc.level2 
					  AND Isnull(l5.level3, lc.level3) = lc.level3 
					  AND Isnull(l5.level4, lc.level4) = lc.level4 
					  AND Isnull(l5.level5, lc.level5) = lc.level5 



	SELECT DISTINCT a.latitude, 
					a.longitude, 
					a.zipcode, 
					a.city, 
					a.street, 
					a.number, 
					a.letter,
					a.[Floor],
					a.Door, 
					a.kvhx 
	FROM   #templevelcomb lc 
		   INNER JOIN dbo.ProfilePosListLevelCombinationListings l ON l.levelcombinationid = lc.id 
		   INNER JOIN dbo.Addresses a ON a.kvhx = l.kvhx 
	WHERE  a.datedeletedutc IS NULL 
	ORDER BY a.Zipcode, a.Street, a.Number, a.Letter, a.Floor, a.Door
END