CREATE PROCEDURE [dbo].[GetPaymentMethods]
    @CustomerID int
AS
BEGIN
    SELECT
		pm.*,
		pmt.PaymentMethodTypeCode,
		pmt.PaymentMethodTypeDesc
	FROM [dbo].[PaymentMethods] pm
	left join [dbo].[PaymentMethodTypes] pmt on pm.PaymentMethodTypeId = pmt.PaymentMethodTypeId
	WHERE pm.OwnerId = @CustomerID
END