CREATE TABLE [dbo].[MobileDevice] (
    [DeviceID]              UNIQUEIDENTIFIER NOT NULL,
    [KundeID]               INT              NOT NULL,
    [BrugerID]              INT              NOT NULL,
    [Oprettet]              DATETIME         NOT NULL,
    [SidstRettet]           DATETIME         NOT NULL,
    [Model]                 NVARCHAR (250)   CONSTRAINT [DF_MobileDevice_Model] DEFAULT ('') NULL,
    [Version]               NVARCHAR (250)   CONSTRAINT [DF_MobileDevice_Version] DEFAULT ('') NULL,
    [Platform]              NVARCHAR (250)   CONSTRAINT [DF_MobileDevice_Platform] DEFAULT ('') NULL,
    [FarmGainMobileVersion] NVARCHAR (10)    NULL,
    [FarmGainMobileBuild]   NVARCHAR (10)    NULL,
    CONSTRAINT [PK_MobileDevice] PRIMARY KEY NONCLUSTERED ([DeviceID] ASC),
    CONSTRAINT [FK_MobileDevice_Bruger] FOREIGN KEY ([BrugerID]) REFERENCES [dbo].[Bruger] ([ID])
);


GO
CREATE CLUSTERED INDEX [CI_MobileDevice]
    ON [dbo].[MobileDevice]([KundeID] ASC, [BrugerID] ASC, [DeviceID] ASC);

