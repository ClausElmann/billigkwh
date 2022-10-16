
CREATE PROCEDURE [dbo].[GetImportRowsByFileId]
(
	@FileId INT,
	@ValidFilter INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT * FROM dbo.ImportRows
	WHERE ImportFileId = @FileId
	AND ((@ValidFilter IS NULL) OR (ISNULL(Valid, 0) = @ValidFilter))

END