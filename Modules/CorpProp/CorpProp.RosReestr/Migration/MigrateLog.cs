using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    /// <summary>
    /// Журнал миграции.
    /// </summary>
    [EnableFullTextSearch]
    public class MigrateLog : BaseObject
    {
        private static readonly CompiledExpression<MigrateLog, string> _SibUserName =
          DefaultTranslationOf<MigrateLog>.Property(x => x.SibUserName)
            .Is(x => (x.MigrateHistory != null && x.MigrateHistory.SibUser != null)? x.MigrateHistory.SibUser.FullName : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateLog.
        /// </summary>
        public MigrateLog()
        {
            MigrateDate = DateTime.Now;
        }
        

        /// <summary>
        /// Получает или задает дату/вермя ошибки.
        /// </summary>
        [DetailView(Name = "Дата/Время", ReadOnly = true)]
        [ListView]
        [PropertyDataType(PropertyDataType.DateTime)]
        [FullTextSearchProperty]
        public DateTime? MigrateDate { get; set; }

        [ListView]
        [DetailView(Name = "Пользователь", ReadOnly = true)]
        public string SibUserName => _SibUserName.Evaluate(this);

        /// <summary>
        /// Получает или задает мнемонику объекта.
        /// </summary>            
        [PropertyDataType(PropertyDataType.Mnemonic)]
        [DetailView(Name = "Тип объекта", ReadOnly = true)]
        [ListView]
        [FullTextSearchProperty]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает текст ошибки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Описание объекта", ReadOnly = true)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса миграции.
        /// </summary>
        [SystemProperty]
        public int? MigrateStateID { get; set; }

        /// <summary>
        /// Получает или задает статус.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Статус", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public MigrateState MigrateState { get; set; }

        /// <summary>
        /// Получает или задает текст ошибки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Текст ошибки", ReadOnly = true)]
        public string ErrorText { get; set; }

        /// <summary>
        /// Получает или задает ИД истории миграции.
        /// </summary>
        [SystemProperty]
        public int? MigrateHistoryID { get; set; }

        /// <summary>
        /// ПОлучает или задает историю миграции.
        /// </summary>
        [DetailView(Name = "История миграции", ReadOnly = true, Visible =false)]
        [ListView(Visible = false)]
        [FullTextSearchProperty]
        public MigrateHistory MigrateHistory { get; set; }

    }
}
