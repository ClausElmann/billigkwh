CREATE PROC [dbo].[insertRequest]

   @RemoteIp varchar(50),
	@UserAgent varchar(2000) ,
	@RequestPath varchar(2000),
	@Referer varchar(2000),
	@IsCrawler varchar(5)
	
AS 

INSERT INTO dbo.Requests
(
RemoteIp,
UserAgent,
RequestPath,
Referer,
Iscrawler
)
VALUES
(

   @RemoteIp,
	@UserAgent  ,
	@RequestPath ,
	@Referer ,	
	@IsCrawler 
)