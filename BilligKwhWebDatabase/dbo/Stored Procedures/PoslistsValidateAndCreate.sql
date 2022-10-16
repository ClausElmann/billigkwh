
CREATE PROCEDURE [dbo].[PoslistsValidateAndCreate]

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	SELECT TOP 200 f.*, 0 AS ProfileSplitter, p.*, 0 AS CustomerSplitter, c.*
	FROM dbo.ProfilePositiveListFiles f 
	INNER JOIN dbo.Profiles p ON f.ProfileId = p.Id
	INNER JOIN dbo.Customers c ON  p.CustomerId = c.Id
	WHERE 
	(ISNULL(f.Deleted, 0) = 0) AND
	(ISNULL(f.AttemptCount, 0) < 3) AND 
	(DATEDIFF(HOUR,f.DateCreatedUtc, GETUTCDATE()) < 24) AND
	(
		f.State = 'ReadyToCreate' AND ISNULL(f.LateInsert, 0) = 0
	)
	ORDER BY f.DateCreatedUtc DESC 


END