using Base;
using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CorpProp.DefaultData
{
    /// <summary>
    /// Класс, предоставляющий методы для создания данных по умолчанию.
    /// </summary>
    public class DefaultDataHelper : IDefaultDataHelper
    {
        public static IDefaultDataHelper InstanceSingletone { get; } = new DefaultDataHelper();

        /// <summary>
        /// Получает XML файл из ресурсов проекта по его названию.
        /// </summary>
        /// <param name="assembly">Сборка, в которую включен ресурс.</param>
        /// <param name="resourceName">Полное наименование файла с указанием сборки.</param>
        /// <returns>XML документ.</returns>
        public XDocument GetXmlFileFromResource(System.Reflection.Assembly assembly, String resourceName)
        {
            try
            {
                if (assembly == null || String.IsNullOrEmpty(resourceName)) return null;
                XDocument xmldoc = null;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        xmldoc = XDocument.Load(stream);
                }
                return xmldoc;
            }
            catch { return null; }

        }

        public static String ReadTextFromResource(string resourceName)
        {
            string text = null;

            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(f => f.GetManifestResourceNames().Contains(resourceName))
                .FirstOrDefault();

            using (Stream rstream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader sr = new StreamReader(rstream))
                {                     
                    text = sr.ReadToEnd();
                }
            }
            
            return text;
        }

        /// <summary>
        /// Десериализация объекта типа T из потока.        
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Десериализованный объект.</returns>
        public T DeserializeFromStream<T>(Stream stream) where T : class
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
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
            return obj;
        }

        static XmlAttributeOverrides XmlAttributeOverrides()
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attribs = new XmlAttributes();
            attribs.XmlIgnore = true;
            var ignoreAttrs = new List<Tuple<Type, string>>()
                              {
                                  new Tuple<Type, string>(
                                                          typeof(CardFolder),
                                                          nameof(CardFolder.Children_))
                                  ,
                                  new Tuple<Type, string>(
                                                          typeof(CardFolder),
                                                          nameof(CardFolder.Children))
                                  ,

                                  new Tuple<Type, string>(
                                                          typeof(CardFolder),
                                                          nameof(CardFolder.Parent)),

                                  new Tuple<Type, string>(
                                                          typeof(HCategory),
                                                          nameof(HCategory.sys_all_parents))
                              };

            foreach (var ignoreAttr in ignoreAttrs)
            {
                attribs.XmlElements.Add(new XmlElementAttribute(ignoreAttr.Item2));
                overrides.Add(ignoreAttr.Item1, ignoreAttr.Item2, attribs);
            }
            return overrides;
        }

        /// <summary>
        /// Десериализация объекта типа T из XMl ресурса проекта.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="assembly">Сборка, в которой присутсвует ресурс.</param>
        /// <param name="resourceName">Наименование ресурса.</param>
        /// <returns>Объект.</returns>
        public T DeserializeFromXmlResource<T>(System.Reflection.Assembly assembly, String resourceName) where T : class
        {
            T obj = null;
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null && stream.Length > 0)
                    {
                        stream.Position = 0;
                        System.Xml.Serialization.XmlSerializer reader =
                            new System.Xml.Serialization.XmlSerializer(typeof(T), XmlAttributeOverrides());
                        obj = (T) reader.Deserialize(stream);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return obj;
        }

        /// <summary>
        /// Десериализация из XMl ресурса.
        /// </summary>
        /// <param name="ttype">Тип десериализуемого объекта.</param>
        /// <param name="assembly">Сборка.</param>
        /// <param name="resourceName">Наименование ресурса.</param>
        /// <returns>Объект.</returns>
        private object DeserializeFromXmlResource(Type ttype, System.Reflection.Assembly assembly, String resourceName)
        {
            object obj = null;
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null && stream.Length > 0)
                    {
                        stream.Position = 0;
                        System.Xml.Serialization.XmlSerializer reader =
                            new System.Xml.Serialization.XmlSerializer(ttype);
                        obj = reader.Deserialize(stream);
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
            return obj;
        }

        /// <summary>
        /// Читаем XML, создаем объект DefaultDataHolder.
        /// </summary>      
        /// <returns>Объект DefaultDataHolder.</returns>
        private T ReadDataHolder<T>(string resourceName) where T: class
        {
            T data = null;
            try
            {
                 data = DeserializeFromXmlResource<T>(typeof(T).Assembly, resourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return data;
        }

        void IDefaultDataHelper.CreateDictItem<T>(IUnitOfWork uow, List<T> items)
        {
            try
            {
                Parallel.ForEach(items, (item) =>
                {
                    if (String.IsNullOrEmpty(item.PublishCode))
                        item.PublishCode = item.Code;
                    item.TrimCode();
                });
                uow.GetRepository<T>().CreateCollection(items);
                uow.SaveChanges();
               
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
        }

        void IDefaultDataHelper.CreateDefaultItem<T>(IUnitOfWork uow, List<T> items)
        {
            try
            {               
                uow.GetRepository<T>().CreateCollection(items);
                uow.SaveChanges();               
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
        }


        void IDefaultDataHelper.CreateNSI(IUnitOfWork uow, List<NSI> items)
        {
            try
            {
                if (items != null)
                {
                    var rep = uow.GetRepository<NSI>();
                    string err = "";
                    foreach (var item in items)
                    {                        
                        var t = ImportHelper.GetDictByCode(uow, typeof(NSIType), item.NSIType?.Code, ref err);
                        item.NSIType = t as NSIType;
                        rep.Create(item);
                    }                       

                    uow.SaveChanges();
                }
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
        }

        /// <summary>
        /// Инициирует создание дефолтных значений в БД.
        /// </summary>
        /// <param name="uow">Сесия.</param>
        /// <remarks>
        /// В отличие от Generate может вызываться многократно и везде с использованием сессии UnitOfWork.
        /// </remarks>
        void IDefaultDataHelper.CreateDefaulData<T>(IUnitOfWork uow, IFillDataStrategy<T> dataStrategy)
        {
            try
            {
                var resourcesFolder = ((DataHolderAttribute)typeof(T).GetCustomAttribute(typeof(DataHolderAttribute))).ResourcesFolder;

                var resources = typeof(T).Assembly.GetManifestResourceNames()
                    .Where(f => f.Contains(resourcesFolder))
                    .ToList();

                foreach (var resourceName in resources)
                {
                    T data = ReadDataHolder<T>(resourceName);
                    if (data != null) dataStrategy.FillData(this, uow, data);
                }

               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
        
        /// <summary>
        /// Добавляет элементы.
        /// </summary>
        /// <typeparam name="T">Тип добавляемых элементов.</typeparam>
        /// <param name="context">Контекст</param>
        /// <param name="items">Элементы.</param>
        /// <remarks>
        /// Использовать с осторожностью,т.к. при каждом запуске приложения, будет добавлять в БД элементы без разбора!
        /// </remarks>
        void IDefaultDataHelper.AddItems<T>(DbContext context, List<T> items)
        {            
            context.Set<T>().AddRange(items);            
        }

        /// <summary>
        /// Добавляет или обновляет элементы справочников, наследуемых от DictObject, по ключу = полю "Code".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        public void AddDictObjects<T>(DbContext context, List<T> items) where T : DictObject
        {
            foreach (var item in items)            
                context.Set<T>().AddOrUpdate(i => i.Code, item);           
            
        }

       /// <summary>
       /// Инициирует наполнение БД дефолтными данными.
       /// </summary>
       /// <param name="unitOfWork">Сессия</param>
        void IDefaultDataHelper.InitDefaulData<T>(IUnitOfWork unitOfWork, IFillDataStrategy<T> dataStrategy) 
        {  
            bool SetDefaultData = false;
            bool.TryParse(ConfigurationManager.AppSettings["SetDefaultData"], out SetDefaultData);
            if (SetDefaultData)
                ((IDefaultDataHelper)this).CreateDefaulData<T>(unitOfWork, dataStrategy);
            unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Инициирует запрос к Repository.
        /// </summary>
        /// <param name="unitOfWork">Сессия</param>
        void IDefaultDataHelper.IsGetRepository<T>(IUnitOfWork unitOfWork, IFillDataStrategy<T> dataStrategy)
        {
            bool SetDefaultData = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsGetRepository"], out SetDefaultData);
            if (SetDefaultData)
            {
                var obj1 = unitOfWork.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().Find(0);
                var obj2 = unitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>().Find(0);
                var obj3 = unitOfWork.GetRepository<CorpProp.Entities.Estate.InventoryObject>().Find(0);
                var obj4 = unitOfWork.GetRepository<CorpProp.Entities.Estate.RealEstate>().Find(0);
                var obj5 = unitOfWork.GetRepository<CorpProp.Entities.Estate.Cadastral>().Find(0);
                var obj6 = unitOfWork.GetRepository<CorpProp.Entities.Estate.Land>().Find(0);
                var obj7 = unitOfWork.GetRepository<CorpProp.Entities.Estate.BuildingStructure>().Find(0);
                var obj8 = unitOfWork.GetRepository<CorpProp.Entities.Estate.PropertyComplexIO>().Find(0);
                var obj9 = unitOfWork.GetRepository<CorpProp.Entities.Estate.MovableEstate>().Find(0);
                var obj10 = unitOfWork.GetRepository<CorpProp.Entities.Estate.Vehicle>().Find(0);
                var obj11 = unitOfWork.GetRepository<CorpProp.Entities.Estate.Cadastral>().Find(0);
            }            
        }        
    }
}
