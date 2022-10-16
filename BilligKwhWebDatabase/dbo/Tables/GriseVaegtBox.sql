CREATE TABLE [dbo].[GriseVaegtBox] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [BedriftID]             INT            CONSTRAINT [DF_GriseVaegtBox_BedriftID] DEFAULT ((0)) NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_GriseVaegtBox_Beskrivelse] DEFAULT ('') NOT NULL,
    [OrdreNr]               INT            CONSTRAINT [DF_GriseVaegtBox_OrdreNr] DEFAULT ((0)) NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_GriseVaegtBox_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_GriseVaegtBox_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_GriseVaegtBox_Slettet] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GriseVaegtBox] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GriseVaegtBox_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID])
);

