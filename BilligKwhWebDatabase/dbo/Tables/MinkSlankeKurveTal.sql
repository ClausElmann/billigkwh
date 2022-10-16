CREATE TABLE [dbo].[MinkSlankeKurveTal] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [SlankeKurveID]         INT            NOT NULL,
    [Dag]                   INT            NOT NULL,
    [UgeDag]                SMALLINT       CONSTRAINT [DF_MinkSlankeKurveTal_UgeDag] DEFAULT ((1)) NOT NULL,
    [Uge]                   SMALLINT       NOT NULL,
    [Procent]               DECIMAL (7, 4) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkSlankeKurveTal_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkSlankeKurveTal_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkSlankeKurveTal_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkSlankeKurveTal] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkSlankeKurveTal_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkSlankeKurveTal_MinkSlankeKurve] FOREIGN KEY ([SlankeKurveID]) REFERENCES [dbo].[MinkSlankeKurve] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_MinkSlankeKurveTal]
    ON [dbo].[MinkSlankeKurveTal]([SlankeKurveID] ASC, [Dag] ASC);

