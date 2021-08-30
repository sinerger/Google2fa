CREATE PROCEDURE [dbo].[SetExistTFAByLeadID] @LeadID UNIQUEIDENTIFIER
AS
UPDATE [dbo].[LeadsTFA]
SET IsExist = 1
WHERE [dbo].[LeadsTFA].LeadID = @LeadID

