CREATE TABLE [dbo].[Rolle] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]     INT            NOT NULL,
    [Navn]        NVARCHAR (100) NOT NULL,
    [Beskrivelse] NVARCHAR (250) NOT NULL,
    [Placering]   INT            CONSTRAINT [DF_Rolle_Placering] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Rolle] PRIMARY KEY NONCLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Rolle_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Rolle]
    ON [dbo].[Rolle]([KundeID] ASC, [ID] ASC);


GO

CREATE TRIGGER [dbo].[RolleAccessCacheUpdate] ON [dbo].[Rolle]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF UPDATE(KundeID)
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.KundeID <> D.KundeID))
					BEGIN
        					UPDATE [AccessCacheState] SET [RolleTabel] = GETUTCDATE(), [RolleCount] = [RolleCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [RolleTabel] = GETUTCDATE(), [RolleCount] = [RolleCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [RolleTabel] = GETUTCDATE(), [RolleCount] = [RolleCount] + 1;
	END