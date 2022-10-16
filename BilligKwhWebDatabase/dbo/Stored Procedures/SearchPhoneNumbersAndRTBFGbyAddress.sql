
CREATE PROCEDURE [dbo].[SearchPhoneNumbersAndRTBFGbyAddress]
(
    -- Add the parameters for the stored procedure here
    @Zip INT, 
    @Street NVARCHAR(100),
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
		DisplayName  
FROM 
		dbo.View_ForgottenNumbers vf
WHERE
		vf.PhoneCode = @PhoneCode AND
		vf.Zip = @Zip AND 
		(@Street IS NULL OR vf.Street LIKE @Street + '%') AND
		(@Number IS NULL OR @Number = vf.Number)
ORDER BY 
		vf.Forgotten DESC, vf.Zip, vf.Street, vf.Number

END