
CREATE PROCEDURE [dbo].[UpdateMunicipalitiesSwedishOwnerState]
(
	@MunicipalityCode SMALLINT,
	@OwnersState VARCHAR(20)
)
AS
BEGIN

	UPDATE dbo.MunicipalitiesSwedenGISupdates 
	SET		[OwnersState] = @OwnersState, 
			DateLastUpdatedUtc = GETUTCDATE(), 
			DateOwnersStateUpdatedUtc = GETUTCDATE()
	WHERE MunicipalityCode = @MunicipalityCode

END