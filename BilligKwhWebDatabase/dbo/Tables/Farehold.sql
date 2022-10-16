CREATE TABLE [dbo].[Farehold] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT           NOT NULL,
    [SoID]                  INT           NOT NULL,
    [FareStiID]             INT           NOT NULL,
    [Navn]                  NVARCHAR (50) NOT NULL,
    [StartDato]             DATETIME      NOT NULL,
    [SlutDato]              DATETIME      NOT NULL,
    [SidstRettet]           DATETIME      CONSTRAINT [DF_Farehold_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT           CONSTRAINT [DF_Farehold_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT           CONSTRAINT [DF_Farehold_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Farehold] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Farehold_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Farehold_FareSti] FOREIGN KEY ([FareStiID]) REFERENCES [dbo].[FareSti] ([ID]),
    CONSTRAINT [FK_Farehold_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Farehold_So] FOREIGN KEY ([SoID]) REFERENCES [dbo].[So] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Farehold]
    ON [dbo].[Farehold]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

