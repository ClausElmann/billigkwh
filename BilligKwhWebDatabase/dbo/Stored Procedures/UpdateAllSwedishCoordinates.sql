
CREATE PROCEDURE [dbo].[UpdateAllSwedishCoordinates]
(
	@CountryId INT,
	@MunicipalityCode SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	EXEC dbo.UpdateMunicipalitiesSwedenGISupdates @MunicipalityCode = @MunicipalityCode, @State = 'PROCESSING'

	
	DECLARE @Rowcount INT = 1

	WHILE @Rowcount > 0
	BEGIN
		DELETE TOP (4900) FROM dbo.Addresses WHERE CountryId = @CountryId AND MunicipalityCode = @MunicipalityCode

		SET @Rowcount = @@ROWCOUNT
	END


END