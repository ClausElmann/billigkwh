
CREATE PROCEDURE [dbo].[UpdateMunicipalitiesSwedenGISupdates]
(
	@MunicipalityCode SMALLINT,
	@State VARCHAR(20)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	UPDATE dbo.MunicipalitiesSwedenGISupdates 
	SET		[State] = @State, 
			DateLastUpdatedUtc = GETUTCDATE(), 
			DateStateUpdatedUtc = GETUTCDATE()
	WHERE MunicipalityCode = @MunicipalityCode

END