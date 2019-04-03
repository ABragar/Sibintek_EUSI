using Base;
using Base.DAL;
using CorpProp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Base
{
    public interface IHistoryService<out T> : IHistory 
    {        

        IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date);

        int? GetObjIDByDate(IUnitOfWork unitOfWork, int id, DateTime? date);

        DateTime? GetMinDate(IUnitOfWork unitOfWork, int id);

        
    }
}
