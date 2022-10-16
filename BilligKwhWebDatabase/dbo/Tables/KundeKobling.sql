CREATE TABLE [dbo].[KundeKobling] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [FraKundeID]     INT      NOT NULL,
    [TilKundeID]     INT      NOT NULL,
    [KoblingsTypeID] SMALLINT NOT NULL,
    [Slettet]        BIT      NOT NULL,
    CONSTRAINT [PK_KundeKobling] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_KundeKobling_FraKunde] FOREIGN KEY ([FraKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_KundeKobling_TilKunde] FOREIGN KEY ([TilKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_KundeKobling_Type] FOREIGN KEY ([KoblingsTypeID]) REFERENCES [dbo].[TypeID] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_KundeKobling]
    ON [dbo].[KundeKobling]([KoblingsTypeID] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_KundeKobling]
    ON [dbo].[KundeKobling]([FraKundeID] ASC, [TilKundeID] ASC, [ID] ASC);

