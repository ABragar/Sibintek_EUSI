using Base.BusinessProcesses.Entities;
using System;
using Base.Service;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IProductionCalendarService: IService
    {
        void CreateCalendar(int year);
        DateTime GetEndDate(DateTime start, int period, PerfomancePeriodType periodType);
        DateTime GetEndDate(DateTime start, TimeSpan period, PerfomancePeriodType periodType);
        /// <summary>
        /// Период между двумя датами в минутах
        /// </summary>
        /// <param name="start">Начальная дата</param>
        /// <param name="end">Конечная дата</param>
        /// <param name="periodType">Тип расчета</param>
        /// <returns>Период в минутах</returns>
        int GetPeriod(DateTime start, DateTime end, PerfomancePeriodType periodType);

        
    }
}
