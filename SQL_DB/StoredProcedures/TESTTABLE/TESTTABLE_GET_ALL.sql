﻿CREATE PROCEDURE [dbo].[TESTTABLE_GET_ALL]
	@ACTION int 
AS
	SELECT * FROM [dbo].TestTable
RETURN 0
