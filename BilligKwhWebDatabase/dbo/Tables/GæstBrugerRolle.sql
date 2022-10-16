CREATE TABLE [dbo].[GæstBrugerRolle] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [GæstBrugerID] INT NOT NULL,
    [RolleID]      INT NOT NULL,
    CONSTRAINT [PK_GæstBrugerRolle_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_GæstBrugerRolle_GæsteBruger] FOREIGN KEY ([GæstBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_GæstBrugerRolle_Rolle] FOREIGN KEY ([RolleID]) REFERENCES [dbo].[Rolle] ([ID])
);


GO

CREATE TRIGGER [dbo].[GæstBrugerRolleAccessCacheUpdate] ON [dbo].[GæstBrugerRolle]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(GæstBrugerID) OR UPDATE(RolleID))
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.GæstBrugerID <> D.GæstBrugerID OR I.RolleID <> D.RolleID))
					BEGIN
        					UPDATE [AccessCacheState] SET [GæstBrugerRolleTabel] = GETUTCDATE(), [GæstBrugerRolleCount] = [GæstBrugerRolleCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [GæstBrugerRolleTabel] = GETUTCDATE(), [GæstBrugerRolleCount] = [GæstBrugerRolleCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [GæstBrugerRolleTabel] = GETUTCDATE(), [GæstBrugerRolleCount] = [GæstBrugerRolleCount] + 1;
	END