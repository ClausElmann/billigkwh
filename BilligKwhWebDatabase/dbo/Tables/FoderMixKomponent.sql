CREATE TABLE [dbo].[FoderMixKomponent] (
    [ID]                INT      IDENTITY (1, 1) NOT NULL,
    [FoderMixVersionID] INT      NOT NULL,
    [FoderIngrediensID] INT      CONSTRAINT [DF_Table_1_Navn] DEFAULT ('') NOT NULL,
    [Maengde]           SMALLINT CONSTRAINT [DF_Table_1_Beskrivelse] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FoderMixIngridiens] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderMixIngrediens_FoderIngrediens] FOREIGN KEY ([FoderIngrediensID]) REFERENCES [dbo].[FoderKomponent] ([ID]),
    CONSTRAINT [FK_FoderMixIngrediens_FoderMixVersion] FOREIGN KEY ([FoderMixVersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_FoderMixIngrediens]
    ON [dbo].[FoderMixKomponent]([FoderMixVersionID] ASC, [FoderIngrediensID] ASC);

