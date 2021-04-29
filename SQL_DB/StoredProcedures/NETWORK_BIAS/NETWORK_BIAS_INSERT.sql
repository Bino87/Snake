CREATE PROCEDURE [dbo].[NETWORK_BIAS_INSERT]
	@VALUE FLOAT ,
	@LAYER_ID INT ,
	@INTERNAL_INDEX INT ,
	@ID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NETWORK_BIAS VALUES(@VALUE, @LAYER_ID, @INTERNAL_INDEX)
	SET @ID = SCOPE_IDENTITY();
	RETURN @ID
END