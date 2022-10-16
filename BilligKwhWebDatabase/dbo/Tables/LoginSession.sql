CREATE TABLE [dbo].[LoginSession] (
    [ID]                INT              IDENTITY (1, 1) NOT NULL,
    [BrugernavnUtfCode] UNIQUEIDENTIFIER NOT NULL,
    [SessionID]         NVARCHAR (100)   NOT NULL,
    [LoginUtcTime]      DATETIME         NOT NULL,
    [ExpireUtcTime]     DATETIME         NOT NULL,
    CONSTRAINT [PK_LoginSessions] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_LoginSession_UtcCode]
    ON [dbo].[LoginSession]([BrugernavnUtfCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LoginSession_Session]
    ON [dbo].[LoginSession]([SessionID] ASC);

