using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Common;

namespace EUSI.Entities.Models
{
    /// <summary>
    /// Модель для установки комментария
    /// </summary>
    public class SetCommentModel : BaseObject, IDialogObject
    {
        /// <summary>
        /// Айдишки сущностей для которых необходимо сохранить комментарий. 
        /// </summary>
        public string EntityIds { get; set; }

        /// <summary>
        /// Коментарий
        /// </summary>
        [DetailView("Комментарий", Required = false)]
        [FullTextSearchProperty]
        public string Comment { get; set; }
    }
}
