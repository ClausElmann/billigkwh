CREATE TABLE [dbo].[Print] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [PrintId]              NVARCHAR (250) NOT NULL,
    [KundeId]              INT            NULL,
    [OprettetDatoUtc]      DATETIME       NOT NULL,
    [SidsteKontaktDatoUtc] DATETIME       NOT NULL,
    [Lokation]             NVARCHAR (200) CONSTRAINT [DF_Print_Lokation] DEFAULT ('') NOT NULL,
    [Slettet]              DATETIME       NULL,
    [Kommentar]            NVARCHAR (MAX) CONSTRAINT [DF_Print_Kommentar] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Print] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [CX_Print]
    ON [dbo].[Print]([PrintId] ASC);

