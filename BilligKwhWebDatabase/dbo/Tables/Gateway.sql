CREATE TABLE [dbo].[Gateway] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [DeviceID]              NVARCHAR (25) CONSTRAINT [DF_Gateway_DeviceUid] DEFAULT ('') NOT NULL,
    [KundeID]               INT           NOT NULL,
    [BedriftID]             INT           CONSTRAINT [DF_OpsamlingBox_BedriftID] DEFAULT ((0)) NOT NULL,
    [Kanal]                 SMALLINT      CONSTRAINT [DF_Gateway_Kanal] DEFAULT ((1)) NOT NULL,
    [SendForsinkelse]       SMALLINT      CONSTRAINT [DF_Gateway_SendForsinkelse] DEFAULT ((0)) NOT NULL,
    [Debug]                 BIT           CONSTRAINT [DF_Gateway_Debug] DEFAULT ((0)) NOT NULL,
    [SidstRettet]           DATETIME      CONSTRAINT [DF_OpsamlingBox_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [SidstRettetAfBrugerID] INT           CONSTRAINT [DF_OpsamlingBox_SidstRettetAfBrugerID] DEFAULT ((0)) NOT NULL,
    [Slettet]               BIT           CONSTRAINT [DF_OpsamlingBox_Slettet] DEFAULT ((0)) NOT NULL,
    [SidstOnline]           DATETIME      NULL,
    [Overvaagning]          BIT           CONSTRAINT [DF_Gateway_Overvaagning] DEFAULT ((1)) NULL,
    [Genstart]              BIT           CONSTRAINT [DF_Gateway_Overvaagning1] DEFAULT ((1)) NULL,
    [ResetFlash]            BIT           NULL,
    CONSTRAINT [PK_Gateway] PRIMARY KEY NONCLUSTERED ([DeviceID] ASC),
    CONSTRAINT [FK_Gateway_Device] FOREIGN KEY ([DeviceID]) REFERENCES [dbo].[Device] ([DeviceID]),
    CONSTRAINT [FK_OpsamlingBox_Bedrift] FOREIGN KEY ([BedriftID]) REFERENCES [dbo].[Bedrift] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Gateway]
    ON [dbo].[Gateway]([SidstRettet] ASC, [Slettet] ASC);


GO
CREATE CLUSTERED INDEX [CI_Gateway]
    ON [dbo].[Gateway]([KundeID] ASC, [DeviceID] ASC);

