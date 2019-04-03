using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Security.Service
{
    public interface IAccessErrorDescriber
    {
        string AccessDenied();
        string UserNotFound(int user_id);
        string ProfileTypeIsEmpty(int category_id);
    }
}
