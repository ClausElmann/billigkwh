CREATE TABLE [dbo].[FoderVaegt] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]                 INT            NOT NULL,
    [Navn]                    NVARCHAR (50)  NOT NULL,
    [Beskrivelse]             NVARCHAR (MAX) CONSTRAINT [DF_FoderVaegt_Beskrivelse] DEFAULT ('') NOT NULL,
    [Sti1ID]                  INT            CONSTRAINT [DF_FoderVaegt_Sti1ID] DEFAULT ((0)) NOT NULL,
    [Sti2ID]                  INT            CONSTRAINT [DF_FoderVaegt_Sti2ID] DEFAULT ((0)) NOT NULL,
    [GensendFraID]            INT            CONSTRAINT [DF_FoderVaegt_HentFraID] DEFAULT ((0)) NOT NULL,
    [OpsaetningData]          NVARCHAR (MAX) CONSTRAINT [DF_FoderVaegt_OpsaetningData] DEFAULT ('') NULL,
    [OpsaetningModtaget]      DATETIME       CONSTRAINT [DF_FoderVaegt_SidstRettet1] DEFAULT (getdate()) NULL,
    [OrdreNr]                 INT            CONSTRAINT [DF_FoderVaegt_OrdreNr] DEFAULT ((0)) NULL,
    [FoderBegraensningPlanID] INT            CONSTRAINT [DF_FoderVaegt_FoderBegraensningPlanID] DEFAULT ((0)) NOT NULL,
    [SidstRettet]             DATETIME       CONSTRAINT [DF_FoderVaegt_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID]   INT            CONSTRAINT [DF_FoderVaegt_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]                 BIT            CONSTRAINT [DF_FoderVaegt_Slettet] DEFAULT ((0)) NOT NULL,
    [FoderMixNr]              SMALLINT       CONSTRAINT [DF_FoderVaegt_OnsketFoderMixNr] DEFAULT ((0)) NOT NULL,
    [FoderAnlaegID]           INT            CONSTRAINT [DF_FoderVaegt_FoderBegraensningPlanID1] DEFAULT ((0)) NOT NULL,
    [SidstOnline]             DATETIME       NULL,
    [Overvaagning]            BIT            CONSTRAINT [DF_FoderVaegt_Overvaagning] DEFAULT ((1)) NOT NULL,
    [MailAdvisering]          BIT            CONSTRAINT [DF_FoderVaegt_MailAdvisering] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FoderVaegt] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FoderVaegt_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FoderVaegt_FoderAnlaeg] FOREIGN KEY ([FoderAnlaegID]) REFERENCES [dbo].[FoderAnlaeg] ([ID]),
    CONSTRAINT [FK_FoderVaegt_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_FoderVaegt_Sti1] FOREIGN KEY ([Sti1ID]) REFERENCES [dbo].[Sti] ([ID]),
    CONSTRAINT [FK_FoderVaegt_Sti2] FOREIGN KEY ([Sti2ID]) REFERENCES [dbo].[Sti] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_FoderVaegt]
    ON [dbo].[FoderVaegt]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

