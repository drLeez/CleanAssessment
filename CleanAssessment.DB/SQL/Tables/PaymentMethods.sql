CREATE TABLE [dbo].[PaymentMethods] (
    [PaymentMethodId]     INT           IDENTITY (1, 1) NOT NULL,
    [NickName]            VARCHAR (255) NULL,
    [PaymentMethodTypeId] INT           NULL,
    [OwnerId]             INT           NULL,
    [ExpirationDateId]    INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC),
    FOREIGN KEY ([PaymentMethodTypeId]) REFERENCES [dbo].[PaymentMethodTypes] ([PaymentMethodTypeId]),
    FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Customers] ([CustomerId])
);

