﻿CREATE TYPE [dbo].[HaendelseTvp] AS TABLE (
    [ID]                    INT              NULL,
    [KundeID]               INT              NULL,
    [Dato]                  DATETIME         NULL,
    [KildeVejeholdDataID]   BIGINT           NULL,
    [VejeholdDataID]        BIGINT           NULL,
    [StaldID]               INT              NULL,
    [SektionID]             INT              NULL,
    [StiID]                 INT              NULL,
    [VejeholdID]            INT              NULL,
    [HoldID]                INT              NULL,
    [HaendelseTypeID]       INT              NULL,
    [Bemaerkning]           NVARCHAR (MAX)   NULL,
    [LbNr]                  SMALLINT         NULL,
    [AntalGalteFjernet]     SMALLINT         NULL,
    [AntalSogriseFjernet]   SMALLINT         NULL,
    [AntalFjernet]          INT              NULL,
    [VaegtFjernet]          INT              NULL,
    [AarsagID]              INT              NULL,
    [Oprettet]              DATETIME         NULL,
    [BrugerDeviceID]        INT              NULL,
    [SidstRettet]           DATETIME         NULL,
    [SidstRettetAfBrugerID] INT              NULL,
    [Slettet]               BIT              NULL,
    [ObjektGuid]            UNIQUEIDENTIFIER NULL,
    [Dag]                   SMALLINT         NULL,
    [Pris]                  DECIMAL (6, 2)   NULL,
    [Udfort]                DATETIME         NULL,
    [UdfortAfBrugerID]      INT              NULL,
    [DeviceID]              NVARCHAR (50)    NULL,
    [SlutAntalGalte]        SMALLINT         NULL,
    [SlutAntalSogrise]      SMALLINT         NULL);

