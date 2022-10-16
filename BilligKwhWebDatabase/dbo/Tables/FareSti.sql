CREATE TABLE [dbo].[FareSti] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT           NOT NULL,
    [StaldID]               INT           NOT NULL,
    [StiNr]                 INT           NOT NULL,
    [Navn]                  NVARCHAR (50) NOT NULL,
    [Tag]                   NVARCHAR (50) NOT NULL,
    [SidstRettet]           DATETIME      CONSTRAINT [DF_FareSti_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT           CONSTRAINT [DF_FareSti_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT           CONSTRAINT [DF_FareSti_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FareSti] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FareSti_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FareSti_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_FareSti_Stald] FOREIGN KEY ([StaldID]) REFERENCES [dbo].[Stald] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_FareSti]
    ON [dbo].[FareSti]([KundeID] ASC, [StaldID] ASC, [Slettet] ASC, [ID] ASC);

