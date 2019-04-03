using Base.UI;
using Base.UI.Editors;
using Base.UI.Presets;
using CorpProp.Entities.Estate;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Extentions
{
    public static class Extentions
    {

        /// <summary>
        /// Возвращает значение даты из строки формата dd.MM.yyyy
        /// </summary>
        /// <param name="dateStr">Дата строкой в формате dd.MM.yyyy</param>
        /// <returns></returns>
        public static DateTime? GetDate(this string dateStr)
        {
            DateTime date = DateTime.MinValue;
            if (
            DateTime.TryParseExact(dateStr,
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out date))
                return date;
            else
                 if (DateTime.TryParse(dateStr,out date))
                return date;
            else
                return null;
        }


        /// <summary>
        /// Добавляет ассоциацию многие ко многим.
        /// </summary>
        /// <param name="eds"></param>
        /// <param name="sysName"></param>
        /// <param name="manyToManyType"></param>
        /// <param name="manyToManyLeftOrRightType"></param>
        /// <param name="associationType"></param>
        /// <param name="action"></param>
        public static List<EditorViewModel> AddManyToMany(
            this List<EditorViewModel> eds
            , string sysName
            , Type manyToManyType
            , Type manyToManyLeftOrRightType
            , ManyToManyAssociationType associationType
            , Action<ManyToManyAssociationEditorBuilder> action)
        {
            ManyToManyAssociationEditor editor = null;
            if (sysName != null)
                editor = (ManyToManyAssociationEditor)eds.SingleOrDefault(x => x.SysName == sysName);
            if (editor != null) return eds;
            editor = new ManyToManyAssociationEditor(manyToManyType, associationType)
            {
                SysName = string.IsNullOrWhiteSpace(sysName) ? Guid.NewGuid().ToString("N"): sysName,
                PropertyName = null,
                PropertyDataTypeName = "",
                PropertyType = manyToManyType.GetInterfaces()
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == manyToManyLeftOrRightType),
                Title = "",
                IsLabelVisible = false,
                SortOrder = eds.Max(x => x.SortOrder) + 1,
                Type = EditorAssociationType.InLine
            };

            action?.Invoke(new ManyToManyAssociationEditorBuilder(editor));
            eds.Add(editor);
            return eds;
        }

        /// <summary>
        /// Добавляет в DetailView указанный редактор.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conf"></param>
        /// <param name="ed"></param>
        /// <param name="tabName"></param>
        public static void AddEditor<T>(
         this ViewModelConfigBuilder<T> conf
          , EditorViewModel ed
          , string tabName) where T : Estate
        {

            if (ed != null && !conf.Config.DetailView.Editors.Contains(ed))
            {
                ed.TabName = tabName;
                conf.Config.DetailView.Editors.Add(ed);
            }
        }


        /// <summary>
        /// Удаляет пустые пункты меню.
        /// </summary>
        /// <param name="me"></param>
        public static void RemoveNulls(
            this Base.UI.Presets.MenuElement me
        )
        {
            if (me.Children.Count == 0)
                return;

            (me.Children as List<Base.UI.Presets.MenuElement>).RemoveAll(f => f.Name.ToLower() == "null");
            if (me.Children.Count == 0)
                return;
            foreach (MenuElement child in me.Children)
            {
                child.RemoveNulls();
            }
            return;
        }

        public static byte[] ToByteArray(this System.IO.Stream stream)
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// Возвращает список видимых таблиц Excel.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<DataTable> GetVisbleTables(this IExcelDataReader reader)
        {
            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = x => new ExcelDataTableConfiguration() { UseHeaderRow = false } });
            return result.GetVisbleTables();

        }

        /// <summary>
        /// Возвращает список видимых таблиц DataSet.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<DataTable> GetVisbleTables(this DataSet ds)
        {
            return
                ds.Tables.Cast<DataTable>()
                .Where(f => f.ExtendedProperties["visiblestate"]?.ToString()?.ToLower() == "visible")
                .ToList();

        }
    }
}
