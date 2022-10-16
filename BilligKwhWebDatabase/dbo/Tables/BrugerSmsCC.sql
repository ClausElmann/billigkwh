CREATE TABLE [dbo].[BrugerSmsCC] (
    [ID]       INT      IDENTITY (1, 1) NOT NULL,
    [BrugerID] INT      NOT NULL,
    [SMS]      CHAR (8) CONSTRAINT [DF_Table_1_VarslingSMS] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BrugerCcSms] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BrugerSmsCC_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);

