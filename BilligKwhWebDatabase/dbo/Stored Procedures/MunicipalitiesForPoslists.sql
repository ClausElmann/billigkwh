
CREATE PROCEDURE [dbo].[MunicipalitiesForPoslists]
(
	@CountryId INT,
	@skipMunCodes dbo.MunCodesTableType READONLY,
	@OnlyhMunicipalityCode SMALLINT = NULL 
)
AS
BEGIN
SET NOCOUNT ON;

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/*
 CREATE TYPE [dbo].[MunCodesTableType] AS TABLE([MunicipalityCode] SMALLINT NOT NULL)


Test-code:

DECLARE @testskipMunCodes dbo.MunCodesTableType 

INSERT INTO @testskipMunCodes VALUES(1440)

EXEC dbo.MunicipalitiesForPoslists @CountryId = 2,              
									@skipMunCodes = @testskipMunCodes,
                                   @OnlyhMunicipalityCode = NULL
*/								   

IF (@OnlyhMunicipalityCode IS NOT NULL)
BEGIN
				SELECT TOP(1) m.*
					FROM dbo.Municipalities m
					WHERE
						m.MunicipalityCode = @OnlyhMunicipalityCode AND
						m.CountryId = @CountryId
END
ELSE
BEGIN

	SELECT DISTINCT
		allUnion.MunicipalityCode,
		allUnion.MunicipalityName 
	FROM 
		dbo.Municipalities allUnion 
	WHERE 
		allUnion.CountryId = @CountryId AND		
		NOT Exists (SELECT MunicipalityCode FROM @skipMunCodes WHERE MunicipalityCode = allUnion.MunicipalityCode )
	ORDER BY MunicipalityName
END

END