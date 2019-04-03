using Base.Attributes;
using Base.Security;

namespace Base.Access.Entities
{
    public class UserAccess : BaseObject
    {
        public int ParentID { get; set; }
        public virtual AccessEntry Parent { get; set; }
        public int UserID { get; set; }
        [DetailView("Пользователь", Required = true)]
        [ListView]
        public virtual User User { get; set; }
        [DetailView("Чтение")]
        [ListView]
        public bool Read { get; set; }
        [DetailView("Изменение")]
        [ListView]
        public bool Update { get; set; }
        [DetailView("Удаление")]
        [ListView]
        public bool Delete { get; set; }
        [DetailView("Изменение доступа")]
        [ListView]
        public bool ChangeAccess { get; set; }
    }
}