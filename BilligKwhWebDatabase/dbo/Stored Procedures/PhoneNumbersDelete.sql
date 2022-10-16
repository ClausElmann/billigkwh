﻿
CREATE PROCEDURE [dbo].[PhoneNumbersDelete]
(
	@CountryId INT,
	@MunicipalityCode SMALLINT
)
AS

BEGIN
	DECLARE @Rowcount INT = 1

	WHILE @Rowcount > 0
	BEGIN
		DELETE TOP (4800) 
		FROM	dbo.PhoneNumbers 
		WHERE	CountryId = @CountryId 
		AND		MunicipalityCode = @MunicipalityCode
		SET @Rowcount = @@ROWCOUNT
	END
END