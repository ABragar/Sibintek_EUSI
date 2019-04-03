using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Security.Service
{
    public class AccessErrorDescriber: IAccessErrorDescriber
    {
        public string AccessDenied()
        {
            return $"Отказано в доступе";
        }

        public string UserNotFound(int user_id)
        {
            return $"Пользователь [{user_id}] не найден";
        }

        public string ProfileTypeIsEmpty(int category_id)
        {
            return $"Для категории пользователей [{category_id}] не задан тип Профиля";
        }
    }
}
