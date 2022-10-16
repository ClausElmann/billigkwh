
-- =============================================
-- Author:      Henrik K. Madsen
-- Create Date: 04-06-2020
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[Sync_Subscriptions_GetByCustomerId]
(@CustomerId int)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

																			SELECT s.Id
																					,s.PhoneNumber
																					,s.Email 
																					,s.SubscriberName
																					,s.Blocked
																					,s.Deleted
																					,s.IdentifierKey
																					,s.DateCreatedUtc
																					,s.DateLastUpdatedUtc
																					,s.DateDeletedUtc
																					,s.BalarmId
																					,s.BalarmEmailId
																					,HASHBYTES('SHA1',  
																					 CAST(ISNULL(s.PhoneNumber,'0') AS nvarchar(100)) --Phonenumber
																					+ CAST(ISNULL(s.Email,'') AS nvarchar(200)) 
																					+ CAST(ISNULL(s.SubscriberName,'') AS nvarchar(100)) 
																					+ CAST(s.CustomerId AS nvarchar(100)) 
																					+ CAST(s.Blocked AS nvarchar(100)) 
																					+ CAST(s.Deleted AS nvarchar(100))  
																					+ CAST(s.IdentifierKey AS nvarchar(100))
																					+ CAST('1' AS nvarchar(100)) --SubscriptionTypeId
																					) As HashValue

																			FROM Subscriptions s
																			WHERE s.CustomerId = @CustomerId
																				AND 
																				(s.IdentifierTypeId = 1 OR s.SubscriptionTypeId = 1)
																				AND
																				 (
																					(
																						(s.Deleted = 0) 
																						AND 
																						(
																							s.HashValue IS NULL 
																							OR	s.HashValue <> HASHBYTES('SHA1',  
																												 CAST(ISNULL(s.PhoneNumber,'0') AS nvarchar(100)) --Phonenumber
																												+ CAST(ISNULL(s.Email,'') AS nvarchar(200)) 
																												+ CAST(ISNULL(s.SubscriberName,'') AS nvarchar(100)) 
																												+ CAST(s.CustomerId AS nvarchar(100)) 
																												+ CAST(s.Blocked AS nvarchar(100)) 
																												+ CAST(s.Deleted AS nvarchar(100))  
																												+ CAST(s.IdentifierKey AS nvarchar(100))
																												+ CAST('1' AS nvarchar(100)) --SubscriptionTypeId
																												)
																						)
																					)
																					OR
																					(
																						(s.Deleted = 1) 
																						AND NOT s.HashValue IS NULL 
																						AND	s.HashValue <> HASHBYTES('SHA1',  
																												 CAST(ISNULL(s.PhoneNumber,'0') AS nvarchar(100)) --Phonenumber
																												+ CAST(ISNULL(s.Email,'') AS nvarchar(200)) 
																												+ CAST(ISNULL(s.SubscriberName,'') AS nvarchar(100)) 
																												+ CAST(s.CustomerId AS nvarchar(100)) 
																												+ CAST(s.Blocked AS nvarchar(100)) 
																												+ CAST(s.Deleted AS nvarchar(100))  
																												+ CAST(s.IdentifierKey AS nvarchar(100))
																												+ CAST('1' AS nvarchar(100)) --SubscriptionTypeId
																												)
																					)
																				)


END