CREATE TABLE [dbo].[BrugerKunde] (
    [ID]       INT IDENTITY (1, 1) NOT NULL,
    [BrugerID] INT NULL,
    [KundeID]  INT NULL,
    CONSTRAINT [PK_BrugerKunde] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO

create TRIGGER [dbo].[BrugerKundeAccessCacheUpdate] ON [dbo].[BrugerKunde]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 

	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF (UPDATE(KundeID) OR UPDATE(BrugerID)) 
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
			IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.KundeID <> D.KundeID OR I.BrugerID <> D.BrugerID))
     				BEGIN
        					UPDATE [AccessCacheState] SET [BrugerKundeTabel] = GETUTCDATE(), [BrugerKundeCount] = [BrugerKundeCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [BrugerKundeTabel] = GETUTCDATE(), [BrugerKundeCount] = [BrugerKundeCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [BrugerKundeTabel] = GETUTCDATE(), [BrugerKundeCount] = [BrugerKundeCount] + 1;
	END