
CREATE PROCEDURE [dbo].[AdminCustomerDelete]
(
	@CustomerId INT
)
AS

BEGIN
    
    SET NOCOUNT ON

	-- Validation: Required that Customers.Deleted is True 
	DECLARE @IsDeleted BIT = (SELECT TOP(1) Deleted FROM dbo.Customers WHERE Id = @CustomerId)

	IF (ISNULL(@IsDeleted, 0) = 1)
	BEGIN

		BEGIN TRANSACTION

				-- First delete from related datatables.
				DELETE FROM dbo.Subscriptions WHERE CustomerId = @CustomerId
				DELETE FROM dbo.UserRoleMappings WHERE CustomerId = @CustomerId
				DELETE FROM dbo.SocialMediaAccountProfileMappings WHERE CustomerId = @CustomerId
				DELETE FROM dbo.SocialMediaAccounts WHERE CustomerId = @CustomerId
				DELETE FROM dbo.CustomerAccounts WHERE CustomerId = @CustomerId
				DELETE FROM dbo.CustomerLogs WHERE CustomerId = @CustomerId
				DELETE FROM dbo.CustomerProfileRolePrices WHERE CustomerId = @CustomerId

				DELETE FROM dbo.StandardReceiverGroups WHERE CustomerId = @CustomerId
				DELETE FROM dbo.BenchmarkNetworkCompanies WHERE CustomerId = @CustomerId
				DELETE FROM dbo.CustomerUserMappings WHERE CustomerId = @CustomerId
				DELETE FROM dbo.CustomerUserRoleMappings WHERE CustomerId = @CustomerId
				DELETE FROM dbo.Profiles WHERE CustomerId = @CustomerId
				DELETE FROM dbo.WebMessageMapModuleSettings WHERE CustomerId = @CustomerId

				DELETE FROM dbo.StandardReceivers WHERE CustomerId = @CustomerId
				DELETE FROM dbo.StandardReceiverGroups WHERE CustomerId = @CustomerId
				
				DELETE FROM dbo.Templates WHERE CustomerId = @CustomerId
				DELETE FROM dbo.DynamicMergefields WHERE CustomerId = @CustomerId
				DELETE FROM dbo.BenchmarkCauses WHERE CustomerId = @CustomerId
				DELETE FROM dbo.StatstidendeReceivers WHERE CustomerId = @CustomerId
				

				-- Finally Delete the Customer
				DELETE FROM dbo.Customers WHERE Id = @CustomerId

				-- Log it
				INSERT INTO dbo.Logs (LogLevelId,ShortMessage,DateCreatedUtc,Module) VALUES (20, 'Deleted Customer Id ' + CAST(@CustomerId AS NVARCHAR(8)),GETUTCDATE(),'AdminCustomerDelete')

				-- And commit when all done! 
		
		COMMIT TRANSACTION
		SELECT @CustomerId
	END
	ELSE
	BEGIN
		PRINT 'Not deleted anything! Customer must be marked with Deleted.'
		SELECT -1
	END

END