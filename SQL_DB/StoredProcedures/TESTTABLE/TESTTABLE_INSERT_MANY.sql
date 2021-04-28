﻿CREATE PROCEDURE [dbo].[TESTTABLE_INSERT_MANY]
	@data [dbo].TestTableType READONLY
AS
	BEGIN
		MERGE [dbo].TESTTABLE as dbTest
		USING @data as tblData
		ON (dbTest.Id = tblData.Id)

		WHEN NOT MATCHED THEN
			INSERT ([Value])
			VALUES( tblData.Value);

	END
RETURN 0

