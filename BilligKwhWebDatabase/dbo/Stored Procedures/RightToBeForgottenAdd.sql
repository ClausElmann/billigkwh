
CREATE PROCEDURE [dbo].[RightToBeForgottenAdd] 
	-- Add the parameters for the stored procedure here
	@Mobile int,
	@Zip int,
	@Street nvarchar(100),
	@Number int null,
	@Comment nvarchar(max),
	@City nvarchar(100),
	@LocationName nvarchar(100),
	@Letter nvarchar(10),
	@Floor nvarchar(10),
	@Door nvarchar(10),
	@DisplayName nvarchar(200),
	@Kvhx nvarchar(20)

AS
BEGIN
		UPDATE dbo.RightToBeForgottens
		SET Comment = @Comment
		WHERE Mobile = @Mobile 
		AND Zip = @Zip
		AND Street = @Street
		AND Number = @Number

		IF(@@ROWCOUNT = 0)
		BEGIN
		INSERT INTO [dbo].[RightToBeForgottens]
				   ([Mobile]
				   ,[Zip]
				   ,[Street]
				   ,[Number]
				   ,[Comment]
				   ,[City]
				   ,[LocationName]
				   ,[Letter]
				   ,[Floor]
				   ,[Door]
				   ,[DisplayName]
				   ,[Kvhx])
			 VALUES
				   (@Mobile
				   ,@Zip
				   ,@Street
				   ,@Number
				   ,@Comment
				   ,@City,
					@LocationName,
					@Letter,
					@Floor,
					@Door,
					@DisplayName,
					@Kvhx
				   )
		END

		--Remove from PhoneNumbers
		DELETE M
		FROM dbo.PhoneNumbers AS M 
		WHERE M.NumberIdentifier = @Mobile AND M.CountryId = 1

		DELETE M
		FROM dbo.Subscriptions AS M
		Where M.PhoneNumber = @Mobile
END