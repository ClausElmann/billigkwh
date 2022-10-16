-- =============================================
-- Author:      Henrik K. Madsen
-- Create Date: 08-04-2020
-- Description: Bruges til at lave opslag på på ID i Subscriptions i forbindelse synkronering af tilmeldinger med gammet system. 
-- =============================================
CREATE PROCEDURE [dbo].[Sync_Subscriptions_GetIdByHash]
(@HashVal varbinary(40))
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	
	Return (SELECT TOP(1) Id FROM Subscriptions WHERE Deleted = 0 AND HashValue = @HashVal)
END