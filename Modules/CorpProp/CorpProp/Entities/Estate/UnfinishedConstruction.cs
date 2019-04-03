using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет незавершенное строительство.
    /// </summary>     
    [EnableFullTextSearch]
    public class UnfinishedConstruction : Cadastral
    {
        
        /// <summary>
        /// Инициализирует новый экземпляр класса UnfinishedConstruction.
        /// </summary>
        public UnfinishedConstruction() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса UnfinishedConstruction из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public UnfinishedConstruction(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }

        /// <summary>
        /// Получает или задает степень готовности объекта незавершенного строительства, %.
        /// </summary>       
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Preparedness { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Дата начала использования (НКС)", Visible = false)]
        public DateTime? StartDateUse { get; set; }


    }
}
