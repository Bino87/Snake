CREATE PROCEDURE [dbo].[UPDATE_MANY]
	@data [dbo].TestTableType READONLY
AS
	BEGIN
		MERGE [dbo].TESTTABLE as dbTest
		USING @data as tblData
		ON (dbTest.Id = tblData.Id)

		WHEN MATCHED THEN
			UPDATE SET 
						dbTest.Value = tblData.Value
						;

	END
RETURN 0
