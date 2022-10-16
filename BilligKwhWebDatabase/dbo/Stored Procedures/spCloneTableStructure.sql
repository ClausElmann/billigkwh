
CREATE PROCEDURE [dbo].[spCloneTableStructure]
    @TableName nvarchar(128)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RecID INT
       ,@RecCount INT
       ,@ExecuteCMD NVARCHAR(MAX) = ''
       ,@DateTime VARCHAR(100)
       ,@Schema NVARCHAR(128)
       ,@SourceTable NVARCHAR(128)
       ,@DestinationTable NVARCHAR(128)
       ,@NameAdd NVARCHAR(128) = N'_ST';

    BEGIN TRANSACTION;
    BEGIN TRY;
        SET XACT_ABORT ON;
        SELECT  @Schema = s.name
               ,@SourceTable = t.name
               ,@DestinationTable = t.name + @NameAdd
        FROM    sys.tables AS t
        INNER JOIN sys.schemas AS s
        ON      s.schema_id = t.schema_id
        WHERE   s.name = 'dbo'
                AND t.name = @TableName;

        BEGIN
            SET @ExecuteCMD ='IF EXISTS (SELECT *   FROM sys.tables AS t 
                                        INNER JOIN sys.schemas AS s
                                            ON s.schema_id = t.schema_id
                                        WHERE s.name = ''' + @Schema + ''' AND t.name = ''' + @DestinationTable + ''')
                                            DROP TABLE [' + @Schema + '].[' + @DestinationTable + ']

                              SELECT TOP (0) * INTO [' + @Schema + '].[' + @DestinationTable + '] FROM [' + @Schema + '].[' + @SourceTable + ']';
            SELECT @DateTime = CONVERT(VARCHAR(100),GETDATE(),(121));
            RAISERROR('--Creating table: %s at: %s ',0,1,@DestinationTable,@DateTime) WITH NOWAIT;  
            PRINT @ExecuteCMD;
            EXECUTE sp_executesql @ExecuteCMD;
        END;

        BEGIN
            --create indexes and PK
            DECLARE @IndexId INT
               ,@IndexName NVARCHAR(128)
               ,@FilterDefinition NVARCHAR(MAX)
               ,@IsPrimaryKey BIT
               ,@Unique NVARCHAR(128)
               ,@Clustered NVARCHAR(128)
               ,@DataCompression NVARCHAR(60)
               ,@KeyColumns NVARCHAR(MAX)
               ,@IncludedColumns NVARCHAR(MAX);

            IF OBJECT_ID('tempdb.dbo.#Indexes') IS NOT NULL
            BEGIN 
                DROP TABLE dbo.#Indexes;
            END;

            CREATE TABLE dbo.#Indexes
                (
                 [RecID] INT IDENTITY(1, 1) PRIMARY KEY
                ,IndexId INT
                ,IndexName NVARCHAR(128)
                ,IsUnique BIT
                ,FilterDefinition NVARCHAR(MAX)
                ,IsClustered INT
                ,IsPrimaryKey BIT
                ,DataCompression NVARCHAR(60)
                );

            INSERT INTO dbo.#Indexes
                    ( IndexId
                    ,IndexName
                    ,IsUnique
                    ,FilterDefinition
                    ,IsClustered
                    ,IsPrimaryKey
                    ,DataCompression )
            SELECT  i.index_id
                   ,i.name
                   ,i.is_unique
                   ,i.filter_definition
                   ,i.index_id
                   ,i.is_primary_key
                   ,sp.data_compression_desc
            FROM    sys.indexes AS i
            INNER JOIN sys.tables AS t
            ON      t.[object_id] = i.[object_id]
            INNER JOIN sys.schemas AS s
            ON      s.[schema_id] = t.[schema_id]
            INNER JOIN sys.partitions AS sp 
            ON      i.[object_id] = sp.[object_id]
                    AND i.[index_id] = sp.[index_id]
                    AND sp.partition_number = 1
            WHERE   i.type <>0 
                    AND s.name = @Schema
                    AND t.name = @SourceTable;

            SELECT @RecCount = COUNT(*) FROM dbo.#Indexes;
            SET @RecID = 1;
            WHILE (@RecID <= @RecCount)     
            BEGIN
                SELECT  @IndexId = IndexId
                       ,@IndexName = IndexName
                       ,@Unique = CASE WHEN IsUnique = 1 THEN ' UNIQUE ' ELSE '' END
                       ,@FilterDefinition = FilterDefinition
                       ,@Clustered = CASE WHEN IsClustered = 1 THEN ' CLUSTERED ' ELSE ' NONCLUSTERED ' END
                       ,@IsPrimaryKey = IsPrimaryKey
                       ,@DataCompression = DataCompression
                       ,@KeyColumns = ''
                       ,@IncludedColumns = ''
                FROM    dbo.#Indexes
                WHERE   [RecID] = @RecID;

                SELECT  @KeyColumns = @KeyColumns + '[' + c.name + '] '
                        + CASE WHEN is_descending_key = 1 THEN 'DESC'
                               ELSE 'ASC'
                          END + ','
                FROM    sys.index_columns ic
                INNER JOIN sys.columns c
                ON c.object_id = ic.object_id
                    AND c.column_id = ic.column_id
                INNER JOIN sys.tables AS t
                ON t.object_id = c.object_id
                INNER JOIN sys.schemas AS s
                ON s.schema_id = t.schema_id
                WHERE   ic.index_id = @IndexId
                    AND s.name = @Schema
                    AND t.name = @SourceTable
                    AND key_ordinal > 0
                ORDER BY index_column_id;

                SELECT  @IncludedColumns = @IncludedColumns + '[' + c.name + '],'
                FROM    sys.index_columns ic
                INNER JOIN sys.columns c
                ON      c.object_id = ic.object_id
                        AND c.column_id = ic.column_id
                INNER JOIN sys.tables AS t
                ON t.object_id = c.object_id
                INNER JOIN sys.schemas AS s
                ON s.schema_id = t.schema_id
                WHERE   ic.index_id = @IndexId
                    AND s.name = @Schema
                    AND t.name = @SourceTable
                    AND key_ordinal = 0
                ORDER BY index_column_id;

                IF LEN(@KeyColumns) > 0
                    SET @KeyColumns = LEFT(@KeyColumns, LEN(@KeyColumns) - 1);

                IF LEN(@IncludedColumns) > 0
                BEGIN
                    SET @IncludedColumns = ' INCLUDE (' + LEFT(@IncludedColumns, LEN(@IncludedColumns) - 1) + ')';
                END

                IF @FilterDefinition IS NULL
                    SET @FilterDefinition = '';
                ELSE
                    SET @FilterDefinition = 'WHERE ' + @FilterDefinition + ' ';

                --create the index or PK
                IF @IsPrimaryKey = 1
                    SET @ExecuteCMD = 'ALTER TABLE [' + @Schema + '].[' + @DestinationTable + '] ADD  CONSTRAINT [' + @IndexName + @NameAdd + '] PRIMARY KEY CLUSTERED (' + @KeyColumns + ') WITH (SORT_IN_TEMPDB=ON,DATA_COMPRESSION='+@DataCompression+');';
                ELSE
                    SET @ExecuteCMD = 'CREATE ' + @Unique + @Clustered + ' INDEX [' + @IndexName + '] ON [' + @Schema + '].[' + @DestinationTable + '] (' + @KeyColumns + ')' + @IncludedColumns + @FilterDefinition + ' WITH (SORT_IN_TEMPDB=ON,DATA_COMPRESSION='+@DataCompression+');';

                SELECT @DateTime = CONVERT(VARCHAR(100),GETDATE(),(121));
                RAISERROR('--Creating index: %s%s at: %s ',0,1,@IndexName,@NameAdd,@DateTime) WITH NOWAIT;  
                PRINT @ExecuteCMD;
                EXECUTE sp_executesql @ExecuteCMD;

                SET @RecID = @RecID + 1;

            END;/*While loop*/
        END;

        COMMIT TRAN;
        SET XACT_ABORT OFF;
    END TRY BEGIN CATCH;
        SET XACT_ABORT OFF;
        IF (XACT_STATE() != 0)
        BEGIN;
            ROLLBACK TRANSACTION;
        END;
        THROW;
        -- RETURN;
    END CATCH;
END