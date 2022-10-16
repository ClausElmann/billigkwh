CREATE TABLE [dbo].[IndstillingEnum] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [Navn]                NVARCHAR (100) NOT NULL,
    [DataTypeID]          INT            NOT NULL,
    [ErBrugerIndstilling] BIT            NOT NULL,
    [Mobile]              BIT            CONSTRAINT [DF_IndstillingEnum_Mobile] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InstillingEnum] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

