﻿CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int out,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Sale] ([CashierId],[SaleDate],[SubTotal],[Tax],[Total])
	VALUES(@CashierId,@SaleDate,@SubTotal,@Tax,@Total)

	SELECT @Id = CAST(SCOPE_IDENTITY() as int);
END
