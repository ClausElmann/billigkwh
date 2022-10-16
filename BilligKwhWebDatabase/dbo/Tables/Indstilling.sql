CREATE TABLE [dbo].[Indstilling] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT             NOT NULL,
    [BrugerID]              INT             NULL,
    [InstillingEnumID]      INT             NOT NULL,
    [_Int]                  INT             NULL,
    [_DateTime]             DATETIME        NULL,
    [_String]               NVARCHAR (MAX)  NULL,
    [_Double]               FLOAT (53)      NULL,
    [_Bool]                 BIT             NULL,
    [_Blob]                 VARBINARY (MAX) NULL,
    [SidstRettet]           DATETIME        CONSTRAINT [DF_Indstilling_SidstRettet] DEFAULT (getutcdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT             CONSTRAINT [DF_Indstilling_SidstRettetAfBrugerID] DEFAULT ((-1)) NOT NULL,
    [Slettet]               BIT             NOT NULL,
    CONSTRAINT [PK_Indstilling] PRIMARY KEY NONCLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Aktion_Indstilling] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Indstilling] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Indstilling_IndstillingEnum] FOREIGN KEY ([InstillingEnumID]) REFERENCES [dbo].[IndstillingEnum] ([ID])
);




GO
CREATE CLUSTERED INDEX [CK_Indstilling]
    ON [dbo].[Indstilling]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

