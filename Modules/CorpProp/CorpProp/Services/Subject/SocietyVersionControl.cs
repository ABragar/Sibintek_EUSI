using Base.DAL;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Subject
{
    /// <summary>
    /// Представляет управление историей и версионностью ОГ.
    /// </summary>
    public class SocietyVersionControl : BaseVersionControl<Society>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SocietyVersionControl.
        /// </summary>
        public SocietyVersionControl(
            IUnitOfWork _uow
            , DataTable _table
            , Dictionary<string, string> _colsNameMapping
            , DateTime _period
            , ref ImportHistory history
            ) : base(_uow, _table, _colsNameMapping, _period, ref history)
        {

        }
    }
}
