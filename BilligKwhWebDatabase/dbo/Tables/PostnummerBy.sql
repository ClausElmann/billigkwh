CREATE TABLE [dbo].[PostnummerBy] (
    [Postnummer]     SMALLINT      NOT NULL,
    [By]             NVARCHAR (50) COLLATE SQL_Danish_Pref_CP1_CI_AS NOT NULL,
    [SidstRettet]    DATETIME      NOT NULL,
    [PostnummerOgBy] NVARCHAR (54) COLLATE SQL_Danish_Pref_CP1_CI_AS NOT NULL,
    CONSTRAINT [PK_PostnummerBy] PRIMARY KEY CLUSTERED ([Postnummer] ASC)
);

