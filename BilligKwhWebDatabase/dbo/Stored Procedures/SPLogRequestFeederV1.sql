CREATE PROC [dbo].[SPLogRequestFeederV1]
	@TicksCall int,
	@TicksAll int,
	@BeskedLbNr nvarchar(150),
	@Metode nvarchar(150),
	@Request nvarchar(MAX),
	@ErrInfo nvarchar(MAX),
	@UniqueID nvarchar(50),
	@Host nvarchar(50)
AS

INSERT INTO dbo.LogRequestFeeder
(
Tidspunkt,
TicksCall,
TicksAll,
BeskedLbNr,
Metode,
Request,
ErrInfo,
UniqueID,
Host
)
VALUES
(
GETUTCDATE(),
@TicksCall,
@TicksAll,
@BeskedLbNr,
@Metode,
@Request,
@ErrInfo,
@UniqueID,
@Host
)