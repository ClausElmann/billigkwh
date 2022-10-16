
CREATE PROCEDURE [dbo].[PoslistsCleanup]

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    SELECT f.*, 0 AS ProfileSplitter, p.*, 0 AS CustomerSplitter, c.*
	FROM dbo.ProfilePositiveListFiles f 
	INNER JOIN dbo.Profiles p ON f.ProfileId = p.Id
	INNER JOIN dbo.Customers c ON  p.CustomerId = c.Id
	WHERE 
	(
		(ISNULL(f.Deleted, 0) = 0) AND
		(DATEDIFF(HOUR,f.DateCreatedUtc, GETUTCDATE()) > 12 OR ISNULL(f.Active, 0) = 1)
	)

END