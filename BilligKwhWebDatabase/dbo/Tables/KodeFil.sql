CREATE TABLE [dbo].[KodeFil] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Filnavn]     NVARCHAR (256) CONSTRAINT [DF_SprogLabelFil_Filnavn] DEFAULT ('') NOT NULL,
    [SidstRettet] DATETIME       CONSTRAINT [DF_SprogLabelFil_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SprogLabelFil] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_SprogLabelFil]
    ON [dbo].[KodeFil]([Filnavn] ASC);

