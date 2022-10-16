CREATE TABLE [dbo].[BrugerDevice] (
    [ID]                    INT              IDENTITY (1, 1) NOT NULL,
    [KundeID]               INT              NOT NULL,
    [BrugerID]              INT              NOT NULL,
    [DeviceGuid]            UNIQUEIDENTIFIER CONSTRAINT [DF_BrugerDevice_DokumentGuid] DEFAULT (newid()) NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [Model]                 NVARCHAR (250)   CONSTRAINT [DF_BrugerDevice_Model] DEFAULT ('') NULL,
    [Version]               NVARCHAR (250)   CONSTRAINT [DF_BrugerDevice_Version] DEFAULT ('') NULL,
    [Platform]              NVARCHAR (250)   CONSTRAINT [DF_BrugerDevice_Platform] DEFAULT ('') NULL,
    [FarmGainMobileVersion] NVARCHAR (10)    NULL,
    [FarmGainMobileBuild]   NVARCHAR (10)    NULL,
    [DeviceID]              NVARCHAR (50)    NULL,
    CONSTRAINT [PK_BrugerDevice] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BrugerDevice_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_BrugerDevice]
    ON [dbo].[BrugerDevice]([BrugerID] ASC, [KundeID] ASC, [ID] ASC);

