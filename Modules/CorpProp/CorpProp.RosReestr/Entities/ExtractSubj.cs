using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using System;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Выписка из ЕГРП(Н) о правах юридического лица на ОНИ.
    /// </summary>
    [EnableFullTextSearch]
    public class ExtractSubj : Extract
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractSubj.
        /// </summary>
        public ExtractSubj():base()
        {

        }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Статус обновления в АИС КС", Visible = false)]
        public StatusCorpProp UpdateCPStatus { get; set; }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Дата обновления в АИС КС", Visible = false)]
        public DateTime? UpdateCPDateTime { get; set; }

        /// <summary>
        /// Получает или задает ИД субъекта.
        /// </summary>
        public int? SubjectRecordID { get; set; }

        /// <summary>
        /// Получает или задает субъекта.
        /// </summary>
        public SubjectRecord SubjectRecord { get; set; }                       

        /// <summary>
        /// Получает или задает ИД уведомления об отсутствии сведений.
        /// </summary>
        public int? NoticeID { get; set; }

        /// <summary>
        /// Получает или задает уведомление об отсутствии сведений об обобщенных правах субъекта.
        /// </summary>
        public Notice Notice { get; set;}

        /// <summary>
        /// Получает или задает ИД отказа.
        /// </summary>
        public int? RefusalID { get; set; }

        /// <summary>
        /// Получает или задает отказ в выдаче сведений.
        /// </summary>
        public Refusal Refusal { get; set; }



        

    }
}
