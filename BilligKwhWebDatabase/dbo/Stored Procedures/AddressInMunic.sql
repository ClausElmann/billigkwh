
CREATE PROCEDURE [dbo].[AddressInMunic]
(
	@CountryId INT,
	@MunicipalityCode SMALLINT
)
AS
BEGIN
SET NOCOUNT ON;

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT a.Id, a.ExtUUID, a.Zipcode, a.Street, a.Number, a.Letter, a.[Floor], a.Door, a.Meters, a.OriginCode, a.Kvhx, a.Kvh
	FROM dbo.Addresses a
	WHERE (a.CountryId = @CountryId) AND (a.MunicipalityCode = @MunicipalityCode) 


END