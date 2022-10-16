
CREATE PROCEDURE [dbo].[RightToBeForgottenDelete] 
	-- Add the parameters for the stored procedure here
	@Mobile int,
	@Zip int,
	@Street nvarchar(100),
	@Number int null
AS
BEGIN
	--Try to delete
	DELETE
	FROM dbo.RightToBeForgottens
	WHERE Mobile = @Mobile
	AND Zip = @Zip
	AND Street = @Street
	AND Number = @Number

END