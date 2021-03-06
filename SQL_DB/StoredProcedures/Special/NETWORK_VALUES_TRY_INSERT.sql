CREATE PROCEDURE [dbo].[NETWORK_VALUE_TRY_INSERT]
	@VALUE FLOAT
AS
	
	DECLARE @RETURN INT;

	IF(EXISTS(SELECT Id FROM [DBO].NETWORK_VALUES WHERE VALUE = @VALUE))
	BEGIN
		SET @RETURN = (SELECT TOP(1) Id FROM [DBO].NETWORK_VALUES WHERE VALUE = @VALUE)
	END
	ELSE
	BEGIN
		INSERT INTO [DBO].NETWORK_VALUES (VALUE)
		VALUES(@VALUE)

		SET @RETURN = SCOPE_IDENTITY();
	END


RETURN @RETURN
