using System;
using System.Collections.Generic;

namespace Common.Data.Services.Abstract
{
    public interface IBaseImportService
    {
        void Import(string pathFile);
        IEnumerable<T> GetRowProjections<T>(string pathFile, string worksheetName) where T : new();
        void ExecuteProjection<T>(Action<T> action, string pathFile, string worksheetName) where T : new();
    }
}
