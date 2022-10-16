CREATE PROC [dbo].[SPLogRequestJson]
	@TicksCall int,
	@TicksAll int,
	@Metode nvarchar(150),
	@Request nvarchar(MAX),
	@ErrInfo nvarchar(MAX)
AS

INSERT INTO dbo.LogRequestJson
(
Tidspunkt,
TicksCall,
TicksAll,
Metode,
Request,
ErrInfo
)
VALUES
(
GETUTCDATE(),
@TicksCall,
@TicksAll,
@Metode,
@Request,
@ErrInfo
)