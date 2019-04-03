using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service.Log;
using LinqToExcel;

namespace Common.Data.Services.Abstract
{
    public abstract class BaseImportService : IBaseImportService
    {
        protected readonly ILogService _logService;
        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;

        protected BaseImportService(IUnitOfWorkFactory unitOfWorkFactory, ILogService logService)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _logService = logService;
        }

        public abstract void Import(string pathFile);

        public IEnumerable<T> GetRowProjections<T>(string pathFile, string worksheetName = null) where T : new()
        {
            if (string.IsNullOrEmpty(worksheetName))
                return ExcelQueryFactory.Worksheet<T>(0, pathFile, null).Select(x => x);
            else
                return ExcelQueryFactory.Worksheet<T>(worksheetName, pathFile, null).Select(x => x);
        }

        public void ExecuteProjection<T>(Action<T> action, string pathFile, string worksheetName = null) where T : new()
        {
            List<T> projections = GetRowProjections<T>(pathFile, worksheetName).ToList();
            foreach (T projection in projections)
            {
                try
                {
                    action?.Invoke(projection);
                }
                catch (Exception e)
                {
                    int numberRow = projections.IndexOf(projection) + 2;
                    _logService.Log($"Лист '{worksheetName}', ошибка при обработке строки {numberRow}: {e.Message}");  
                }
            }
        }
    }
}
