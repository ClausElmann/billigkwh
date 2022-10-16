-- =============================================
-- Author:      Henrik Madsen
-- Create Date: 13-04-2020
-- Description: Brugt til Subscription sync
-- =============================================
	CREATE PROCEDURE [dbo].[Sync_Subscriptions_Update]
	(
	 @AddedNumId bigint 
	, @AddedNumber bigint 
	, @Created datetime = NULL
	, @Updated datetime = NULL
	, @DeletedDateTime datetime = NULL
	, @KVHx nvarchar(19) = NULL
	, @Blocked bit
	, @Disabled bit
	, @Name nvarchar(100) = NULL
	, @SmsServiceId int = NULL
	, @HashValueSmsService varbinary(40)
	, @foundKVHx nvarchar(25) = NULL
	, @CustomerId int
	)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON


								--IF we don't have @SmsServiceId... then try to find a key by before inserting a new value. 
								IF @SmsServiceId IS NULL OR @SmsServiceId = 0
								BEGIN
									SELECT top(1) @SmsServiceId = [Id] 
									FROM [Subscriptions] 
									WHERE 
									[BalarmId] = @AddedNumId
									OR
									[CustomerId] = @CustomerId AND [IdentifierKey] = @foundKVHx AND [PhoneNumber] = @AddedNumber AND [Deleted] = 0  
									ORDER BY [Id] DESC
								END

								--Do we update or insert new record?
								IF @SmsServiceId IS NULL OR @SmsServiceId = 0
									BEGIN
										--This record do not exsist in tagret
										--Print 'INSERT: Kvhx: ' + ISNULL(@foundKVHx,'NULL') + ' = ' + ISNULL(@KVHx,'NULL') + ' - ' + ' Letter:' + ISNULL(@Letter,'NULL') + ' Floor:' + ISNULL(@Floor,'NULL')+ ' Door:' + ISNULL(@Door,'NULL') + ' Hash:' ;
										----Find subscriptions neaded to be inserted

										INSERT INTO Subscriptions
											   ([PhoneNumber]
											   ,[Email]
											   ,[SubscriberName]
											   ,[CustomerId]
											   ,[Blocked]
											   ,[DateCreatedUtc]
											   ,[Deleted]
											   ,[DateDeletedUtc]
											   ,[IdentifierKey]
											   ,[IdentifierTypeId]
											   ,[SubscriptionTypeId]
											   ,[DateLastUpdatedUtc]
											   ,[BalarmId]
											   ,[HashValue])
										 VALUES
											   (@AddedNumber
											   ,NULL
											   ,@Name
											   ,@CustomerId
											   ,@Blocked
											   ,@Created
											   ,@Disabled
											   ,NULL
											   ,@foundKVHx
											   ,1
											   ,1
											   ,@Updated
											   ,@AddedNumId
											   ,@HashValueSmsService)

									END
								ELSE
									BEGIN
										--Print 'UPDATE: Kvhx: ' + ISNULL(@foundKVHx,'NULL') + ' = ' + ISNULL(@KVHx,'NULL') + ' - ' + ' Letter:' + ISNULL(@Letter,'NULL') + ' Floor:' + ISNULL(@Floor,'NULL')+ ' Door:' + ISNULL(@Door,'NULL');
										--This record exsist in taget then UPDATE :-)
										--If deleted set date
										IF @Disabled = 1
											BEGIN
												SET @DeletedDateTime = @Updated
											END
										ELSE
											BEGIN
												SET @DeletedDateTime = NULL
											END
										
																			
										----Find subscriptions neaded to be updated
										IF @Disabled = 1
											BEGIN
												DELETE FROM Subscriptions
												WHERE Id = @SmsServiceId
											END
										ELSE
											BEGIN
												UPDATE Subscriptions
													   SET [PhoneNumber] = @AddedNumber
													   ,[SubscriberName] = @Name
													   ,[CustomerId] = @CustomerId
													   ,[Blocked] = @Blocked
													   ,[Deleted] = @Disabled
													   ,[DateDeletedUtc] = @DeletedDateTime
													   ,[IdentifierKey] = @foundKVHx
													   ,[IdentifierTypeId] = 1
													   ,[SubscriptionTypeId] = 1
													   ,[DateLastUpdatedUtc] = @Updated
													   ,[BalarmId] = @AddedNumId
													   ,[HashValue] = HASHBYTES('SHA1',  
															CAST(ISNULL(@AddedNumber,'0') AS nvarchar(100))
															+ CAST(ISNULL('','') AS nvarchar(150)) --Email
															+ CAST(ISNULL(@Name,'') AS nvarchar(100)) 
															+ CAST(@CustomerId AS nvarchar(100)) 
															+ CAST(@Blocked AS nvarchar(100)) 
															+ CAST(@Disabled AS nvarchar(100))  
															+ CAST(@foundKVHx AS nvarchar(100))
															+ CAST('1' AS nvarchar(100)) --SubscriptionTypeId
															)
												WHERE Id = @SmsServiceId
											END
									END
END