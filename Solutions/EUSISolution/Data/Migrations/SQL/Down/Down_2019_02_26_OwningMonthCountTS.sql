IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'OwningMonthCountTS')
DROP FUNCTION [dbo].[OwningMonthCountTS]

GO

CREATE FUNCTION [dbo].[OwningMonthCountTS]
(
	@calcYear INT,
	@startMonth INT,
	@endMonth INT,
	@OID uniqueidentifier
)
RETURNS INT
AS
BEGIN
	DECLARE
	@tDate datetime,
	@result INT,
        @monthCounter INT = 0,
		@year INT = @calcYear
	IF (@year = NULL)
		RETURN 0
		
	WHILE (@year <= @calcYear)
		BEGIN
			WHILE (@startMonth <= @endMonth)
				BEGIN
					
					SET @tDate = DATEFROMPARTS(@year, @startMonth, 15)

					SELECT @result=
					/*@startMonth
					,AO.VehicleRegDate
					,AO.VehicleDeRegDate
					,@tDate,
					*/
					CASE WHEN (CASE WHEN (AO.VehicleDeRegDate is null or (AO.VehicleDeRegDate is not null and ((AO.VehicleRegDate) < (AO.VehicleDeRegDate) and (AO.VehicleDeRegDate > @tDate)))) THEN 1 ELSE 0 END
					+
					CASE WHEN (AO.VehicleDeRegDate is null or (AO.VehicleDeRegDate is not null and ((AO.VehicleRegDate) > (AO.VehicleDeRegDate) and (AO.VehicleRegDate < @tDate)))) THEN 1 ELSE 0 END )
					>0 THEN 1 ELSE 0 END

					FROM [CorpProp.Accounting].AccountingObjecttbl AS AO
					WHERE AO.Oid=@OID AND (year(AO.ActualDate) = @calcYear AND month(AO.ActualDate) = @startMonth)

					BEGIN
					SET @monthCounter = @monthCounter + @result
					END
					SET @startMonth = @startMonth + 1	

					SET @tDate = NULL
				END
			SET @year = @year + 1
		END
	RETURN
	@monthCounter
END
