
CREATE PROCEDURE [dbo].[RighttobeforgottenSearch]
(
    -- Add the parameters for the stored procedure here
	@Mobile INT = NULL,
    @Zip INT = NULL, 
    @Street NVARCHAR(100) = NULL,
	@Number INT = NULL,
	@PhoneCode INT = 45
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

SELECT 
		Forgotten, 
		Mobile, 
		Zip, 
		Street, 
		Number, 
		PhoneCode, 
		Comment,
		DisplayName,
		Letter,
		Floor,
		Door,
		City,
		LocationName,
		Kvhx
FROM 
		dbo.View_ForgottenNumbers vf
WHERE
		(@Mobile IS NULL OR  vf.Mobile = @Mobile) AND
		(@Zip IS NULL OR vf.Zip = @Zip) AND
		(@Street IS NULL OR vf.Street LIKE @Street + '%') AND
		(@Number IS NULL OR @Number = vf.Number)
ORDER BY 
		vf.Forgotten DESC, vf.Zip, vf.Street, vf.Number

END