using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DAL
{
    public class FilterParams
    {
        public static int CurrentUserId => Ambient.AppContext.SecurityUser.ID;
        public static bool CurrentUserIsAdmin => Ambient.AppContext.SecurityUser.IsAdmin;

        public static DateTime Today => DateTime.Today;
    }
}

