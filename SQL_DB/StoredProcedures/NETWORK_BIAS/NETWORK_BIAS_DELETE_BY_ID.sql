CREATE PROCEDURE [dbo].[NETWORK_BIAS_DELETE_BY_ID]
	@ID INT
AS
	DELETE FROM NETWORK_BIAS WHERE ID = @ID