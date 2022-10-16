
CREATE PROCEDURE [dbo].[PosListsSelectByDateRange]
(
    -- Add the parameters for the stored procedure here
    @DateCreatedFrom DATETIME = GETUTCDATE,
    @DateCreatedTo DATETIME = GETUTCDATE
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT 
					 pf.Id AS Id,
					 c.Name AS CustomerName,
					 p.Name AS ProfileName,
					 pf.OrginalFilename AS FileName,
					 pf.DateCreatedUtc AS CreatedUtc,
					 pf.DateInsertStartUtc AS InsertStartUtc,
					 pf.DateInsertedUtc AS InsertedUtc,
					 pf.LateInsert AS LateInsert,
					 pf.TotalRecordCount AS TotalRecordCount,
					 pf.InvalidRecordCount AS InvalidRecordCount,
					 pf.State AS Status,
					 pf.CrudOperation
					  FROM dbo.ProfilePositiveListFiles pf
					  INNER JOIN dbo.Profiles p  on p.Id = pf.ProfileId
					  INNER JOIN dbo.Customers c on c.Id = p.CustomerId
					  WHERE 
					  ISNULL(pf.Deleted, 0) = 0 AND
					  pf.State <> 'UploadedToBlob' AND
					  pf.State <> 'SettingColumns' AND
					  pf.DateCreatedUtc >= @DateCreatedFrom AND
					  pf.DateCreatedUtc <= @DateCreatedTo
END