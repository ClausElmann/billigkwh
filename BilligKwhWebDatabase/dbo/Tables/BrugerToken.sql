CREATE TABLE [dbo].[BrugerToken] (
    [Token]      UNIQUEIDENTIFIER NOT NULL,
    [BrugerID]   INT              NOT NULL,
    [Expiration] DATETIME         NOT NULL,
    [RememberMe] BIT              CONSTRAINT [DF_BrugerToken_RememberMr] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BrugerToken] PRIMARY KEY NONCLUSTERED ([Token] ASC),
    CONSTRAINT [FK_BrugerToken_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_BrugerToken]
    ON [dbo].[BrugerToken]([BrugerID] ASC, [Token] ASC);

