CREATE TABLE [dbo].[MinkTypeStd] (
    [ID]                     INT IDENTITY (1, 1) NOT NULL,
    [ListePostID]            INT NOT NULL,
    [VedligeholdKcalHun]     INT CONSTRAINT [DF_MinkTypeStd_VedligeholdKcalHun] DEFAULT ((0)) NOT NULL,
    [VaegtaendringDagligHun] INT CONSTRAINT [DF_MinkTypeStd_VaegtaendringDagligHun] DEFAULT ((0)) NOT NULL,
    [VedligeholdKcalHan]     INT CONSTRAINT [DF_MinkTypeStd_VedligeholdKcalHan] DEFAULT ((0)) NOT NULL,
    [VaegtaendringDagligHan] INT CONSTRAINT [DF_MinkTypeStd_VaegtaendringDagligHan] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MinkTypeStd] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkTypeStd_ListePost] FOREIGN KEY ([ListePostID]) REFERENCES [dbo].[ListePost] ([ID])
);

