using Base;
using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CorpProp.Entities.Export;

namespace CorpProp.Helpers.Export
{
    public static class ExcelExportHelper
    {

        public static string GetValue(Type _type, object value)
        {
            if (_type == typeof(String))
                return value.ToString();

            if (_type.IsSubclassOf(typeof(Consolidation)) || _type == typeof(Consolidation))
                return _type.GetProperty("Title")?.GetValue(value).ToString();

            if (_type.IsSubclassOf(typeof(DictObject)) || _type == typeof(DictObject))
                return _type.GetProperty("Name")?.GetValue(value).ToString();

            if (_type.IsSubclassOf(typeof(Society)) || _type == typeof(Society))
                return _type.GetProperty("ShortName")?.GetValue(value).ToString();
                       
            if (_type == typeof(DateTime))
                return DateTime.Parse(value.ToString()).ToString("dd.MM.yyyy");

            if (_type == typeof(Boolean))
                return (bool)value ? "ИСТИНА" : "ЛОЖЬ";

            if (_type == typeof(TypeObject) || _type.IsSubclassOf(typeof(TypeObject)))
                return _type.GetProperty("Name")?.GetValue(value).ToString();

            return value.ToString();
        }

        /// <summary>
        /// Таблица значений для Экспорта.
        /// </summary>
        /// <typeparam name="T">Экспортируемый тип.</typeparam>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="list">Список объектов для экспорта.</param>
        /// <param name="colsMap">Мэппинг колонок.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IUnitOfWork unitOfWork, IList<T> list, Dictionary<int, string> colsMap) where T : BaseObject
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            foreach (var col in colsMap)
            {
                PropertyDescriptor prop = props.Find(col.Value, false);
                if (prop != null || col.Value == "RowNumber")
                    table.Columns.Add(((prop == null)? col.Value : prop.Name), typeof(string));               
            }
            object[] values = new object[colsMap.Count];
            for (int j = 0; j < list.Count; j++)            
            {
                var item = list[j];
                //TODO: навигационные св-ва приходят пустыми                
                for (int i = 0; i < values.Length; i++)
                {    
                    if (colsMap.ContainsKey(i))
                    {
                        PropertyDescriptor prop = props.Find(colsMap[i], false);
                        if (prop == null && colsMap[i] == "RowNumber")
                        {
                            values[i] = j+1;
                            continue;
                        }
                        values[i] = (prop.GetValue(item) != null) ? GetValue(prop.GetValue(item)?.GetType(), prop.GetValue(item)) : "";

                        if (values[i] == null || string.IsNullOrEmpty(values[i].ToString()))
                        {

                            PropertyDescriptor idProp = prop.Name == "EUSINumber" ? props.Find("EstateID", false) : props.Find($"{colsMap[i]}ID", false);

                            int? navPropId = (int?) idProp?.GetValue(item);

                            if (navPropId != null)
                                values[i] = GetNavigationPropertyValue(unitOfWork, prop, (int) navPropId);
                        }
                    }
                    else
                        values[i] = null;
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Подготавливает Base64 строку архива.
        /// </summary>
        /// <param name="files">Список файлов для архива.</param>
        /// <returns>Base64 архива.</returns>
        public static string PrepareArchive(List<string> files)
        {
            string archivePath = $"{Path.GetDirectoryName(files.First())}Export.zip";
            string srcPath = $@"{Path.GetDirectoryName(files.First())}";
            //File.Create(archivePath);
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(System.Text.Encoding.GetEncoding("cp866")))
            {
                zip.AddDirectory(srcPath);
                zip.Save(archivePath);
            }

            byte[] bytes = File.ReadAllBytes(archivePath);
            string archiveBase64 = Convert.ToBase64String(bytes);
            if (Directory.Exists(Path.GetDirectoryName(archivePath)))
                Directory.Delete(Path.GetDirectoryName(archivePath), true);
            return archiveBase64;
        }

        /// <summary>
        /// Получение шаблона экспорта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="mnemonic">Мнемоника.</param>
        /// <param name="code">Код шаблона.</param>
        /// <returns>ExportTemplate</returns>
        public static ExportTemplate GetExportTemplate(IUnitOfWork unitOfWork, string mnemonic, string code)
        {
            return unitOfWork.GetRepository<ExportTemplate>()
                .Filter(f =>
                    !f.Hidden &&
                    !f.IsHistory &&
                    f.Mnemonic == mnemonic &&
                    f.Code == code
                )
                .Include(i => i.File)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получение имени элемента DictObject по ID.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="type">Тип.</param>
        /// <param name="id">ИД.</param>
        /// <returns>Имя объекта DictObject.</returns>
        public static string GetDictById(IUnitOfWork unitOfWork, Type type, int id)
        {
            try
            {
                MethodInfo methodUow = unitOfWork.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow?.MakeGenericMethod(type);
                var reposit = genericUow?.Invoke(unitOfWork, null);

                //фильтруем по коду
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "ID"),
                        Expression.Constant(id)
                    )
                    , parameter);

                MethodInfo method = reposit?.GetType().GetMethod("Filter");
                var filter = method?.Invoke(reposit, new object[] {lambda});

                object obj = ((IQueryable) filter)?.Provider.Execute(Expression.Call(
                    typeof(Enumerable),
                    "FirstOrDefault",
                    new Type[] {type},
                    ((IQueryable) filter).Expression));

                return type.GetProperty("Name")?.GetValue(obj, null).ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Получение значения навигационного свойства.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="propertyDescriptor">Описание свойства.</param>
        /// <param name="propertyId">Ид объекта навигационного свойства.</param>
        /// <returns></returns>
        public static string GetNavigationPropertyValue(IUnitOfWork unitOfWork, PropertyDescriptor propertyDescriptor, int propertyId)
        {
            if (propertyDescriptor.Name == "EUSINumber")
            {
                var eusiNumber = unitOfWork.GetRepository<Entities.Estate.Estate>().Find(f => f.ID == propertyId)?.EUSINumber;

                return (eusiNumber != null && !string.IsNullOrEmpty(eusiNumber.ToString())) ? eusiNumber.ToString() : "";
            }

            if (propertyDescriptor.PropertyType.IsSubclassOf(typeof(Consolidation)) || propertyDescriptor.PropertyType == typeof(Consolidation))
                return unitOfWork.GetRepository<Consolidation>().Find(f => f.ID == propertyId)?.Title;
            

            if (propertyDescriptor.PropertyType == typeof(DictObject) ||
                propertyDescriptor.PropertyType.IsSubclassOf(typeof(DictObject)))
                return GetDictById(unitOfWork, propertyDescriptor.PropertyType, propertyId);

            if (propertyDescriptor.PropertyType == typeof(Subject) || propertyDescriptor.PropertyType.IsSubclassOf(typeof(Subject)))
                return unitOfWork.GetRepository<Subject>().Find(f => f.ID == propertyId)?.ShortName;

            return null;
        }
    }
}
