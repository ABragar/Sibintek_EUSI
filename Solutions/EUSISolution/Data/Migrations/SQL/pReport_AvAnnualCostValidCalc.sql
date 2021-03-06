if exists (select * from dbo.sysobjects where Name = N'pReport_AvAnnualCostValidCalc' and xtype = 'P')
drop procedure pReport_AvAnnualCostValidCalc

go

CREATE PROCEDURE [dbo].[pReport_AvAnnualCostValidCalc]
(
@DateYear nvarchar (20),
@BE nvarchar(500),
@vintConsolidationId int = NULL,
@taxPeriod NVARCHAR(20),
@currentUserId				INT = NULL
)
as

--declare 
--@BE nvarchar(500),
--@DateYear nvarchar (20)
--@DateF datetime,
--@DateO datetime
--set @BE =N'Публичное акционерное общество "Нефтяная компания "Роснефть"'
--set @DateYear =N'2015'
--set @DateF ='01.01.2014'
--set @DateO ='01.01.2019'
--select charindex ('01',convert(nvarchar(10),@DateF,104 ))
--select CONVERT (datetime,'22.11.'+ @DateYear,104)

--default values
 DECLARE 
	@eventCode NVARCHAR(30) = N'Report_AvAnnualCostValidCalc',
	@isValid BIT = 1,
	@comment NVARCHAR(MAX) = N'',
	@resultText NVARCHAR(40) = N'Расхождения не выявлены',
	@startdate  DATETIME ,
	@enddate	DATETIME

	set @startDate = CONVERT (date,'01.01.'+@DateYear,104)

	SET @enddate = CASE @taxPeriod 
		WHEN N'1 квартал' THEN  CONVERT (date,'31.03.'+@DateYear,104)
		WHEN N'полугодие' THEN  CONVERT (date,'30.06.'+@DateYear,104)
		WHEN N'девять месяцев' THEN  CONVERT (date,'30.09.'+@DateYear,104)
		WHEN N'Год' THEN  CONVERT (date,'31.12.'+@DateYear,104)
		END

BEGIN TRY
select 
-------------------------------------------подсчет 1 строки по всем месяцам 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P1 end),0) as S_1_31_12,

-----------------------------------------подсчет 2 строки по всем месяцам
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_01, 
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_02,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_03,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_04,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_05,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_06,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_07,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_08,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_09,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_10,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_11,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_01_12,
ISNULL(SUM(case when (Sel1.P1 is not NULL) and (Sel1.P2 is not NULL) and (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P2 end),0) as S_2_31_12,

-----------------------------------------подсчет 4 строки по всем месяцам
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P4 end),0) as S_4_31_12,

------------------------------------------подсчет 7 строки по всем месяцам
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P7 end),0) as S_7_31_12,

-------------------------------------------подсчет 8 строки по всем месяцам

ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P8 end),0) as S_8_31_12,

-----------------------------------------------подсчет 9 строки по всем месяцам

ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P9 end),0) as S_9_31_12,

------------------------------------------подсчет 10 строки по всем месяцам

ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P10 end),0) as S_10_31_12,
 
-------------------------------------------подсчет 11 строки по всем месяцам

ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P11 end),0) as S_11_31_12,
 

-------------------------------------------подсчет 12 строки по всем месяцам

ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.01.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_01, 
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.02.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_02,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.03.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_03,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.04.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_04,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.05.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_05,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.06.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_06,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.07.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_07,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.08.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_08,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.09.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_09,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.10.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_10,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.11.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_11,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'01.12.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_01_12,
ISNULL(SUM(case when (Sel1.InSDate<=CONVERT (datetime,'31.12.'+@DateYear,104)) then Sel1.P12 end),0) as S_12_31_12
 


from
(SELECT    D_Cons.Name as DCons, OBU.InServiceDate as InSDate, D_OBU_EstMov.Name as DEstMov, D_OBU_StObj.Name as DStObj, D_OBU_EDT.Name as DEDT, OBU.AccountNumber,

case when charindex ('1',convert(nvarchar(10),OBU.AccountNumber,104 ))=LEN(convert(nvarchar(10),OBU.AccountNumber,104 )) and 
(D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое'))
and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.InitialCost end as P1,

case when charindex ('2',convert(nvarchar(10),OBU.AccountNumber,104 ))=LEN(convert(nvarchar(10),OBU.AccountNumber,104 )) and
(D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое'))
and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.DepreciationCostNU end as P2,

case when charindex ('8',convert(nvarchar(10),OBU.AccountNumber,104 ))=LEN(convert(nvarchar(10),OBU.AccountNumber,104 )) and (UnFinConstr.StartDateUse is not NULL) and 
(D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое'))
and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.InitialCost end as P4,

case when (D_Est_EDT.Name=N'Земельный участок') and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.InitialCost end as P7,

case when ((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа')) and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO) */and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.ResidualCostNU end as P8,

case when charindex ('8',convert(nvarchar(10),OBU.AccountNumber,104 ))=LEN(convert(nvarchar(10),OBU.AccountNumber,104 )) and (UnFinConstr.StartDateUse IS not NULL) and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO) */and 
((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.InitialCost end as P9,

case when (CadSt.CadastralValue is NOT NULL) and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.ResidualCostNU end as P10,

case when (D_TB.Name=N'Кадастровая стоимость') and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.ResidualCostEstimate end as P11,

case when (OBU.ShipRegDate is not NULL) and (D_Cons.Name=@BE) /*and (OBU.InServiceDate between @DateF and @DateO)*/ and ((D_OBU_EstMov.Name=N'Движимое имущество') or (D_OBU_EstMov.Name=N'Недвижимое')) and 
((D_OBU_EDT.Name=N'Земельный участок') or ((D_OBU_EDT.Name=N'Речные/морские суда') and (OBU.ShipRegDate is not NULL)) or (OBU.IsCultural is not NULL) or
((D_Inv_DepGr.Name=N'1 группа') or (D_Inv_DepGr.Name=N'2 группа'))) and (D_OBU_StObj.Name<>N'В аренде') and (D_OBU_EDT.Name<>N'Нематериальный актив') then OBU.ResidualCostNU end as P12

  
FROM          [CorpProp.Accounting].AccountingObjectTbl AS OBU LEFT OUTER JOIN
              [CorpProp.Base].DictObject AS D_OBU_EDT ON OBU.EstateDefinitionTypeID = D_OBU_EDT.ID LEFT OUTER JOIN
              [CorpProp.Base].DictObject AS D_OBU_StObj ON OBU.StateObjectRSBUID = D_OBU_StObj.ID LEFT OUTER JOIN
              [CorpProp.Base].DictObject AS D_OBU_EstMov ON OBU.EstateMovableNSIID = D_OBU_EstMov.ID LEFT OUTER JOIN
              [CorpProp.NSI].Consolidation AS Cons ON OBU.ConsolidationID = Cons.ID LEFT OUTER JOIN
              [CorpProp.Base].DictObject AS D_Cons ON Cons.ID = D_Cons.ID left join
			  [CorpProp.Estate].Estate AS Est ON OBU.EstateID=Est.ID left join
			  [CorpProp.Estate].InventoryObject  AS Inv ON Est.ID=Inv.ID left join
			  [CorpProp.Estate].Cadastral AS Cadst ON Inv.FakeID=Cadst.ID left join
			  [CorpProp.Estate].UnfinishedConstruction as UnFinConstr ON Cadst.ID=UnFinConstr.ID left join
			  [CorpProp.Base].DictObject AS D_Est_EDT ON Est.EstateDefinitionTypeID = D_Est_EDT.ID left join
			  [CorpProp.Base].DictObject as D_Inv_DepGr ON Inv.DepreciationGroupID=D_Inv_DepGr.ID left join 
			  [CorpProp.Base].DictObject as D_TB ON OBU.TaxBaseID=D_TB.ID
) AS Sel1
END TRY
BEGIN CATCH
	SET @comment = ERROR_MESSAGE();
	SET @resultText = N'Ошибка при построении отчета';
	SET @isValid = 0;
END CATCH

	EXEC [dbo].[pCreateReportMonitoring]  
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationId,
		@isvalid = @isValid,
		@resulttext = @resultText,
		@comment = @comment,
		@startdate  = @startDate,
		@enddate  = @enddate

