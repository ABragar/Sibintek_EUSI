namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            //Описать sql-скрипты обновления БД до версии 29.10.2018

            Sql(@"
                if exists (select * from dbo.sysobjects where Name = N'splitstring' and xtype = 'TF')
                BEGIN
                DROP FUNCTION dbo.splitstring
                END
                GO
                CREATE FUNCTION [dbo].[splitstring] ( @stringToSplit VARCHAR(MAX),@stringDelimeter VARCHAR(1) )
                RETURNS
                 @returnList TABLE ([value] [nvarchar] (500))
                AS
                BEGIN

                 DECLARE @name NVARCHAR(255)
                 DECLARE @pos INT

                 WHILE CHARINDEX(@stringDelimeter, @stringToSplit) > 0
                 BEGIN
                  SELECT @pos  = CHARINDEX(@stringDelimeter, @stringToSplit)  
                  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

                  INSERT INTO @returnList 
                  SELECT @name

                  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
                 END

                 INSERT INTO @returnList
                 SELECT @stringToSplit

                 RETURN
                END
               ");

            Sql(@"  -- =============================================
                    -- Author:		<Antonov>
                    -- Create date: <13.04.2018>
                    -- Description:	<Тригер для обновления стоимости ОИ и ИК из ОС>
                    -- =============================================
                    ALTER TRIGGER [CorpProp.Accounting].[TR_AccountingCalculatedFild_CostEstate] 
                        ON [CorpProp.Accounting].[AccountingObjectTbl] AFTER INSERT, UPDATE
                    AS
                    BEGIN
	
	                    Declare @tb table (RNum int, Id int, EstateIDOld int, EstateID int)
	                    DECLARE @id int
	                    DECLARE @EstateIDOld int
	                    DECLARE @EstateID int
	
	                    insert into @tb
	                    (RNum, Id, EstateIDOld, EstateID)
	                    select ROW_NUMBER() OVER(ORDER BY ins.id) AS 'Row Number', ins.ID, del.EstateID, ins.EstateID from deleted as del
	                    left join inserted ins on del.ID=ins.ID
	
	                    DECLARE c_cur_AccountingObj_Update_InventoryObject_Cost         CURSOR STATIC READ_ONLY 
	                    FOR
	                        SELECT i.[ID], i.EstateIDOld, i.EstateID 
	                        FROM   @tb i
		                    order by RNum
	
	                    OPEN c_cur_AccountingObj_Update_InventoryObject_Cost;
	                    FETCH NEXT FROM c_cur_AccountingObj_Update_InventoryObject_Cost INTO 
		                    @id, @EstateIDOld, @EstateID
	
	                    WHILE @@FETCH_STATUS = 0
	                    BEGIN
		                    if(@EstateID is null)
		                    BEGIN
			                    declare @strOldValue nvarchar(500)
			                    set @strOldValue=cast(@EstateIDOld as nvarchar(500))
	
			                    update calc
			                    set 		
			                    calc.OwnerID = aobj.OwnerID
			                    ,calc.WhoUseID = aobj.WhoUseID
			                    ,calc.DealProps = aobj.DealProps
			                    ,calc.MainOwnerID = aobj.MainOwnerID
			                    ,calc.InitialCostSumOBU = isnull(aobj.InitialCost,0.00)
			                    ,calc.ResidualCostSumOBU=isnull(aobj.ResidualCost,0.00)
			                    ,calc.InitialCostSumNU = isnull(aobj.InitialCostNU,0.00)
			                    ,calc.ResidualCostSumNU = isnull(aobj.ResidualCostNU,0.00)
			                    from [CorpProp.Estate].[EstateCalculatedField] as calc
			                    left join [CorpProp.Estate].InventoryObject as inv on calc.EstateID=inv.ID
			                    left join (select
			                    aobjV.ID
			                    ,aobjV.EstateID
			                    ,aobjV.OwnerID
			                    ,aobjV.WhoUseID
			                    ,aobjV.DealProps
			                    ,aobjV.MainOwnerID
			                    ,isnull(aobjV.InitialCost,0.00) as InitialCost
			                    ,isnull(aobjV.ResidualCost,0.00) as ResidualCost
			                    ,isnull(aobjV.InitialCostNU,0.00) as InitialCostNU
			                    ,isnull(aobjV.ResidualCostNU,0.00) as ResidualCostNU
			                     from [CorpProp.Accounting].[AccountingObjectTbl] as aobjV where aobjV.ID<>@id and (aobjV.AccountNumber like '01%' or aobjV.AccountNumber like '1%') and aobjV.IsHistory = 0 and aobjV.Hidden = 0) as aobj on inv.ID=aobj.EstateID
			                    where  inv.ID=@EstateIDOld and calc.ID is not null and isnull(inv.IsPropertyComplex,0)=0
		                    END
		                    ELSE
		                    BEGIN
			                    update calc
			                    set 		
			                    calc.OwnerID = aobj.OwnerID
			                    ,calc.WhoUseID = aobj.WhoUseID
			                    ,calc.DealProps = aobj.DealProps
			                    ,calc.MainOwnerID = aobj.MainOwnerID
			                    ,calc.InitialCostSumOBU = isnull(aobj.InitialCost,0.00)
			                    ,calc.ResidualCostSumOBU=isnull(aobj.ResidualCost,0.00)
			                    ,calc.InitialCostSumNU = isnull(aobj.InitialCostNU,0.00)
			                    ,calc.ResidualCostSumNU = isnull(aobj.ResidualCostNU,0.00)
			                    from [CorpProp.Estate].[EstateCalculatedField] as calc
			                    left join [CorpProp.Estate].InventoryObject as inv on calc.EstateID=inv.ID
			                    left join (select
			                    aobjV.ID
			                    ,aobjV.EstateID
			                    ,aobjV.OwnerID
			                    ,aobjV.WhoUseID
			                    ,aobjV.DealProps
			                    ,aobjV.MainOwnerID
			                    ,isnull(aobjV.InitialCost,0.00) as InitialCost
			                    ,isnull(aobjV.ResidualCost,0.00) as ResidualCost
			                    ,isnull(aobjV.InitialCostNU,0.00) as InitialCostNU
			                    ,isnull(aobjV.ResidualCostNU,0.00) as ResidualCostNU
			                     from [CorpProp.Accounting].[AccountingObjectTbl] as aobjV where (aobjV.AccountNumber like '01%' or aobjV.AccountNumber like '1%') and aobjV.IsHistory = 0 and aobjV.Hidden = 0) as aobj on inv.ID=aobj.EstateID
			                    where  inv.ID=@EstateID and calc.ID is not null and isnull(inv.IsPropertyComplex,0)=0
		                    END
	                    FETCH NEXT FROM c_cur_AccountingObj_Update_InventoryObject_Cost INTO 
	                        @id, @EstateIDOld, @EstateID
	                    END;
	                    CLOSE c_cur_AccountingObj_Update_InventoryObject_Cost;
	                    DEALLOCATE c_cur_AccountingObj_Update_InventoryObject_Cost;
                    END
                    GO
                    ");

            Sql(@"
                        -- =============================================
                        -- Author:		<Antonov>
                        -- Create date: 13.04.2018
                        -- Description: Подсчет количества потомков для ИК
                        -- =============================================
                        ALTER TRIGGER [CorpProp.Estate].[TR_EstateCalculatedField_Count] 
                            ON [CorpProp.Estate].[InventoryObject] AFTER INSERT, UPDATE
                        AS
                        BEGIN
                            SET NOCOUNT ON;
	
	                        Declare @tb table (RNum int, Id int, ParentIdOld int, ParentId int)
	                        DECLARE @id int
	                        DECLARE @ParentIdOld int
	                        DECLARE @ParentID int
	                        DECLARE @tParentID int
	
	                        insert into @tb
	                        (RNum, Id, ParentIdOld, ParentId)
	                        select ROW_NUMBER() OVER(ORDER BY ins.id) AS 'Row Number', ins.ID, del.ParentID, ins.ParentID from deleted as del
	                        left join inserted ins on del.ID=ins.ID
	
	                        DECLARE c_cur_CalculatedField_Count_Update_InventoryObject         CURSOR STATIC READ_ONLY 
	                        FOR
	                            SELECT i.[ID], i.ParentIdOld, i.ParentID 
	                            FROM   @tb i
		                        order by RNum
	
	                        OPEN c_cur_CalculatedField_Count_Update_InventoryObject;
	                        FETCH NEXT FROM c_cur_CalculatedField_Count_Update_InventoryObject INTO 
		                        @id, @ParentIdOld, @ParentID
	
	                        WHILE @@FETCH_STATUS = 0
	                        BEGIN
		                        if(@ParentID is null)
		                        BEGIN
			                        update calc
			                        set 
				                        calc.[ChildObjectsCount] = isnull((SELECT COUNT(ins2.id) FROM [CorpProp.Estate].InventoryObject ins2  with (nolock) left join [CorpProp.Estate].Estate as est2 on ins2.ID=est2.ID  WHERE est2.[IsHistory] = 0 and est2.Hidden=0 and ins2.ParentID=@ParentIdOld and isnull(ins2.IsPropertyComplex,0)=0),0)
			                        from [CorpProp.Estate].[EstateCalculatedField] as calc
				                        left join [CorpProp.Estate].Estate as estP  with (nolock) on calc.EstateID=estP.ID
			                        where estP.ID = @ParentIdOld and estP.Hidden=0
		                        END
		                        ELSE
		                        BEGIN
			                        update calc
			                        set 
				                        calc.[ChildObjectsCount] = isnull((SELECT COUNT(ins2.id) FROM [CorpProp.Estate].InventoryObject ins2 left join [CorpProp.Estate].Estate as est2  with (nolock) on ins2.ID=est2.ID  WHERE est2.[IsHistory] = 0 and est2.Hidden=0 and ins2.ParentID=@ParentID and isnull(ins2.IsPropertyComplex,0)=0),0)
			                        from [CorpProp.Estate].[EstateCalculatedField] as calc
				                        left join [CorpProp.Estate].Estate as estP  with (nolock) on calc.EstateID=estP.ID
			                        where estP.ID = @ParentID and estP.Hidden=0
		                        END
	                        FETCH NEXT FROM c_cur_CalculatedField_Count_Update_InventoryObject INTO 
	                            @id, @ParentIdOld, @ParentID
	                        END;
	                        CLOSE c_cur_CalculatedField_Count_Update_InventoryObject;
	                        DEALLOCATE c_cur_CalculatedField_Count_Update_InventoryObject;    
                        END
                        GO

                        -- =============================================
                        -- Author:		<Antonov>
                        -- Create date: <13.04.2018>
                        -- Description:	<Тригер для обновления стоимости ОИ и ИК>
                        -- =============================================
                        ALTER TRIGGER [CorpProp.Estate].[TR_InventoryObject_UpdateCost] 
                           ON [CorpProp.Estate].[InventoryObject] AFTER insert, update
                        AS
                        BEGIN
	                        SET NOCOUNT ON;
	
	                        Declare @tb table (RNum int, Id int, ParentIdOld int, ParentId int)
	                        DECLARE @id int
	                        DECLARE @ParentIdOld int
	                        DECLARE @ParentID int
	                        DECLARE @tParentID int
	
	                        insert into @tb
	                        (RNum, Id, ParentIdOld, ParentId)
	                        select ROW_NUMBER() OVER(ORDER BY ins.id) AS 'Row Number', ins.ID, del.ParentID, ins.ParentID from deleted as del
	                        left join inserted ins on del.ID=ins.ID
	
	                        DECLARE c_cur_CalculatedField_Cost_Update_InventoryObject         CURSOR STATIC READ_ONLY 
	                        FOR
	                            SELECT i.[ID], i.ParentIdOld, i.ParentID 
	                            FROM   @tb i
		                        order by RNum
	
	                        OPEN c_cur_CalculatedField_Cost_Update_InventoryObject;
	                        FETCH NEXT FROM c_cur_CalculatedField_Cost_Update_InventoryObject INTO 
		                        @id, @ParentIdOld, @ParentID
	
	                        WHILE @@FETCH_STATUS = 0
	                        BEGIN
		                        if(@ParentID is null)
		                        BEGIN
			                        declare @strOldValue nvarchar(500)
			                        set @strOldValue=cast(@ParentIdOld as nvarchar(500))
			                        set @tParentID=@ParentIdOld
	
			                        update calcP
				                        set
					                        calcP.InitialCostSumOBU = isnull(inv.calcDownInitialCostSumOBU,0.00)
					                        ,calcP.ResidualCostSumOBU = isnull(inv.calcDownResidualCostSumOBU,0.00)
					                        ,calcP.InitialCostSumNU = isnull(inv.calcDownInitialCostSumNU,0.00)
					                        ,calcP.ResidualCostSumNU = isnull(inv.calcDownResidualCostSumNU,0.00)
					                        from [CorpProp.Estate].Estate as estP
					                        left join 
					                        (select invDown.ParentID as invDownParentID, sum(calcDown.InitialCostSumOBU) as calcDownInitialCostSumOBU, sum(calcDown.ResidualCostSumOBU) as calcDownResidualCostSumOBU, sum(calcDown.InitialCostSumNU) as calcDownInitialCostSumNU, sum(calcDown.ResidualCostSumNU) as calcDownResidualCostSumNU
						                        from [CorpProp.Estate].InventoryObject as invDown with (nolock)
						                        left join [CorpProp.Estate].Estate as estDown with (nolock) on invDown.ID=estDown.id
						                        left JOIN [CorpProp.Estate].[EstateCalculatedField] as calcDown with (nolock) on estDown.[CalculateID]=calcDown.[ID]
						                        where estDown.[IsHistory] = 0 and estDown.Hidden=0 and invDown.ParentID is not null and invDown.ID <> @id group by invDown.ParentID) as inv on estP.ID=inv.invDownParentID
					                        left JOIN [CorpProp.Estate].[EstateCalculatedField] as calcP on estP.[CalculateID]=calcP.[ID]
					                        where inv.invDownParentID is not null and estP.ID=@ParentIdOld
		                        END
		                        ELSE
		                        BEGIN
			                        update calcP
			                        set
				                        calcP.InitialCostSumOBU = isnull(inv.calcDownInitialCostSumOBU,0.00)
				                        ,calcP.ResidualCostSumOBU = isnull(inv.calcDownResidualCostSumOBU,0.00)
				                        ,calcP.InitialCostSumNU = isnull(inv.calcDownInitialCostSumNU,0.00)
				                        ,calcP.ResidualCostSumNU = isnull(inv.calcDownResidualCostSumNU,0.00)
				                        from [CorpProp.Estate].Estate as estP
				                        left join 
				                        (select invDown.ParentID as invDownParentID, sum(calcDown.InitialCostSumOBU) as calcDownInitialCostSumOBU, sum(calcDown.ResidualCostSumOBU) as calcDownResidualCostSumOBU, sum(calcDown.InitialCostSumNU) as calcDownInitialCostSumNU, sum(calcDown.ResidualCostSumNU) as calcDownResidualCostSumNU
					                        from [CorpProp.Estate].InventoryObject as invDown with (nolock)
					                        left join [CorpProp.Estate].Estate as estDown  with (nolock) on invDown.ID=estDown.id
					                        left JOIN [CorpProp.Estate].[EstateCalculatedField] as calcDown with (nolock) on estDown.[CalculateID]=calcDown.[ID]
					                        where estDown.[IsHistory] = 0 and estDown.Hidden=0 and invDown.ParentID is not null group by invDown.ParentID) as inv on estP.ID=inv.invDownParentID
				                        left JOIN [CorpProp.Estate].[EstateCalculatedField] as calcP on estP.[CalculateID]=calcP.[ID]
				                        where inv.invDownParentID is not null and estP.ID=@ParentID
		                        END
	                        FETCH NEXT FROM c_cur_CalculatedField_Cost_Update_InventoryObject INTO 
	                            @id, @ParentIdOld, @ParentID
	                        END;
	                        CLOSE c_cur_CalculatedField_Cost_Update_InventoryObject;
	                        DEALLOCATE c_cur_CalculatedField_Cost_Update_InventoryObject;
                        END
                        GO
                ");


        }
        
        public override void Down()
        {
            Sql(@"
                if exists (select * from dbo.sysobjects where Name = N'splitstring' and xtype = 'TF')
                BEGIN
                DROP FUNCTION dbo.splitstring
                END
            ");
            
        }
    }
}
