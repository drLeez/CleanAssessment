CREATE TABLE [dbo].[PaymentMethodTypes] (
    [PaymentMethodTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [PaymentMethodTypeCode] VARCHAR (255) NOT NULL,
    [PaymentMethodTypeDesc] VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentMethodTypeId] ASC)
);

