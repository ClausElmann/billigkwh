CREATE TABLE [dbo].[Sms] (
    [ID]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [KundeID]            INT            NOT NULL,
    [ModtagerNr]         CHAR (8)       NULL,
    [ModtagerAdresse]    NVARCHAR (60)  NULL,
    [Besked]             NVARCHAR (MAX) CONSTRAINT [DF_Sms_Tekst] DEFAULT ('') NOT NULL,
    [Oprettet]           DATETIME       CONSTRAINT [DF_Sms_Sendt] DEFAULT (getdate()) NOT NULL,
    [OprettetAfBrugerID] INT            NOT NULL,
    [Sendt]              DATETIME       CONSTRAINT [DF_Sms_Sendt_1] DEFAULT (getdate()) NULL,
    [ErrorLog]           NVARCHAR (MAX) CONSTRAINT [DF_Sms_ErrorLog] DEFAULT ('') NOT NULL,
    [SendTidligst]       DATETIME       CONSTRAINT [DF_Sms_Oprettet1] DEFAULT (getdate()) NOT NULL,
    [AfsenderNavn]       NVARCHAR (11)  CONSTRAINT [DF_Sms_VarslingSmsAfsenderNavn] DEFAULT ('SMS Service') NOT NULL,
    CONSTRAINT [PK_Sms] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Sms_Bruger] FOREIGN KEY ([OprettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Sms_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Sms]
    ON [dbo].[Sms]([KundeID] ASC, [ID] ASC);

