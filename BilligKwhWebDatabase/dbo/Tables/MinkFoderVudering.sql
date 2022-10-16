CREATE TABLE [dbo].[MinkFoderVudering] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [FoderCentralID]        INT              CONSTRAINT [DF_MinkFoderVudering_FoderCentralID] DEFAULT ((0)) NOT NULL,
    [BedriftID]             INT              NOT NULL,
    [Dato]                  DATETIME         NOT NULL,
    [SeasonID]              INT              CONSTRAINT [DF_MinkFoderVudering_SeasonID] DEFAULT ((0)) NOT NULL,
    [ForTyndt]              BIT              CONSTRAINT [DF_MinkFoderVudering_ForTyndt] DEFAULT ((0)) NOT NULL,
    [ForTykt]               BIT              CONSTRAINT [DF_MinkFoderVudering_ForTykt] DEFAULT ((0)) NOT NULL,
    [Lugter]                BIT              CONSTRAINT [DF_MinkFoderVudering_Lugter] DEFAULT ((0)) NOT NULL,
    [Styrke]                INT              CONSTRAINT [DF_MinkFoderVudering_Foderstyrke] DEFAULT ((0)) NOT NULL,
    [NaesteLeveringMaengde] INT              CONSTRAINT [DF_MinkFoderVudering_NaesteLeveringMaengde] DEFAULT ((-1)) NOT NULL,
    [LeveretMaengde]        INT              CONSTRAINT [DF_MinkFoderVudering_Maengde1] DEFAULT ((0)) NOT NULL,
    [Maengde]               INT              CONSTRAINT [DF_MinkFoderVudering_NaesteLeveringMaengde1] DEFAULT ((-1)) NOT NULL,
    [FoderPrDyr]            INT              NULL,
    [BrugerDeviceID]        INT              NULL,
    [SidstRettet]           DATETIME         CONSTRAINT [DF_MinkFoderVudering_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_MinkFoderVudering_ObjektGuid] DEFAULT (newid()) NOT NULL,
    [SidstRettetAfBrugerID] INT              CONSTRAINT [DF_MinkFoderVudering_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT              CONSTRAINT [DF_MinkFoderVudering_Slettet] DEFAULT ((0)) NOT NULL,
    [DeviceID]              NVARCHAR (50)    CONSTRAINT [DF_MinkFoderVudering_DeviceID] DEFAULT ('') NULL,
    [BemaerkningCentral]    NVARCHAR (MAX)   CONSTRAINT [DF_MinkFoderVudering_Bemaerkning1] DEFAULT ('') NOT NULL,
    [Bemaerkning]           NVARCHAR (MAX)   CONSTRAINT [DF_MinkFoderVudering_Beskrivelse] DEFAULT ('') NOT NULL,
    [AntalDyr]              INT              NULL,
    CONSTRAINT [PK_MinkFoderVudering] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MinkFoderVudering_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID]),
    CONSTRAINT [FK_MinkFoderVudering_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_MinkFoderVudering_FoderCentral] FOREIGN KEY ([FoderCentralID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkFoderVudering_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID]),
    CONSTRAINT [FK_MinkFoderVudering_MinkSeason] FOREIGN KEY ([SeasonID]) REFERENCES [dbo].[MinkSeason] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_MinkFoderVudering]
    ON [dbo].[MinkFoderVudering]([ObjektGuid] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UI_Bedrift_Dato]
    ON [dbo].[MinkFoderVudering]([BedriftID] ASC, [Dato] ASC);


GO
CREATE CLUSTERED INDEX [CI_MinkFoderVudering]
    ON [dbo].[MinkFoderVudering]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

