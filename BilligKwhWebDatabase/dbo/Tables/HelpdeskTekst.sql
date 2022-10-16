CREATE TABLE [dbo].[HelpdeskTekst] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [HelpdeskID]            INT            NOT NULL,
    [Tekst]                 NVARCHAR (MAX) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [SidstRettet]           DATETIME       NOT NULL,
    [SidstRettetAfBrugerID] INT            NOT NULL,
    [IsEnviDanTekst]        BIT            NOT NULL,
    [TimeForbrug]           FLOAT (53)     CONSTRAINT [DF_HelpdeskTekst_TimeForbrug] DEFAULT ((0)) NULL,
    [ProjektNummer]         NVARCHAR (100) NULL,
    [AnsvarligBrugerID]     INT            NULL,
    CONSTRAINT [PK_HelpdeskTekst] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_HelpdeskTekst_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_HelpdeskTekst_Bruger1] FOREIGN KEY ([AnsvarligBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_HelpdeskTekst_Helpdesk] FOREIGN KEY ([HelpdeskID]) REFERENCES [dbo].[Helpdesk] ([ID]),
    CONSTRAINT [FK_HelpdeskTekst_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);

