CREATE PROC [dbo].[SPLogRequestFeeder]
	@TicksCall int,
	@TicksAll int,
	@BeskedLbNr nvarchar(150),
	@Metode nvarchar(150),
	@Request nvarchar(MAX),
	@ErrInfo nvarchar(MAX)
AS

INSERT INTO dbo.LogRequestFeeder
(
Tidspunkt,
TicksCall,
TicksAll,
BeskedLbNr,
Metode,
Request,
ErrInfo
)
VALUES
(
GETUTCDATE(),
@TicksCall,
@TicksAll,
@BeskedLbNr,
@Metode,
@Request,
@ErrInfo
)