IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'OwningMonthCountTS')
BEGIN 
DROP FUNCTION [dbo].[OwningMonthCountTS]

PRINT N'Dropping [dbo].[OwningMonthCountTS]...';
END

GO

PRINT N'Creating [dbo].[OwningMonthCountTS]...';

GO

CREATE FUNCTION [dbo].[OwningMonthCountTS]
( 
	@inServiceDate datetime,
	@leavingDate datetime,
	@leaveDateTS datetime,
	@calcYear INT,
	@startMonth INT,
	@endMonth INT
)
RETURNS
INT
AS
BEGIN
    DECLARE
        @monthCounter INT = 0,
		@year INT = @calcYear
	IF (@year = NULL)
		RETURN @monthCounter
	WHILE (@year <= @calcYear)
		BEGIN
			WHILE (@startMonth <= @endMonth)
				BEGIN
					IF
					((@inServiceDate IS NOT NULL AND @inServiceDate <= DATEFROMPARTS(@year, @startMonth, 15)
						AND
						(@leavingDate is NULL OR (@leavingDate > DATEFROMPARTS(@year, @startMonth, 15))))
					OR
					(@leaveDateTS IS NULL AND @inServiceDate IS NOT NULL AND (@inServiceDate <= DATEFROMPARTS(@year, @startMonth, 15)
						AND @inServiceDate > @leavingDate))
					OR
					(@leaveDateTS IS NULL AND @inServiceDate IS NOT NULL AND (@inServiceDate <= DATEFROMPARTS(@year, @startMonth, 15)
						AND @inServiceDate >= @leavingDate))
					)
					BEGIN
					SET @monthCounter = @monthCounter + 1
					END
					SET @startMonth = @startMonth + 1	
				END
			SET @year = @year + 1
		END
	RETURN
	@monthCounter
END