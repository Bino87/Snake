﻿CREATE TABLE [dbo].[NETWORK_BIAS]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[INTERNAL_INDEX] int NOT NULL,
	[LAYER_ID] int NOT NULL,
	[VALUE] int NOT NULL
)
