CREATE TABLE [dbo].[GæstKundeRolle] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [GæstKundeID] INT NOT NULL,
    [RolleID]     INT NOT NULL,
    CONSTRAINT [PK_GæstKundeRolle_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_GæsteKundeRolle_GæsteBruger] FOREIGN KEY ([RolleID]) REFERENCES [dbo].[Rolle] ([ID]),
    CONSTRAINT [FK_GæsteKundeRolle_GæstKunde] FOREIGN KEY ([GæstKundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO

CREATE TRIGGER [dbo].[GæstKundeRolleAccessCacheUpdate] ON [dbo].[GæstKundeRolle]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(GæstKundeID) OR UPDATE(RolleID))
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.GæstKundeID <> D.GæstKundeID OR I.RolleID <> D.RolleID))
					BEGIN
        					UPDATE [AccessCacheState] SET [GæstKundeRolleTabel] = GETUTCDATE(), [GæstKundeRolleCount] = [GæstKundeRolleCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [GæstKundeRolleTabel] = GETUTCDATE(), [GæstKundeRolleCount] = [GæstKundeRolleCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [GæstKundeRolleTabel] = GETUTCDATE(), [GæstKundeRolleCount] = [GæstKundeRolleCount] + 1;
	END