


CREATE PROCEDURE [dbo].[BenchmarkRapportCustomer]
(
	@CustomerId int
)
AS


	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT --@UserID as UserId
	   BenchmarkReports.ProfileTypeId
      ,[BenchmarkCategory]
      ,[MonthNumber]
      ,[Year]
      ,sum([NumberOfInterruptions]) as [NumberOfInterruptions]
      ,sum([NumberOfAffectedAddresses]) as [NumberOfAffectedAddresses]
	  ,sum([NrAffectedAdressManualEdit]) as [NrAffectedAdressManualEdit]
      ,sum([NumberOfInterruptionsMinutes]) as [NumberOfInterruptionsMinutes]
	  ,max([TotalNumberOfAdress]) as [TotalNumberOfAdress]
	  ,min([TotalNrAdressesManualEdit]) as [TotalNrAdressesManualEdit]
      ,sum([TotalIntMin]) as [TotalIntMin] 
  FROM [BenchmarkReports]
  inner join Profiles on Profiles.Id = [BenchmarkReports].ProfileId
  where Profiles.CustomerId = @CustomerId
  group by 
  BenchmarkReports.ProfileTypeId,
  [BenchmarkCategory],
  [MonthNumber],
  [Year]