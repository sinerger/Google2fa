﻿CREATE TABLE [dbo].[LeadsTFA]
(
	[LeadID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [TFAKey] NVARCHAR(500) NOT NULL,
	[IsExist] bit NOT NULL default 0
)
