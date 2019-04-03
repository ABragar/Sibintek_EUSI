CREATE PROC [dbo].[pReport_ChecklistsOfAO_E_SD]
  AS
  -- =============================================
-- Author:		Sharov Alexey
-- Create date: 24.01.2018
-- Description:	Retrive data to report Checklists Of Real Estate
-- 
-- Parametrs:
-- 
-- History:
--    Author		   Date		Description
--    SharovAV		24.01.2018	Create the procedure
-- =============================================


-- rs.1
select 
--основные хар-ки ОБУ
 Obu_InventoryNumber	=obu.InventoryNumber
,Obu_Name				=obu.Name
,Obu_InitialCost		=obu.InitialCost
,Obu_ResidualCost		=obu.ResidualCost
--основные хар-ки ОИ
,Est_InventoryNumber	=Estate.InventoryNumber
,Est_Name				=Estate.Name
,Est_InitialCost		=Estate.InitialCost
,Est_ResidualCost		=Estate.ResidualCost
-- Сделка
,SibDeal_Date = SibDeal.DateStateRegistration
,SibDeal_ = SibDeal.ParentContractNumber

from [CorpProp.Accounting].[AccountingObject] obu -- ОБУ
INNER JOIN [CorpProp.Estate].EstateDeal EstateDeal ON obu.ID = EstateDeal.ID 
INNER JOIN [CorpProp.Estate].Estate Estate ON obu.EstateID = Estate.ID AND EstateDeal.EstateID = Estate.ID 
INNER JOIN [CorpProp.DocumentFlow].SibDeal SibDeal ON EstateDeal.SibDealID = SibDeal.ID