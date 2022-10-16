CREATE PROCEDURE [dbo].[RebuildAllIndexOnTable]
(
	@TableName NVARCHAR(100)
)

AS
BEGIN
   DECLARE @execstr NVARCHAR(300)	
   SELECT @execstr = 'ALTER INDEX ALL ON ' + @TableName + ' REBUILD'

    --Check REBUILD command not is allredy is running
	IF (SELECT COUNT(*) FROM sys.dm_exec_requests req CROSS APPLY sys.dm_exec_sql_text(sql_handle) AS sqltext  WHERE sqltext.text like @execstr) = 0
	BEGIN
	   EXEC (@execstr)
	END
END