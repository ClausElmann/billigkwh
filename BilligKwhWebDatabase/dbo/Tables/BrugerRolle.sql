CREATE TABLE [dbo].[BrugerRolle] (
    [ID]       INT IDENTITY (1, 1) NOT NULL,
    [KundeID]  INT NOT NULL,
    [BrugerID] INT NOT NULL,
    [RolleID]  INT NOT NULL,
    CONSTRAINT [PK_BrugerRolle] PRIMARY KEY CLUSTERED ([KundeID] ASC, [ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BrugerRolle] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_BrugerRolle_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_BrugerRolle_Rolle] FOREIGN KEY ([RolleID]) REFERENCES [dbo].[Rolle] ([ID])
);


GO

CREATE TRIGGER [dbo].[BrugerRolleAccessCacheUpdate] ON [dbo].[BrugerRolle]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(BrugerID) OR UPDATE(RolleID))
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.BrugerID <> D.BrugerID OR I.RolleID <> D.RolleID))
					BEGIN
        					UPDATE [AccessCacheState] SET [BrugerRolleTabel] = GETUTCDATE(), [BrugerRolleCount] = [BrugerRolleCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [BrugerRolleTabel] = GETUTCDATE(), [BrugerRolleCount] = [BrugerRolleCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [BrugerRolleTabel] = GETUTCDATE(), [BrugerRolleCount] = [BrugerRolleCount] + 1;
	END