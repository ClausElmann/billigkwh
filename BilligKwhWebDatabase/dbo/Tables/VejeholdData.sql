CREATE TABLE [dbo].[VejeholdData] (
    [ID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [UniqueID]             NVARCHAR (50)  NOT NULL,
    [VejeholdNr]           SMALLINT       NOT NULL,
    [LbNr]                 SMALLINT       NOT NULL,
    [KundeID]              INT            CONSTRAINT [DF_RawFeedDataNy_KundeID] DEFAULT ((0)) NOT NULL,
    [IFVersion]            SMALLINT       NOT NULL,
    [Dato]                 DATETIME       CONSTRAINT [DF_RawFeedDataNy_Dato] DEFAULT (getdate()) NOT NULL,
    [Data]                 NVARCHAR (MAX) NOT NULL,
    [Modtaget]             DATETIME       CONSTRAINT [DF_RawFeedDataNy_Modtaget] DEFAULT (getdate()) NOT NULL,
    [Importeret]           BIT            CONSTRAINT [DF_RawFeedDataNy_Importeret] DEFAULT ((0)) NULL,
    [TimeNrID]             SMALLINT       CONSTRAINT [DF_RawFeedData_Dag] DEFAULT ((0)) NOT NULL,
    [VejeholdID]           INT            CONSTRAINT [DF_RawFeedDataNy_VejeholdID] DEFAULT ((0)) NOT NULL,
    [Foder]                INT            CONSTRAINT [DF_Table_1_FoderKg] DEFAULT ((0)) NOT NULL,
    [Vand]                 INT            CONSTRAINT [DF_Table_1_VandL] DEFAULT ((0)) NOT NULL,
    [Mix1]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix1] DEFAULT ((0)) NOT NULL,
    [Mix2]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix2] DEFAULT ((0)) NOT NULL,
    [Mix3]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix3] DEFAULT ((0)) NOT NULL,
    [Mix4]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix4] DEFAULT ((0)) NOT NULL,
    [Mix5]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix5] DEFAULT ((0)) NOT NULL,
    [Mix6]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix6] DEFAULT ((0)) NOT NULL,
    [Mix7]                 INT            CONSTRAINT [DF_RawFeedDataNy_Mix7] DEFAULT ((0)) NOT NULL,
    [FoderDelta]           INT            CONSTRAINT [DF_RawFeedDataNy_DeltaFoder] DEFAULT ((-1)) NOT NULL,
    [VandDelta]            INT            CONSTRAINT [DF_RawFeedDataNy_DeltaVand] DEFAULT ((-1)) NOT NULL,
    [Mix1Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix11] DEFAULT ((0)) NOT NULL,
    [Mix2Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix21] DEFAULT ((0)) NOT NULL,
    [Mix3Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix31] DEFAULT ((0)) NOT NULL,
    [Mix4Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix41] DEFAULT ((0)) NOT NULL,
    [Mix5Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix51] DEFAULT ((0)) NOT NULL,
    [Mix6Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix61] DEFAULT ((0)) NOT NULL,
    [Mix7Delta]            INT            CONSTRAINT [DF_RawFeedData_Mix71] DEFAULT ((0)) NOT NULL,
    [FoderMixVersionID]    INT            CONSTRAINT [DF_RawFeedDataNy_FoderMixID] DEFAULT ((0)) NOT NULL,
    [VejeholdVejningID]    INT            NULL,
    [ErFoderskift]         BIT            NULL,
    [FoderGramTimeDyr]     DECIMAL (8, 4) CONSTRAINT [DF_VejeholdData_FoderTimeDyr] DEFAULT ((0)) NOT NULL,
    [VandMlTimeDyr]        DECIMAL (8, 4) CONSTRAINT [DF_VejeholdData_FoderTimeDyr1] DEFAULT ((0)) NOT NULL,
    [TilvaekstGramTimeDyr] DECIMAL (8, 4) CONSTRAINT [DF_VejeholdData_FoderTimeDyr2] DEFAULT ((0)) NOT NULL,
    [Redigeret]            BIT            CONSTRAINT [DF_VejeholdData_Redigeret] DEFAULT ((0)) NOT NULL,
    [Vaegt]                INT            CONSTRAINT [DF_VejeholdData_VaegtModtaget1] DEFAULT ((-1)) NOT NULL,
    [AntalGalte]           SMALLINT       CONSTRAINT [DF_VejeholdData_AntalModtaget1] DEFAULT ((0)) NOT NULL,
    [AntalSogrise]         SMALLINT       CONSTRAINT [DF_VejeholdData_AntalGalte1] DEFAULT ((0)) NOT NULL,
    [MixNr]                SMALLINT       CONSTRAINT [DF_VejeholdData_MixNrModtaget1] DEFAULT ((-1)) NOT NULL,
    [TilvaegstGramTimeDyr] DECIMAL (8, 4) CONSTRAINT [DF_VejeholdData_TilvaegstGramTimeDyr1] DEFAULT ((0)) NULL,
    [CopyToNew]            BIT            CONSTRAINT [DF_VejeholdData_copyToNew] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_RawFeedDataNy] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_RawFeedData_FoderMixVersion] FOREIGN KEY ([FoderMixVersionID]) REFERENCES [dbo].[FoderMixVersion] ([ID]),
    CONSTRAINT [FK_RawFeedData_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_RawFeedData_TimeNr] FOREIGN KEY ([TimeNrID]) REFERENCES [dbo].[TimeNr] ([ID]),
    CONSTRAINT [FK_RawFeedDataNy_Vejehold] FOREIGN KEY ([VejeholdID]) REFERENCES [dbo].[Vejehold] ([ID]),
    CONSTRAINT [FK_VejeholdData_VejeholdVejning] FOREIGN KEY ([VejeholdVejningID]) REFERENCES [dbo].[VejeholdVejning] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_267_266_VejeholdData]
    ON [dbo].[VejeholdData]([UniqueID] ASC, [VejeholdNr] ASC, [LbNr] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VejeholdData_LbNr]
    ON [dbo].[VejeholdData]([VejeholdID] ASC, [LbNr] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [Index1]
    ON [dbo].[VejeholdData]([VejeholdID] ASC, [FoderMixVersionID] ASC);


GO
CREATE UNIQUE CLUSTERED INDEX [CI_RawFeedData]
    ON [dbo].[VejeholdData]([KundeID] ASC, [UniqueID] ASC, [VejeholdNr] ASC, [LbNr] ASC);

