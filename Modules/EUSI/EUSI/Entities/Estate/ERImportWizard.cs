using Base;
using Base.Attributes;
using CorpProp.Entities.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Estate
{
    /// <summary>
    /// Мастер импорта заявки на регистрацию ОИ.
    /// </summary>
    [NotMapped]
    public class ERImportWizard : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ERImportWizard.
        /// </summary>
        public ERImportWizard() : base()
        {
            
        }
        
        /// <summary>
        /// Получает или задает файл заявки.
        /// </summary>
        [DetailView("Файл импорта", Required = true), ListView()]
        public virtual FileCard FileCard { get; set; }

        /// <summary>
        /// Получает или задает ИД файла заявки.
        /// </summary>        
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает номер обращения ЦДС.
        /// </summary>
        [DetailView("Номер обращения ЦДС", Required = true), ListView()]           
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberCDS { get; set; }

        /// <summary>
        /// Получает или задает дату и время обращения в ЦДС.
        /// </summary>
        [DetailView("Дата и Время обращения в ЦДС", Required = true), ListView()]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? DateCDS { get; set; }

        /// <summary>
        /// Получает или задает признак быстрого закрытия.
        /// </summary>
        [DetailView("Заявка обрабатывается со статусом «Быстрое закрытие»"), ListView()] 
        public bool QuickClose { get; set; }

        
        /// <summary>
        /// Получает или задает признак выполнения визуальных контролей.
        /// </summary>
        [DetailView("Визуальные контроли пройдены"), ListView()]
        public bool VisualCheck { get; set; }


        /// <summary>
        /// Получает или задает комментарий.
        /// </summary>
        [DetailView("Информация о прохождении визуальных контролей Сервиса ЕУСИ"), ListView()]
        [PropertyDataType(PropertyDataType.MultilineText)]
        public string Comment { get; set; }

    }
}
