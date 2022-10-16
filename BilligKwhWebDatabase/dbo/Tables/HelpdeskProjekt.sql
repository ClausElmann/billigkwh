CREATE TABLE [dbo].[HelpdeskProjekt] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [HelpdeskProjektNavn] NVARCHAR (100) NOT NULL,
    [Beskrivelse]         NVARCHAR (MAX) CONSTRAINT [DF_HelpdeskProjekter_Beskrivelse] DEFAULT ('') NOT NULL,
    [Slettet]             BIT            CONSTRAINT [DF_HelpdeskProjekter_Slettet] DEFAULT ((0)) NOT NULL,
    [ProjektNummer]       NVARCHAR (50)  CONSTRAINT [DF_HelpdeskProjekt_ProjektNummer] DEFAULT ('') NOT NULL,
    [Afsluttet]           BIT            CONSTRAINT [DF_HelpdeskProjekt_Afsluttet] DEFAULT ('0') NOT NULL,
    CONSTRAINT [PK_HelpdeskProjekter] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

