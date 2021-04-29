CREATE PROCEDURE [dbo].[NETWORK_WEIGHT_UPSERT_MANY]
	@DATA_TABLE [dbo].NETWORK_WEIGHT_TYPE READONLY
AS
BEGIN
	MERGE [dbo].NETWORK_WEIGHT as dbTable
	USING @DATA_TABLE as tbl
	ON (dbTable.Id = tbl.Id)

	WHEN MATCHED THEN
		UPDATE SET  
			dbTable.VALUE = tbl.VALUE,
			dbTable.LAYER_ID = tbl.LAYER_ID,
			dbTable.INTERNAL_INDEX = tbl.INTERNAL_INDEX


	WHEN NOT MATCHED THEN
		INSERT ([VALUE], [LAYER_ID], [INTERNAL_INDEX])
		VALUES(tbl.VALUE, tbl.LAYER_ID, tbl.INTERNAL_INDEX);

;
END
RETURN 0
