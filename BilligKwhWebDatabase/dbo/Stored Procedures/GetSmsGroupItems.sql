CREATE PROCEDURE [dbo].[GetSmsGroupItems]
(
	@SmsGroupId INT
)
AS
BEGIN
	SELECT sgi.* 
	FROM dbo.SmsGroupItems sgi
	WHERE sgi.SmsGroupId = @SmsGroupId
END