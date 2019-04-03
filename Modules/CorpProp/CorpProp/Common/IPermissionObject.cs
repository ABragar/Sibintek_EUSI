using CorpProp.Entities.Document;

namespace CorpProp.Common
{
    public interface ISibAccessableObject
    {
        /// <summary>
        /// Получает или задает ИД профиля пользователя, создавшего документ.
        /// </summary>
        int? CreateUserID { get; set; }
        /// <summary>
        /// Получает или задает ИД общества группы.
        /// </summary>
        int? SocietyID { get; set; }
        /// <summary>
        /// Получает или задает права доступа к экземпляру.
        /// </summary>
        FileCardPermission FileCardPermission { get; set; }
        int? FileCardPermissionID { get; set; }
    }
}
