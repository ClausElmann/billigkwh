CREATE TABLE [dbo].[Elpris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DatoUtc] [datetime] NOT NULL,
	[TimeDk] [int] NOT NULL,
	[Dk1] [decimal](4, 2) NOT NULL,
	[Dk2] [decimal](4, 2) NOT NULL,
	[Updated] [datetime] NOT NULL,
    CONSTRAINT [PK_Elpris] PRIMARY KEY ([Id]),
 )