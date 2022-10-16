CREATE TABLE [dbo].[Filtype] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [FiltypeNavn]        NVARCHAR (5)   NOT NULL,
    [MIMEtype]           NVARCHAR (100) NOT NULL,
    [FiltypeBeskrivelse] NVARCHAR (50)  NOT NULL,
    [FiltypeIkonNavn]    NVARCHAR (20)  NOT NULL,
    [SidstRettet]        DATETIME       CONSTRAINT [DF_Filtype_SidstRettet] DEFAULT (getdate()) NOT NULL,
    [ErBillede]          BIT            CONSTRAINT [DF_Filtype_ErBillede] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Filtype] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

