CREATE TABLE [dbo].[Kunde] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [Kundenavn]             NVARCHAR (100)   CONSTRAINT [DF_Kunde_Kundenavn] DEFAULT ('') NOT NULL,
    [Adresse]               NVARCHAR (MAX)   CONSTRAINT [DF_Kunde_Adresse_1] DEFAULT ('') NOT NULL,
    [Kontakt]               NVARCHAR (50)    CONSTRAINT [DF_Kunde_Kontakt_1] DEFAULT ('') NOT NULL,
    [Telefon]               NVARCHAR (50)    CONSTRAINT [DF_Kunde_Telefon] DEFAULT ('') NOT NULL,
    [Fax]                   NVARCHAR (50)    CONSTRAINT [DF_Kunde_Fax] DEFAULT ('') NOT NULL,
    [Email]                 NVARCHAR (50)    CONSTRAINT [DF_Kunde_Email] DEFAULT ('') NOT NULL,
    [PostNr]                SMALLINT         CONSTRAINT [DF_Kunde_Postnummer] DEFAULT ((0)) NOT NULL,
    [By]                    NVARCHAR (100)   CONSTRAINT [DF_Kunde_By] DEFAULT ('') NOT NULL,
    [Lat]                   FLOAT (53)       CONSTRAINT [DF_Kunde_Lat_1] DEFAULT ((0)) NOT NULL,
    [Lon]                   FLOAT (53)       CONSTRAINT [DF_Kunde_Lon_1] DEFAULT ((0)) NOT NULL,
    [KundeTypeID]           SMALLINT         CONSTRAINT [DF_Kunde_KundeType] DEFAULT ((0)) NOT NULL,
    [Skjult]                BIT              CONSTRAINT [DF_Kunde_Skjult_1] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME         CONSTRAINT [DF_Kunde_SidstRettet_1] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT              CONSTRAINT [DF_Kunde_SidstRettetAfBrugerId_1] DEFAULT ((-1)) NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_Kunde_Slettet_1] DEFAULT ((0)) NOT NULL,
    [KundeGuid]             UNIQUEIDENTIFIER CONSTRAINT [DF_Kunde_KundeGuid] DEFAULT (newid()) NOT NULL,
    [BrancheTypeID]         SMALLINT         CONSTRAINT [DF_Kunde_BrancheID] DEFAULT ((1)) NOT NULL,
    [SprogID]               INT              CONSTRAINT [DF_Kunde_SprogID_1] DEFAULT ((1)) NOT NULL,
    [Kontaktperson]         NVARCHAR (50)    CONSTRAINT [DF_Kunde_Kontakt1] DEFAULT ('') NOT NULL,
    [KundeOverskrift]       NVARCHAR (50)    CONSTRAINT [DF_Kunde_Kontakt2] DEFAULT ('') NOT NULL,
    [BrugNyMinkModel]       BIT              CONSTRAINT [DF_Kunde_BrugNyMinkModel] DEFAULT ((0)) NULL,
    [LandID]                INT              CONSTRAINT [DF_Kunde_LandID] DEFAULT ((1)) NOT NULL,
    [Cvr]                   INT              NULL,
    [TidzoneId]             NVARCHAR (50)    CONSTRAINT [DF_Kunde_TidzoneId] DEFAULT ('Romance Standard Time') NOT NULL,
    [EconomicId]            INT              NULL,
    [FakturaMail]           NVARCHAR (255)   NULL,
    [FakturaKontaktPerson]  NVARCHAR (100)   NULL,
    [FakturaTelefonFax]     NVARCHAR (255)   NULL,
    [FakturaMobil]          NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Kunde] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Kunde_PostnummerBy] FOREIGN KEY ([PostNr]) REFERENCES [dbo].[PostnummerBy] ([Postnummer]),
    CONSTRAINT [FK_Kunde_Type] FOREIGN KEY ([KundeTypeID]) REFERENCES [dbo].[TypeID] ([ID]),
    CONSTRAINT [FK_Kunde_Type1] FOREIGN KEY ([BrancheTypeID]) REFERENCES [dbo].[TypeID] ([ID])
);






GO

CREATE TRIGGER [dbo].[KundeAccessCacheUpdate] ON [dbo].[Kunde]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 

	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(Kundenavn) OR UPDATE(Slettet) OR UPDATE(Skjult) OR UPDATE(SprogID)) 
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
			IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.Kundenavn <> D.Kundenavn OR I.Slettet <> D.Slettet OR I.Skjult <> D.Skjult OR I.SprogID <> D.SprogID))
     				BEGIN
        					UPDATE [AccessCacheState] SET [KundeTabel] = GETUTCDATE(), [KundeCount] = [KundeCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [KundeTabel] = GETUTCDATE(), [KundeCount] = [KundeCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [KundeTabel] = GETUTCDATE(), [KundeCount] = [KundeCount] + 1;
	END