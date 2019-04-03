using System.Linq;
using Base.DAL;
using Base.UI.ViewModal;
using Base.UI.Extensions;
using WebUI.Controllers;

namespace WebUI.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable Filter(this IQueryable q, IBaseController controller, IUnitOfWork unit_of_work, ViewModelConfig config, string filter = null, params object[] args)
        {
            return q.ListViewFilter(config.ListView, filter, args);
        }
    }
}