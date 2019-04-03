CREATE PROC [dbo].[pReport_ComparisonOfBasicCharacteristics]
  AS
  -- =============================================
-- Author:		Sharov Alexey
-- Create date: 18.01.2018
-- Description:	Retrive data to report Comparison Of Basic Characteristics
-- 
-- Parametrs:
-- 
-- History:
--    Author		   Date		Description
--    SharovAV		19.01.2018	Create the procedure
-- =============================================


-- rs.1
select 
--основные хар-ки ОБУ
 Obu_InventoryNumber	=obu.InventoryNumber
,Obu_Name				=obu.Name
,Obu_InitialCost		=obu.InitialCost
,Obu_ResidualCost		=obu.ResidualCost
--основные хар-ки ОИ
,Est_InventoryNumber	=est.InventoryNumber
,Est_Name				=est.Name
,Est_InitialCost		=est.InitialCost
,Est_ResidualCost		=est.ResidualCost
--основные хар-ки кадастрового объекта

--основные хар-ки права
,law.ObjectName
,law.CadastralNumber
,law.RegNumber
from [CorpProp.Accounting].[AccountingObject] obu -- ОБУ
left join [CorpProp.Estate].[Estate] est on obu.EstateID = est.ID -- ОИ
left join [CorpProp.Estate].[InventoryObject] inv on est.ID = inv.ID -- материальные активы
left join [CorpProp.Estate].[Cadastral] cad on inv.ID = cad.ID -- кадастровые объекты
left join [CorpProp.Estate].[Cadastral] fake on inv.FakeID = cad.ID -- кадастровые объекты (штрих)
left join [CorpProp.Law].[Right] law on law.EstateID = est.ID OR law.EstateID = fake.ID-- права