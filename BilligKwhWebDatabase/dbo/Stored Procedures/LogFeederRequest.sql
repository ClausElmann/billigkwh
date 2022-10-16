Create PROC [dbo].[LogFeederRequest]
	@PathClass nvarchar(150),
	@Method nvarchar(150),
	@Request nvarchar(MAX),
	@LogInfo nvarchar(150)
AS

INSERT INTO dbo.RequestFeeder
(
Tidspunkt,
PathClass,
Metode,
Request,
ErrInfo
)
VALUES
(
GETDATE(),
@PathClass,
@Method,
@Request,
@LogInfo
)