CREATE PROCEDURE [dbo].[TESTTABLE_UPSERT_MANY]
	@data [dbo].TestTableType READONLY
AS
	BEGIN
		MERGE [dbo].TESTTABLE as dbTest
		USING @data as tblData
		ON (dbTest.Id = tblData.Id)

		WHEN MATCHED THEN
			UPDATE SET 
						dbTest.Id = tblData.Id,
						dbTest.Value = tblData.Value
		

		WHEN NOT MATCHED THEN
			INSERT ([Id],[Value])
			VALUES(tblData.Id, tblData.Value);

	END
RETURN 0