using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using Base.DAL;

namespace CorpProp.Entities.Request
{
    /// <summary>
    /// Представляет ответ на запрос информации.
    /// </summary>
    [EnableFullTextSearch]
    public class Response : TypeObject, IResponse
    {
        // ReSharper disable once InconsistentNaming
        private static readonly CompiledExpression<Response, SibUser> _ResponsableSociety =
        DefaultTranslationOf<Response>.Property(x => x.Responsable).Is(x => x.Executor == null || x.Executor.Society == null ? null: x.Executor.Society.ResponsableForResponse);

        // ReSharper disable once InconsistentNaming
        private static readonly CompiledExpression<Response, string> _RequestDescription =
        DefaultTranslationOf<Response>.Property(x => x.Description).Is(x => x.Request == null ? string.Empty : x.Request.Description);

        /// <summary>
        /// Получает или задает название.
        /// </summary>
        [ListView]
        [DetailView(Name = "Название", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Описание запроса.
        /// </summary>
        [ListView]
        [DetailView(Name = "Описание запроса", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Description => _RequestDescription.Evaluate(this);

        /// <summary>
        /// Получает или задает текст.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Текст ответа", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        public string Text { get; set; }


        /// <summary>
        /// Получает или задает срок ответа на запрос.
        /// </summary>
        [ListView]
        [DetailView(Name = "Срок ответа", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public DateTime? Term { get; set; }

        /// <summary>
        /// Получает или задает дату ответа.
        /// </summary>
        [ListView]
        [DetailView(Name = "Дата ответа", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? Date { get; set; }


        public int? ExecutorID { get; set; }

        /// <summary>
        /// Получает или задает исполнителя ответа.
        /// </summary>
        /// <remarks>
        /// Указывается, если исполнитель не является адресатом соотв. запроса.
        /// </remarks>
        [ListView]
        [DetailView(Name = "Исполнитель ответа", TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = false)]
        public virtual SibUser Executor { get; set; }


        public int? ExecutorSocietyID { get; set; }

        /// <summary>
        /// Получает ОГ исполнителя ответа.
        /// </summary>
        /// <remarks>
        /// Не редактируемое поле "ОГ" - "ОГ" пользователя указанного в поле "Исполнитель ответа"
        /// </remarks>
        [ListView]
        [DetailView(Name = "ОГ", TabName = CaptionHelper.DefaultTabName)]
        public virtual Society ExecutorSociety { get; set; }



        /// <summary>
        /// Получает ОГ исполнителя ответа.
        /// </summary>
        /// <remarks>
        /// Не редактируемое поле "ОГ" - "ОГ" пользователя указанного в поле "Исполнитель ответа"
        /// </remarks>
        [ListView]
        [DetailView(Name = "Ответственный за ответы", TabName = CaptionHelper.DefaultTabName)]
        public virtual SibUser Responsable => _ResponsableSociety.Evaluate(this);

        /// <summary>
        /// Получает или задает соисполнителей ответа.
        /// </summary>
        /// <remarks>
        /// Данное поле представляет коллекцию - пользователей системы.
        /// </remarks>
        ////[ListView]
        //[DetailView(Name = "Соисполнители ответа", TabName = CaptionHelper.DefaultTabName)]
        //[InverseProperty("CoexecutorsSibUserResponses")]
        //public virtual ICollection<SibUser> CoexecutorsSibUser { get; set; }

        ///// <summary>
        ///// Получает или задает контакты исполнителя ответа.
        ///// </summary>
        ///// <remarks>
        ///// Указывается, если исполнитель не является адресатом соотв. запроса.
        ///// </remarks>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Контактная информация исполнителя ответа", TabName = CaptionHelper.DefaultTabName)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string ExecutorContact { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса ответа.
        /// </summary>
        public int? ResponseStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус ответа.
        /// </summary>
        [ListView]
        [DetailView(Name = "Статус ответа", TabName = CaptionHelper.DefaultTabName)]
        public virtual ResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Получает или задает ИД запроса.
        /// </summary>
        public int? RequestID { get; set; }


        /// <summary>
        /// Получает или задает запрос.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Запрос", TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        public virtual Request Request { get; set; }

        ///// <summary>
        ///// Получает или задает строки ответа.
        ///// </summary>
        //[DetailView(Name = "Строки ответа", TabName = "[1]Строки ответа", HideLabel = true)]
        //public virtual ICollection<ResponseRow> ResponseRows { get; set; }

      

        /// <summary>
        /// Инициализирует новый экземпляр класса Response.
        /// </summary>
        public Response()
        {
            //ResponseRows = new List<ResponseRow>();
           
        }

        /// <summary>
        /// Переопределяет сохранение записи.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="entry">Запись.</param>
        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            base.OnSaving(uow, entry);

            //Поле “Соисполнители ответа“ (Пользователь) карточки ответа  
            //заполняется на основании ОГ-адресата, указывается пользователи ОГ, 
            //у которого признак “Исполнитель по запросам“ = true.
            if (this.ExecutorSociety != null && ID == 0)
            {
                var executors = uow.GetRepository<SibUser>()
                .Filter(f => !f.Hidden && f.Society != null
                && f.Society.ID == this.ExecutorSociety.ID
                && f.ExecutorOnRequest
                && f.ID != this.ExecutorID).ToList();

                foreach (var item in executors)
                {
                    uow.GetRepository<ManyToMany.ResponseAndSibUser>()
                        .Create(new ManyToMany.ResponseAndSibUser()
                        {
                            ObjLeft = this,
                            ObjRigth = item
                        });
                }
            }           

        }
    }
}
