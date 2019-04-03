using Base;
using Base.Attributes;
using Base.Entities.Complex;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Import
{
    /// <summary>
    /// Представляет историю импорта объектов Системы.
    /// </summary>
    /// <remarks>
    /// Это связь М:М между объектами и импортом.
    /// </remarks>
    public class ImportObject : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ImportObject.
        /// </summary>
        public ImportObject() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса ImportObject.
        /// </summary>
        public ImportObject(BaseObject bo, Guid historyOid, TypeImportObject ttype) : base()
        {
            Type = ttype;
            Entity = new LinkBaseObject(bo);
            ImportHistoryOid = historyOid;
        }

        /// <summary>
        /// Получает или задает Объект Системы.
        /// </summary>
        public LinkBaseObject Entity { get; set; }
                

        /// <summary>
        /// Получает или задает УИД истории импорта.
        /// </summary>
        public Guid ImportHistoryOid { get; set; }

        /// <summary>
        /// Тип операции.
        /// </summary>
        public TypeImportObject Type { get; set; }

    }


    [UiEnum]
    public enum TypeImportObject
    {
        
        [UiEnumValue("Создание объекта")]
        CreateObject = 1,
        [UiEnumValue("Редактирование объекта")]
        UpdateObject = 2,
        [UiEnumValue("Удаление объекта")]
        DeleteObject = 3
    }
}
