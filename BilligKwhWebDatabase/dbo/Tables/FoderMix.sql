CREATE TABLE [dbo].[FoderMix] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (100) CONSTRAINT [DF_FoderMix_Navn] DEFAULT ('') NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_FoderMix_Beskrivelse] DEFAULT ('') NOT NULL,
    [Placering]             INT            CONSTRAINT [DF_FoderMix_Placering] DEFAULT ('') NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Foder_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Foder_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Foder_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderMix] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderBlanding_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderBlanding_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_FoderMix]
    ON [dbo].[FoderMix]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

