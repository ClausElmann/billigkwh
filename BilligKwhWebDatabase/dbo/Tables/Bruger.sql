CREATE TABLE [dbo].[Bruger] (
    [ID]                               INT              IDENTITY (1, 1) NOT NULL,
    [Brugernavn]                       NVARCHAR (150)   NOT NULL,
    [Adgangskode]                      NVARCHAR (25)    NOT NULL,
    [BrugernavnUtfCode]                UNIQUEIDENTIFIER NULL,
    [BrugernavnUnicode]                UNIQUEIDENTIFIER NULL,
    [VærtKundeID]                      INT              NOT NULL,
    [AktivKundeID]                     INT              CONSTRAINT [DF_Bruger_AktivKundeID1] DEFAULT ((-1)) NOT NULL,
    [ErAdministrator]                  BIT              CONSTRAINT [DF_Bruger_ErAdministrator] DEFAULT ((0)) NOT NULL,
    [SystemAdministrator]              BIT              CONSTRAINT [DF_Bruger_EnviTronicSysAdmin] DEFAULT ((0)) NOT NULL,
    [KundeAdministrator]               BIT              CONSTRAINT [DF_Bruger_SystemAdministrator1] DEFAULT ((0)) NOT NULL,
    [Fornavn]                          NVARCHAR (50)    NOT NULL,
    [Efternavn]                        NVARCHAR (50)    NOT NULL,
    [Afdeling]                         NVARCHAR (100)   NULL,
    [Telefon]                          NVARCHAR (50)    NULL,
    [Mobil]                            NVARCHAR (50)    NULL,
    [NoLogin]                          BIT              CONSTRAINT [DF_Bruger_LoginUser] DEFAULT ((0)) NOT NULL,
    [RemoteIp]                         NVARCHAR (50)    NULL,
    [SprogID]                          INT              CONSTRAINT [DF_Bruger_SprogID] DEFAULT ((1)) NOT NULL,
    [OploesningVandret]                INT              NULL,
    [OploesningLodret]                 INT              NULL,
    [UserAgent]                        NVARCHAR (500)   NULL,
    [Browser]                          NVARCHAR (250)   NULL,
    [Version]                          NVARCHAR (50)    NULL,
    [VarslingSMS]                      CHAR (8)         CONSTRAINT [DF_Bruger_VarslingSMS] DEFAULT ('') NULL,
    [SidstRettet]                      DATETIME         CONSTRAINT [DF_Bruger_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]            INT              NOT NULL,
    [Slettet]                          BIT              NOT NULL,
    [ErHelpdeskBCC]                    BIT              NULL,
    [StandardBedriftID]                INT              CONSTRAINT [DF_Bruger_StandardBedriftID1] DEFAULT ((0)) NOT NULL,
    [FuldtNavn]                        AS               (([Fornavn]+' ')+[Efternavn]),
    [SecurityStamp]                    NVARCHAR (150)   COLLATE SQL_Danish_Pref_CP1_CI_AS CONSTRAINT [DF_Bruger_SecurityStamp_1] DEFAULT ('') NOT NULL,
    [PasswordHash]                     NVARCHAR (150)   COLLATE SQL_Danish_Pref_CP1_CI_AS CONSTRAINT [DF_Bruger_PasswordHash_1] DEFAULT ('') NOT NULL,
    [PortalAdministrator]              BIT              CONSTRAINT [DF_Bruger_PortalAdministrator] DEFAULT ((0)) NOT NULL,
    [PersonDataAcceptDato]             DATETIME         NULL,
    [LandID]                           INT              CONSTRAINT [DF_Bruger_LandID] DEFAULT ((1)) NOT NULL,
    [Password]                         NVARCHAR (200)   CONSTRAINT [DF_Bruger_Password] DEFAULT ('') NOT NULL,
    [PasswordSalt]                     NVARCHAR (24)    NULL,
    [FailedLoginCount]                 INT              CONSTRAINT [DF_Bruger_FailedLoginCount] DEFAULT ((0)) NOT NULL,
    [IsLockedOut]                      BIT              CONSTRAINT [DF_Bruger_IsLockedOut] DEFAULT ((0)) NOT NULL,
    [DateLastFailedLoginUtc]           DATETIME         NULL,
    [DateLastLoginUtc]                 DATETIME         NULL,
    [PasswordResetToken]               UNIQUEIDENTIFIER NULL,
    [DatePasswordResetTokenExpiresUtc] DATETIME         NULL,
    [ImpersonatingUserId]              INT              NULL,
    [TidzoneId]                        NVARCHAR (50)    CONSTRAINT [DF_Bruger_TidzoneId] DEFAULT ('Romance Standard Time') NOT NULL,
    [ResetPhone]                       BIGINT           NULL,
    CONSTRAINT [PK_Bruger] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Bruger_AktivKunde] FOREIGN KEY ([AktivKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Bruger_Bedrift] FOREIGN KEY ([StandardBedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_Bruger_VærtKunde] FOREIGN KEY ([VærtKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [UK_Brugernavn] UNIQUE NONCLUSTERED ([Brugernavn] ASC) WITH (FILLFACTOR = 90)
);




GO
	
CREATE TRIGGER [dbo].[BrugerAccessCacheUpdate] ON [dbo].[Bruger]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
    
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(Brugernavn) OR UPDATE(Adgangskode) OR UPDATE(Slettet) OR UPDATE(Fornavn) OR UPDATE(Efternavn) OR UPDATE(VærtKundeID) OR UPDATE(SprogID) OR UPDATE(SystemAdministrator) OR UPDATE(KundeAdministrator))
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.Brugernavn <> D.Brugernavn OR I.Adgangskode <> D.Adgangskode OR I.Slettet <> D.Slettet OR I.Fornavn <> D.Fornavn OR I.Efternavn <> D.Efternavn OR I.VærtKundeID <> D.VærtKundeID OR I.SprogID <> D.SprogID OR I.SystemAdministrator <> D.SystemAdministrator OR I.KundeAdministrator <> D.KundeAdministrator))
					BEGIN
        					UPDATE [AccessCacheState] SET [BrugerTabel] = GETUTCDATE(), [BrugerCount] = [BrugerCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
  				UPDATE [AccessCacheState] SET [BrugerTabel] = GETUTCDATE(), [BrugerCount] = [BrugerCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
        		UPDATE [AccessCacheState] SET [BrugerTabel] = GETUTCDATE(), [BrugerCount] = [BrugerCount] + 1;
	END