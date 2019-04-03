ALTER PROC [dbo].[pReport_ChecklistsOfRealEstate]
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
--основные хар-ки кадастрового объекта
,fake_CadastralNumber	=fake.CadastralNumber
,fake_NameByRight		=fake.NameByRight
,fake_RightKindCode		=fake.RightKindCode
,fake_ActualValue		=fake.ActualValue
--основные хар-ки права
,law_ObjectName			=law.ObjectName
,law_CadastralNumber	=law.CadastralNumber
,law_RegNumber			=law.RegNumber
from [CorpProp.Accounting].[AccountingObject] obu -- ОБУ
left join [CorpProp.Estate].[Estate] est on obu.EstateID = est.ID -- ОИ
left join [CorpProp.Estate].[InventoryObject] inv on est.ID = inv.ID -- материальные активы
left join [CorpProp.Estate].[Cadastral] cad on inv.ID = cad.ID -- кадастровые объекты
left join [CorpProp.Estate].[Cadastral] fake on inv.FakeID = cad.ID -- кадастровые объекты (штрих)
left join [CorpProp.Law].[Right] law on law.EstateID = est.ID OR law.EstateID = fake.ID-- права
where cad.ID is not null and inv.FakeID is not null