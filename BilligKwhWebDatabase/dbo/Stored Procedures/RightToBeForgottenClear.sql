
CREATE PROCEDURE [dbo].[RightToBeForgottenClear] 
AS
BEGIN
	DELETE M
	FROM dbo.PhoneNumbers AS M 
	INNER JOIN dbo.RightToBeForgottens AS N ON M.NumberIdentifier = N.Mobile 
	WHERE M.CountryId = 1


END