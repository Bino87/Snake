﻿CREATE PROCEDURE [dbo].[TESTTABLE_INSERT]
	@ACTION int  = 0 ,
	@ID int OUTPUT
AS

SET NOCOUNT ON;

INSERT INTO [dbo].TestTable	VALUES(@ID)

DECLARE @NEW_IDENTITY int;

SELECT @NEW_IDENTITY = SCOPE_IDENTITY()

RETURN 1

