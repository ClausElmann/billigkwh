-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[FindSubscriptions]
(
	@CountryId INT,
	@CustomerId INT,
	@Zipcode INT,
	@Street  NVARCHAR(200),
	@Number INT,
	@Letter NVARCHAR(3),
	@Floor NVARCHAR(3),
	@Door NVARCHAR(3),
	@Meters INT,
	@IncludeLandline BIT = 1
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
select 
	subscription.SubscriberName, 
	subscription.PhoneNumber,
	subscription.PhoneNumberType,  
	subscription.Email,
	subscription.Blocked IsUnsubscribed,
	subscription.Id AS SubscriptionId,
	a.Zipcode,
	a.City,
	a.Street,
	a.Number,
	a.LocationName,
	a.Letter,
	a.[Floor],
	a.Door,
	a.Kvhx
FROM dbo.Subscriptions AS subscription
INNER JOIN dbo.Addresses as a on a.Kvhx = subscription.IdentifierKey and (subscription.IdentifierTypeId = 1 OR subscription.SubscriptionTypeId = 1)
INNER JOIN dbo.Customers as c on (c.Id = subscription.CustomerId)
where 
 subscription.Deleted = 0 AND
(@Zipcode IS NULL OR a.ZipCode = @Zipcode) AND
(@Street IS NULL OR a.Street = @Street) AND
(@Number IS NULL OR @Number = a.Number) AND
(@Letter IS NULL OR @Letter = a.Letter OR (@Letter = '0' AND ISNULL(a.Letter, '') = '')) AND
(@Floor IS NULL OR @Floor = a.Floor) AND
(@Door IS NULL OR @Door = a.Door) AND
(@Meters IS NULL OR @Meters = a.Meters) AND
(@CustomerId IS NULL OR @CustomerId = c.Id) AND
(@IncludeLandline = 1 OR subscription.PhoneNumberType = 1)
END