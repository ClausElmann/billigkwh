create PROC [dbo].[insertLogRequest]
	@Duration float,
	@RequestPage nvarchar(100),
	@RequestQuery nvarchar(MAX),
	@RefererPage nvarchar(100),
	@RefererQuery nvarchar(MAX),
    @RemoteIp nvarchar(50),
    @ServerNr int
AS
declare @RequestPageID int, @RemoteIpID int, @RefererPageID int

SELECT @RequestPageID = ID FROM LogRequestPage WHERE Page = @RequestPage
SELECT @RemoteIpID = ID FROM LogRequestIP WHERE RemoteIP = @RemoteIp

IF @RequestPageID IS NULL
BEGIN 
	INSERT INTO dbo.LogRequestPage
	(
	Page
	)
	VALUES
	(
	@RequestPage
	)
	SELECT @RequestPageID=@@IDENTITY-- AS t1
	--set @RequestPageID = t1
END 

IF @RemoteIpID IS NULL
BEGIN 
	INSERT INTO dbo.LogRequestIP
	(
	RemoteIP
	)
	VALUES
	(
	@RemoteIp
	)
	SELECT @RemoteIpID=@@IDENTITY-- AS t1
	--set @RequestPageID = t1
END 

--IF not (@RefererPage IS NULL OR @RefererPage = '')
	SELECT @RefererPageID = ID FROM LogRequestPage WHERE Page = @RefererPage
--	BEGIN 
		IF @RefererPageID IS NULL
		BEGIN 
			INSERT INTO dbo.LogRequestPage
			(
			Page
			)
			VALUES
			(
			@RefererPage
			)
			SELECT @RefererPageID=@@IDENTITY
			--set @RefererPageID = t2
		END
--	 END 

INSERT INTO dbo.LogRequest
(
Duration,
RequestPageID,
RequestQuery,
RefererPageID,
RefererQuery,
ServerNr,
RemoteIpID
)
VALUES
(
@Duration,
@RequestPageID,
@RequestQuery,
@RefererPageID,
@RefererQuery,
@ServerNr,
@RemoteIpID
)