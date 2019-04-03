USE [CorpProp]
GO

/****** Object:  StoredProcedure [dbo].[pReport_ScheduleStateRegistration_0]    Script Date: 20.02.2018 14:36:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pReport_ScheduleStateRegistration_0]
	@year INT
AS
BEGIN
	SET NOCOUNT ON;

    ;WITH MissingMonths
 AS 
   (
    SELECT 1 AS number
		UNION ALL
		SELECT number + 1 AS number
		FROM MissingMonths
		WHERE MissingMonths.number <= 11
    ),
s
AS
  (
   select [Owner] = ssrr.[SocietyName]
        ,Cost = ssrr.[InitialCost]
        ,SSR_SocName = soc.[ShortName]
        ,SSR_Name = ssr.[Name]
        --,[Month] = [Month].value
        ,dateReg = MONTH(DATEADD(MONTH, DATEDIFF(MONTH, 0, ssrr.[DatePlannedRegistration]), 0))
        ,ssrr.[DatePlannedRegistration]
        ,SSR_Year = ssr.[Year] 

   FROM [CorpProp.Law].[ScheduleStateRegistrationRecord] ssrr
   INNER JOIN [CorpProp.Law].[ScheduleStateRegistration] ssr
        ON ssr.[ID] = ssrr.[ScheduleStateRegistrationID]

      INNER JOIN [CorpProp.Subject].[Society] soc
		ON soc.[ID] = ssr.[SocietyID]

  )
SELECT COALESCE(s2.[dateReg],number) AS [Month]
        , s2.dateReg
        , s2.Cost
        ,s2.SSR_Name
        ,s2.SSR_SocName
	    ,s2.Owner
        ,s2.SSR_Year
FROM   MissingMonths MM 
LEFT OUTER JOIN s  s2 ON MM.number = s2.[dateReg] and s2.Owner <> s2.SSR_SocName and s2.Owner = N'ПАО "НК "Роснефть"' AND SSR_Year = @year
END
GO

