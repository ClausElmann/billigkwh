CREATE TABLE [dbo].[KundeEksponering] (
    [ID]                 INT IDENTITY (1, 1) NOT NULL,
    [KundeID]            INT NOT NULL,
    [EksponerForKundeID] INT NOT NULL,
    CONSTRAINT [PK_KundeExponering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_KundeExponering_ExponeringForKunde] FOREIGN KEY ([EksponerForKundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_KundeExponering_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_KundeExponering]
    ON [dbo].[KundeEksponering]([KundeID] ASC, [ID] ASC);

