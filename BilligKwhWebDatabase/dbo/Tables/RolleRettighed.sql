CREATE TABLE [dbo].[RolleRettighed] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [KundeID]          INT NOT NULL,
    [RolleID]          INT NOT NULL,
    [RettighedID]      INT NOT NULL,
    [RettighedsTypeID] INT NOT NULL,
    CONSTRAINT [PK_RolleRettighed_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_RolleRettighed_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_RolleRettighed_Rettighed] FOREIGN KEY ([RettighedID]) REFERENCES [dbo].[Rettighed] ([ID]),
    CONSTRAINT [FK_RolleRettighed_Rolle] FOREIGN KEY ([RolleID]) REFERENCES [dbo].[Rolle] ([ID])
);


GO

CREATE TRIGGER [dbo].[RolleRettighedAccessCacheUpdate] ON [dbo].[RolleRettighed]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF UPDATE(RettighedsTypeID)
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.RettighedsTypeID <> D.RettighedsTypeID))
					BEGIN
        					UPDATE [AccessCacheState] SET [RolleRettighedTabel] = GETUTCDATE(), [RolleRettighedCount] = [RolleRettighedCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [RolleRettighedTabel] = GETUTCDATE(), [RolleRettighedCount] = [RolleRettighedCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [RolleRettighedTabel] = GETUTCDATE(), [RolleRettighedCount] = [RolleRettighedCount] + 1;
	END