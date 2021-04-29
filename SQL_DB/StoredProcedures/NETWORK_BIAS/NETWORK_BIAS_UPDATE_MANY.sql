CREATE PROCEDURE [dbo].[NETWORK_BIAS_UPDATE_MANY]
	@DATA_TABLE [dbo].NETWORK_BIAS_TYPE READONLY
AS
BEGIN
	MERGE [dbo].NETWORK_BIAS as dbTable
	USING @DATA_TABLE as tbl
	ON (dbTable.Id = tbl.Id)

	WHEN MATCHED THEN
		UPDATE SET  
			dbTable.VALUE = tbl.VALUE,
			dbTable.LAYER_ID = tbl.LAYER_ID,
			dbTable.INTERNAL_INDEX = tbl.INTERNAL_INDEX

;
END
RETURN 0
