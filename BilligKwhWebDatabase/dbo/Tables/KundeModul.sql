CREATE TABLE [dbo].[KundeModul] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]       INT           NOT NULL,
    [ModulID]       INT           NOT NULL,
    [Projektnummer] NVARCHAR (20) CONSTRAINT [DF_KundeModul_Projektnummer] DEFAULT ('') NOT NULL,
    [Antal]         INT           CONSTRAINT [DF_KundeModul_Antal] DEFAULT ((1)) NOT NULL,
    [Pris]          INT           CONSTRAINT [DF_KundeModul_Pris] DEFAULT ((0)) NOT NULL,
    [PrisService]   INT           CONSTRAINT [DF_KundeModul_Pris1] DEFAULT ((0)) NOT NULL,
    [PrisHosting]   INT           CONSTRAINT [DF_KundeModul_PrisSupport1] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KundeModul] PRIMARY KEY NONCLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_KundeModul] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_KundeModul_Modul] FOREIGN KEY ([ModulID]) REFERENCES [dbo].[Modul] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_KundeModul]
    ON [dbo].[KundeModul]([KundeID] ASC, [ModulID] ASC, [ID] ASC);


GO

CREATE TRIGGER [dbo].[KundeModulAccessCacheUpdate] ON [dbo].[KundeModul]
    AFTER INSERT, UPDATE, DELETE
AS
    SET NOCOUNT ON 
        
	IF EXISTS (SELECT 1 FROM inserted)
	BEGIN
	  IF EXISTS (SELECT 1 FROM deleted)
	  BEGIN
		-- I am an update
			IF UPDATE(ModulID)
			BEGIN
				/* 
				this verifies that the column actually changed in value.  As a IF UPDATE() will return true if you execute 
				a statement LIKE this UPDATE TABLE SET COLUMN = COLUMN which is not a change.
				*/
				IF EXISTS(SELECT 1 FROM inserted I JOIN deleted D ON I.ID = D.ID AND (I.ModulID <> D.ModulID))
					BEGIN
        					UPDATE [AccessCacheState] SET [KundeModulTabel] = GETUTCDATE(), [KundeModulCount] = [KundeModulCount] + 1;
					END 
			END  
	  END
	  ELSE
	  BEGIN
		-- I am an insert
		UPDATE [AccessCacheState] SET [KundeModulTabel] = GETUTCDATE(), [KundeModulCount] = [KundeModulCount] + 1;
	  END
	END
	ELSE
	BEGIN
	  -- I am a delete
	  UPDATE [AccessCacheState] SET [KundeModulTabel] = GETUTCDATE(), [KundeModulCount] = [KundeModulCount] + 1;
	END