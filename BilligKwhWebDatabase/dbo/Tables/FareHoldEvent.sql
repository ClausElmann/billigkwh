CREATE TABLE [dbo].[FareHoldEvent] (
    [ID]                    INT      IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT      NOT NULL,
    [FareHoldID]            INT      NOT NULL,
    [Dato]                  INT      NOT NULL,
    [Maengde]               INT      NOT NULL,
    [EventTypeID]           INT      NOT NULL,
    [SidstRettet]           DATETIME NOT NULL,
    [SidstRettetAfBrugerID] INT      NOT NULL,
    [Slettet]               BIT      NOT NULL,
    CONSTRAINT [PK_FareHoldEvent] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FareHoldEvent_Bruger] FOREIGN KEY ([SidstRettetAfBrugerID]) REFERENCES [dbo].[Bruger] ([ID]),
    CONSTRAINT [FK_FareHoldEvent_EventType] FOREIGN KEY ([EventTypeID]) REFERENCES [dbo].[HaendelseType] ([ID]),
    CONSTRAINT [FK_FareHoldEvent_Farehold] FOREIGN KEY ([FareHoldID]) REFERENCES [dbo].[Farehold] ([ID]),
    CONSTRAINT [FK_FareHoldEvent_Kunde] FOREIGN KEY ([KundeID]) REFERENCES [dbo].[Kunde] ([ID])
);


GO
CREATE CLUSTERED INDEX [CK_FareHoldEvent]
    ON [dbo].[FareHoldEvent]([KundeID] ASC, [Slettet] ASC, [ID] ASC);

