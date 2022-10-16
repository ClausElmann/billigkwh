CREATE PROCEDURE [dbo].[GetLogs]
(
	@DateFrom DATETIME,
	@DateTo DATETIME,
	@Loglevel INT,	
	@SearchString NVARCHAR(80)
)
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @ftSearchString NVARCHAR(100) = '%' + @SearchString + '%'

	IF (@SearchString IS NULL)
	BEGIN
		SELECT * FROM dbo.Logs
        WHERE 
            (DateCreatedUtc > @DateFrom) AND 
            (@DateTo IS NULL OR DateCreatedUtc < @DateTo) AND                            
            (@Loglevel IS NULL OR LogLevelId = @Loglevel) 
	END
	ELSE
	BEGIN
		SELECT * FROM dbo.Logs
        WHERE 
            (DateCreatedUtc > @DateFrom) AND 
            (@DateTo IS NULL OR DateCreatedUtc < @DateTo) AND                            
            (@Loglevel IS NULL OR LogLevelId = @Loglevel) AND
            ((@SearchString IS NULL) OR ((ShortMessage like @ftSearchString ))  )

	END
END