CREATE PROCEDURE [dbo].[NETWORK_BIAS_INSERT_MANY]
	@DATA_TABLE [dbo].NETWORK_BIAS_TYPE READONLY
AS
BEGIN
	MERGE [dbo].NETWORK_BIAS as dbTable
	USING @DATA_TABLE as tbl
	ON (dbTable.Id = tbl.Id)

	WHEN NOT MATCHED THEN
		INSERT ([VALUE], [LAYER_ID], [INTERNAL_INDEX])
		VALUES(tbl.VALUE, tbl.LAYER_ID, tbl.INTERNAL_INDEX);

;
END
RETURN 0