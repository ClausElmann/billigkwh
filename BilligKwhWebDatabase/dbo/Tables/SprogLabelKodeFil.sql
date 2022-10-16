CREATE TABLE [dbo].[SprogLabelKodeFil] (
    [SporgLabelFilID] INT      NOT NULL,
    [SprogLabelID]    INT      NOT NULL,
    [SidstRettet]     DATETIME CONSTRAINT [DF_KodeFil_SidstRettet] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SprogLabelKodeFil] PRIMARY KEY CLUSTERED ([SporgLabelFilID] ASC, [SprogLabelID] ASC)
);

