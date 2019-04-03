using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Subject;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Helpers
{
    /// <summary>
    /// Предоставляет методы для импорта объектов xml-выписок.
    /// </summary>
    public static class ImportHelper
    {
      
        /// <summary>
        /// Возвращает дату.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns></returns>
        public static DateTime? GetDate(DateTime? date)
        {
            //TODO: фиксировать дату ранее 1793 года строкой в БД
            if (date < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                return null;

            return date;
        }

        /// <summary>
        /// Получает значение даты из строки формата dd.MM.yyyy
        /// </summary>
        /// <param name="dateStr">Дата строкой в формате dd.MM.yyyy</param>
        /// <returns></returns>
        public static DateTime? GetDate(string dateStr)
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(dateStr,
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out date);
           return GetDate(date);
        }


        public static Cadastral FindCadastral(IUnitOfWork uofw, string cadNumber)
        {
            return
                uofw.GetRepository<Cadastral>()
                .Filter(x => x.CadastralNumber.ToLower() == cadNumber.ToLower() && x.Hidden == false)
                .FirstOrDefault();
        }

        public static BuildingStructure FindBuilding(IUnitOfWork uofw, string cadNumber)
        {
            return
                uofw.GetRepository<BuildingStructure>()
                .Filter(x => x.CadastralNumber.ToLower() == cadNumber.ToLower() && x.Hidden == false)
                .FirstOrDefault();
        }

        public static Land FindLand(IUnitOfWork uofw, string cadNumber)
        {
            return
                uofw.GetRepository<Land>()
                .Filter(x => x.CadastralNumber.ToLower() == cadNumber.ToLower() && x.Hidden == false)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получает код Enum-а объекта схемы xml-выписки.
        /// </summary>
        /// <param name="obj">Объект типа Enum.</param>
        /// <returns>Код объекта.</returns>
        public static string GetCodeEnum(object obj) 
        {
            string val = "";
            if (obj == null) return val;
            val = obj.ToString().Replace("Item", "");
            return val;
        }

        /// <summary>
        /// Получает локализованное наименование значения объекта типа Enum.
        /// </summary>
        /// <param name="obj">Объект типа Enum.</param>
        /// <returns>Наименование объекта по коду.</returns>
        public static string GetNameEnum(object obj)
        {
            //TODO: 
            
            return "";
        }

        /// <summary>
        /// Вовзращает ИНН и наименование субъекта выписки.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>ИНН и наименование субъекта.</returns>
        public static string GetSubjectInfo(object obj)
        {
            //TODO
            return "";
        }

        /// <summary>
        /// Возвращает число из входной строки.
        /// </summary>
        /// <param name="obj">Строка.</param>
        /// <returns>Число.</returns>
        public static int GetInt(string obj)
        {
            int val = 0;
            if (!String.IsNullOrEmpty(obj))
                int.TryParse(obj, out val);
            return val;
        }

        /// <summary>
        /// Десериализация объекта типа T из потока.        
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Десериализованный объект.</returns>
        public static T DeserializeFromStream<T>(
             Stream stream
          
            ) where T : class
        {
            T obj = null;
            try
            {
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    System.Xml.Serialization.XmlSerializer reader =
                        new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = (T)reader.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                
            }
            return obj;
        }


        public static Society FindSociety(IUnitOfWork uow, string inn, string ogrn)
        {
            Society subj = null;

            if (!String.IsNullOrEmpty(inn))
            {
                subj = uow.GetRepository<Society>()
                    .Filter(f => !f.Hidden && f.INN == inn).FirstOrDefault();                 
            }
            if (!String.IsNullOrEmpty(ogrn) && subj == null)
            {
                subj = uow.GetRepository<Society>()
                    .Filter(f => !f.Hidden && f.OGRN == ogrn).FirstOrDefault();
            }

            if (subj == null)
                return FindSuccessor(uow, inn);

            return subj;
        }

        public static Society FindSuccessor(IUnitOfWork uow, string inn)
        {
            Predecessor subj = null;
            if (!String.IsNullOrEmpty(inn))
            {
                subj = uow.GetRepository<Predecessor>()
                    .Filter(f => !f.Hidden && f.INN == inn).FirstOrDefault();
            }
            if (subj != null)
                return subj.SocietySuccessor;

            return null;
        }

        
        public static T FindOrCreateDictByName<T>(IUnitOfWork uow, string name) where T : DictObject
        {
            if (String.IsNullOrEmpty(name)) return null;
            var obj = uow.GetRepository<T>().Filter(x => !x.Hidden && x.Name == name).FirstOrDefault();
            if (obj == null)
            {
                object[] paramss = new object[] { name };
                var dict = (T)Activator.CreateInstance(typeof(T), paramss);
                obj = uow.GetRepository<T>().Create(dict);
            }
            return (T)obj;
        }

    }
}
