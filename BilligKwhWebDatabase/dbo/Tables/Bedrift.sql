CREATE TABLE [dbo].[Bedrift] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT            NOT NULL,
    [Navn]                  NVARCHAR (50)  NOT NULL,
    [Beskrivelse]           NVARCHAR (MAX) CONSTRAINT [DF_Bedrift_Adresse1] DEFAULT ('') NOT NULL,
    [Adresse]               NVARCHAR (MAX) CONSTRAINT [DF_Bedrift_Adresse] DEFAULT ('') NOT NULL,
    [PostNr]                SMALLINT       CONSTRAINT [DF_Bedrift_PostNr] DEFAULT ((0)) NOT NULL,
    [KontaktBrugerID]       INT            CONSTRAINT [DF_Bedrift_AnsvarligBrugerID1] DEFAULT ((0)) NOT NULL,
    [AnsvarligBrugerID]     INT            CONSTRAINT [DF_Bedrift_SidstRettetAfBrugerID1] DEFAULT ((0)) NOT NULL,
    [TidzoneID]             SMALLINT       CONSTRAINT [DF_Bedrift_TidzoneID] DEFAULT ((40)) NOT NULL,
    [Lat]                   FLOAT (53)     CONSTRAINT [DF_Bedrift_Lat] DEFAULT ((0)) NOT NULL,
    [Lon]                   FLOAT (53)     CONSTRAINT [DF_Bedrift_Lon] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME       CONSTRAINT [DF_Bedrift_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT            CONSTRAINT [DF_Bedrift_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT            CONSTRAINT [DF_Bedrift_Slettet] DEFAULT ((0)) NOT NULL,
    [BrancheTypeID]         SMALLINT       CONSTRAINT [DF_Bedrift_BrancheTypeID] DEFAULT ((1)) NOT NULL,
    [FoderCentralID]        INT            CONSTRAINT [DF_Bedrift_FoderCentralID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bedrift] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Bedrift_AnsvarligBruger] FOREIGN KEY ([AnsvarligBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Bedrift_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Bedrift_Bruger1] FOREIGN KEY ([KontaktBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_Bedrift_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_Bedrift_PostnummerBy] FOREIGN KEY ([PostNr]) REFERENCES [dbo].[PostnummerBy] ([Postnummer]),
    CONSTRAINT [FK_Bedrift_Tidzone] FOREIGN KEY ([TidzoneID]) REFERENCES [dbo].[Tidzone] ([ID]),
    CONSTRAINT [FK_Bedrift_Type] FOREIGN KEY ([BrancheTypeID]) REFERENCES [dbo].[TypeID] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_Bedrift]
    ON [dbo].[Bedrift]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

