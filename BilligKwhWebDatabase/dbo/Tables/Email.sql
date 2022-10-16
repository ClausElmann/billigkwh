CREATE TABLE [dbo].[Email] (
    [ID]                 INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]            INT              NOT NULL,
    [RefTypeID]          INT              CONSTRAINT [DF_Email_RefTypeID] DEFAULT ((0)) NOT NULL,
    [Emne]               NVARCHAR (MAX)   CONSTRAINT [DF_Email_Emne] DEFAULT ('') NOT NULL,
    [Modtager]           NVARCHAR (100)   NOT NULL,
    [Sendt]              DATETIME         CONSTRAINT [DF_EmailLog_Sendt] DEFAULT (getdate()) NULL,
    [Oprettet]           DATETIME         CONSTRAINT [DF_Email_Oprettet] DEFAULT (getdate()) NOT NULL,
    [Indhold]            NVARCHAR (MAX)   CONSTRAINT [DF_Email_Indhold] DEFAULT ('') NULL,
    [Afsender]           NVARCHAR (100)   NOT NULL,
    [ErrorLog]           NVARCHAR (MAX)   CONSTRAINT [DF_Email_ErrorLog] DEFAULT ('') NOT NULL,
    [OprettetAfBrugerID] INT              NOT NULL,
    [AfsenderNavn]       NVARCHAR (MAX)   NULL,
    [SendTidligst]       DATETIME         CONSTRAINT [DF_Email_SendTidligst] DEFAULT (getdate()) NOT NULL,
    [RefGuid]            UNIQUEIDENTIFIER NULL,
    [RefInt]             INT              CONSTRAINT [DF_Email_RefID] DEFAULT ((0)) NULL,
    [KopiTilKs]          BIT              NULL,
    [HelpdeskID]         INT              NULL,
    CONSTRAINT [PK_Email] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Email_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_EmailLog_Bruger] FOREIGN KEY ([OprettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Email]
    ON [dbo].[Email]([RefInt] ASC, [RefGuid] ASC);


GO
CREATE CLUSTERED INDEX [CK_Email]
    ON [dbo].[Email]([KundeID] ASC, [RefTypeID] ASC, [ID] ASC);

