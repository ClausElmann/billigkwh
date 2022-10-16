CREATE TABLE [dbo].[MinkSlankeKurve] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_MinkSlankeKurve_Beskrivelse] DEFAULT ('') NOT NULL,
    [Navn]                  NVARCHAR (150) CONSTRAINT [DF_MinkSlankeKurve_Opskrift] DEFAULT ('') NOT NULL,
    [HoldTypeID]            SMALLINT       CONSTRAINT [DF_MinkSlankeKurve_HoldTypeID] DEFAULT ((3)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_MinkSlankeKurve_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_MinkSlankeKurve_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_MinkSlankeKurve_Slettet] DEFAULT ((0)) NOT NULL,
    [StartUge]              INT            CONSTRAINT [DF_MinkSlankeKurve_StartUge] DEFAULT ((48)) NOT NULL,
    [LabelKoen]             NVARCHAR (6)   CONSTRAINT [DF_MinkSlankeKurve_LabelKoen] DEFAULT (N'L80121') NOT NULL,
    CONSTRAINT [PK_MinkSlankeKurve] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkSlankeKurve_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkSlankeKurve_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkSlankeKurve_MinkVejeholdholdType] FOREIGN KEY ([HoldTypeID]) REFERENCES [dbo].[MinkVejeholdholdType] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MinkSlankeKurve]
    ON [dbo].[MinkSlankeKurve]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

