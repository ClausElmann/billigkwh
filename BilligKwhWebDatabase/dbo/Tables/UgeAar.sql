CREATE TABLE [dbo].[UgeAar] (
    [ID]      INT      NOT NULL,
    [Uge]     SMALLINT NOT NULL,
    [Aar]     SMALLINT NOT NULL,
    [LobeNr]  INT      IDENTITY (1, 1) NOT NULL,
    [Mandag]  DATE     NOT NULL,
    [SoenDag] DATE     NOT NULL,
    CONSTRAINT [PK_UgeAar] PRIMARY KEY CLUSTERED ([ID] ASC)
);

