CREATE TABLE [dbo].[Customers] (
    [CustomerId]            INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]             VARCHAR (100) NOT NULL,
    [MiddleName]            VARCHAR (100) NULL,
    [LastName]              VARCHAR (100) NOT NULL,
    [DuplicateNumber]       INT           NULL,
    [Age]                   INT           NULL,
    [Address]               VARCHAR (500) NULL,
    [AccountCreationDateId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [age_chk] CHECK ([Age] >= (18)),
    CONSTRAINT [dupe_num_chk] CHECK ([DuplicateNumber] IS NULL
                                         OR [DuplicateNumber] > (-1)),
    CONSTRAINT [first_name_chk] CHECK (len([FirstName]) > (0)),
    CONSTRAINT [last_name_chk] CHECK (len([LastName]) > (0)),
    CONSTRAINT [middle_name_chk] CHECK (len([MiddleName]) > (0))
);

