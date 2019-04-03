using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Base
{
    /// <summary>
    /// Представляет глобальный справочник типов всего приложения.
    /// </summary>
    /// <remarks>
    /// Используется для идентификации типа объекта Системы.
    /// </remarks>
    [JsonObject]
    [Serializable]
    [DataContract]
    public class BaseObjectType  : IBaseObject
    {
        public BaseObjectType()
        {

        }
        /// <summary>
        /// Инициализирует новый экземпляр класса BaseObjectType.
        /// </summary>
        /// <param name="type">Тип объекта Системы.</param>
        public BaseObjectType(Type type)
        {           
            TypeName = type.FullName.ToLower();
            AssemblyName = type.Assembly?.GetName().Name.ToLower();             
        }      

        /// <summary>
        /// Получает или задает идентификатор.
        /// </summary>
        [Key]
        [DataMember]        
        public int ID { get; set; }

        /// <summary>
        /// Получает или задает полное наименование типа.
        /// </summary>
        [DataMember]        
        [MaxLength(1000)]
        public string TypeName { get; set; }

        /// <summary>
        /// Получает или задает наименование сборки.
        /// </summary>
        [DataMember]       
        [MaxLength(1000)]
        public string AssemblyName { get; set; }


        /// <summary>
        /// Получает или задает УИД задписи.
        /// </summary>
        [DataMember]       
        public Guid Oid { get; set; }

      
        /// <summary>
        /// Получает тип.
        /// </summary>
        public virtual Type ObjectType
        {
            get { return GetObjectType(); } 
        }
        
        /// <summary>
        /// Возвращает тип.
        /// </summary>
        /// <returns>Тип.</returns>
        protected virtual Type GetObjectType()
        {
            try
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                Type tt = assem.GetTypes().Where(n => n.FullName.ToLower() == this.TypeName.ToLower()).FirstOrDefault();
                return tt;
            }
            catch { }
            return null;
        }

        #region IBaseObject
        [DataMember]
        public bool Hidden { get; set; }

        [DataMember]
        public double SortOrder { get; set; }

        public byte[] RowVersion { get; set; }
        #endregion

    }
}
