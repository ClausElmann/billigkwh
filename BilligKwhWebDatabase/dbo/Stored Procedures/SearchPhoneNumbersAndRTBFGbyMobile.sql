
CREATE PROCEDURE [dbo].[SearchPhoneNumbersAndRTBFGbyMobile]
(
    -- Add the parameters for the stored procedure here
    @Mobile INT,
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
		vf.Mobile = @Mobile
ORDER BY 
		vf.Forgotten DESC, vf.Zip, vf.Street, vf.Number

END