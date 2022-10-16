CREATE PROCEDURE [dbo].[GetSmsGroups]
(
	@CustomerId INT,
	@ProfileId INT,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@OnlyActive BIT,
	@OnlyLookedUp BIT
)
AS
BEGIN
	SELECT g.* FROM 
		dbo.SmsGroups g
	WHERE 
       (@CustomerId IS NULL OR EXISTS (
			SELECT TOP 1 c.Id FROM dbo.Customers c 
			INNER JOIN dbo.Profiles p ON c.Id = p.CustomerId AND p.Id = g.ProfileId WHERE c.Id = @CustomerId )) AND
		(@ProfileId IS NULL OR ProfileId = @ProfileId) AND
		(@FromDate IS NULL OR DateCreatedUtc > @FromDate) AND
		(@ToDate IS NULL OR DateCreatedUtc < @ToDate) AND
		(@OnlyActive IS NULL OR Active = @OnlyActive) AND
		(@OnlyLookedUp IS NULL OR IsLookedUp = @OnlyLookedUp) 
	ORDER BY g.Id DESC
END