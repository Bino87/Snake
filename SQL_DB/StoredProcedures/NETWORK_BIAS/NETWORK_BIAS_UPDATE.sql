CREATE PROCEDURE [dbo].[NETWORK_BIAS_UPDATE]
	@ID INT OUTPUT,
	@INTERNAL_INDEX INT,
	@VALUE FLOAT,
	@LAYER_ID INT
AS
	UPDATE NETWORK_BIAS SET 
	INTERNAL_INDEX = @INTERNAL_INDEX, VALUE = @VALUE, LAYER_ID = @LAYER_ID
	WHERE ID=@ID
	SET @ID = SCOPE_IDENTITY();
	RETURN @ID
