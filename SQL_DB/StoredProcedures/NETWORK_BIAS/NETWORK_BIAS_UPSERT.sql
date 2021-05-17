CREATE PROCEDURE [dbo].[NETWORK_BIAS_UPSERT]
	@ID INT OUTPUT,
	@INTERNAL_INDEX INT,
	@VALUE FLOAT,
	@LAYER_ID INT
AS
BEGIN
DECLARE @VALUE_ID INT
EXEC @VALUE_ID = [dbo].[NETWORK_VALUE_TRY_INSERT] @VALUE = @VALUE
IF EXISTS(SELECT * FROM NETWORK_BIAS WHERE ID=@ID)
BEGIN
	UPDATE NETWORK_BIAS SET 
	INTERNAL_INDEX = @INTERNAL_INDEX, VALUE_ID = @VALUE_ID, LAYER_ID = @LAYER_ID
	WHERE ID=@ID
	SET @ID = SCOPE_IDENTITY();
	RETURN @ID
END
ELSE
BEGIN
	INSERT INTO NETWORK_BIAS VALUES(@INTERNAL_INDEX, @VALUE, @LAYER_ID)
	SET @ID = SCOPE_IDENTITY();
	RETURN @ID
END
END
