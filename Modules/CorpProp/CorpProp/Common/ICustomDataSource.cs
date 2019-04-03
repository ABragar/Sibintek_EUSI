using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    /// <summary>
    /// Предоставляет методы для работы с кастомными источниками данных для сервисов.
    /// </summary>
    public interface ICustomDataSource<out T> : ICustomDS 
    {

        IQueryable<T> GetAllCustom(IUnitOfWork uow, params object[] objs);
    }

    public interface ICustomDS
    {
    }
}
