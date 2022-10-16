CREATE TABLE [dbo].[Rettighed] (
    [ID]                  INT            NOT NULL,
    [Navn]                NVARCHAR (100) NOT NULL,
    [Beskrivelse]         NVARCHAR (250) NOT NULL,
    [Placering]           INT            CONSTRAINT [DF_Rettighed_Placering] DEFAULT ((0)) NOT NULL,
    [ModulID]             INT            NULL,
    [RettighedOmraadeID]  INT            NOT NULL,
    [KunForKundeID]       INT            NULL,
    [RettighedForklaring] NVARCHAR (512) COLLATE SQL_Danish_Pref_CP1_CI_AS NULL,
    [KunKundeRettighed]   BIT            NULL,
    CONSTRAINT [PK_Rettighed] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Rettighed_Kunde] FOREIGN KEY ([KunForKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Rettighed_Modul] FOREIGN KEY ([ModulID]) REFERENCES [dbo].[Modul] ([ID]),
    CONSTRAINT [FK_Rettighed_RettighedOmraade] FOREIGN KEY ([RettighedOmraadeID]) REFERENCES [dbo].[RettighedOmraade] ([ID])
);


GO

CREATE TRIGGER [dbo].[RettighedAccessCacheUpdate] ON [dbo].[Rettighed]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF UPDATE(ModulID) OR UPDATE(KunKundeRettighed)
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.ModulID <> D.ModulID AND I.KunKundeRettighed <> D.KunKundeRettighed))
					BEGIN
        					UPDATE [AccessCacheState] SET [RettighedTabel] = GETUTCDATE(), [RettighedCount] = [RettighedCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [RettighedTabel] = GETUTCDATE(), [RettighedCount] = [RettighedCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [RettighedTabel] = GETUTCDATE(), [RettighedCount] = [RettighedCount] + 1;
	END