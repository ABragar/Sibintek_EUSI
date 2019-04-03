using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DAL.EF;
using DAL.Entities;
using ReportStorage.Service;

namespace RestService.Helpers
{
    public class ExportNameFormatter
    {
        private readonly IReportStorageService _reportStorageService;

        private const string ChangeNameParameter = "reportExportName";

        private const string SelectQueryNameFormat = "{0}_SelectQueryFormat";

        private const string DateTimeRegex = "^(?i)DateTime:.*$";
        private const string ParameterRegex = @"^(?i)ParameterName:.*;Multiple:(false|true)$";
        private const string ReportPropertyRegex = @"^(?i)ReportProperty:@?[a-zA-Z_]\w*(\.@?[a-zA-Z_]\w*)*$";

        private const string JoinSeparator = "_";
        private const string ValueSeparator = ":";
        private const char ParamsSeparator = ';';


        private const string SplitFormatRegex = @"\{([^{}]*)}";

        public ExportNameFormatter(
            IReportStorageService reportStorageService)
        {
            _reportStorageService = reportStorageService;
        }

        /// <summary>
        /// Возвращает имя файла в соответствии с форматом указанным в отчете
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="fileName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetDocumentName(string templateName, string fileName, Dictionary<string, object> parameters)
        {
            if (!Guid.TryParse(Path.GetFileNameWithoutExtension(templateName), out var guid))
            {
                throw new ArgumentException("Template name is not valid");
            }

            var format = parameters[ChangeNameParameter]?.ToString();
            var extension = Path.GetExtension(fileName);
            var fileNameFormatted = format;
            var report = GetReport(guid);

            if (report == null || string.IsNullOrEmpty(format))
                return extension;

            var expressions = SplitFormat(format);

            foreach (var expression in expressions)
            {
                // отрезаем фигурные скобки
                var trimmedExpression = expression.Substring(1, expression.Length - 2);

                if (Regex.IsMatch(trimmedExpression, ParameterRegex))
                {
                    fileNameFormatted = fileNameFormatted.Replace(expression, GetParamValue(trimmedExpression, parameters));
                }
                else if (Regex.IsMatch(trimmedExpression, ReportPropertyRegex))
                {
                    fileNameFormatted = fileNameFormatted.Replace(expression, GetReportProperty(trimmedExpression, report));
                }
                else if (Regex.IsMatch(trimmedExpression, DateTimeRegex))
                {
                    fileNameFormatted = fileNameFormatted.Replace(expression, GetDateTime(trimmedExpression));
                }
            }


            return $"{fileNameFormatted}{extension}";
        }

        public bool IsChangeNameRequired(Dictionary<string, object> parameters)
        {
            return parameters.ContainsKey(ChangeNameParameter);
        }

        /// <summary>
        /// Возвращает отчет из БД найденный по Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private Report GetReport(Guid guid)
        {
            using (var context = new ReportDbContext())
            {
                return _reportStorageService.Get(context, guid);
            }
        }

        /// <summary>
        /// Парсит формат на составные части
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Массив параметров заключенных в фигурные скобки</returns>
        private static IEnumerable<string> SplitFormat(string format)
        {
            return Regex.Matches(format, SplitFormatRegex).Cast<Match>().Select(m => m.Groups[0].Value).ToArray();
        }

        /// <summary>
        /// Возвращает значение параметров отчета описанных в выражении формата "ParameterName:{Name};Multiple:{Boolean}"
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetParamValue(string expression, IReadOnlyDictionary<string, object> parameters)
        {

            var args = expression.Split(ParamsSeparator);

            var paramName = GetDescriptorValue(args[0]);
            var isMultiple = bool.Parse(GetDescriptorValue(args[1]));

            if (!parameters.ContainsKey(paramName) || parameters[paramName] == null)
                return string.Empty;

            // Дотаем параметры для селект запроса из выражения
            var select = string.Empty;
            var isExistSelectQueryForParam = parameters.ContainsKey(string.Format(SelectQueryNameFormat, paramName));
            if (isExistSelectQueryForParam)
            {
                select = parameters[string.Format(SelectQueryNameFormat, paramName)].ToString();
            }

            // Если параметр поддерживает множественный выбор то берем первые 10
            if (isMultiple)
            {
                var values = parameters[paramName] as IEnumerable<object>;
                var stringValues = values?.Select(v => v.ToString()).ToList();

                if (stringValues == null || !stringValues.Any())
                    return string.Empty;

                var resultList = isExistSelectQueryForParam
                    ? stringValues.Take(10)
                        .Select(value => SelectValue(select, value))
                    : stringValues.Take(10);

                return $"{string.Join(JoinSeparator, resultList)}";
            }

            return isExistSelectQueryForParam
                ? SelectValue(select, parameters[paramName].ToString())
                : parameters[paramName].ToString();
        }

        /// <summary>
        /// Возвращает значение свойства объекта Report указонного в выражении формата "ReportProperty:{PropertyName}"
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        private static string GetReportProperty(string expression, Report report)
        {
            var propertyName = GetDescriptorValue(expression);
            var pi = report.GetType().GetProperty(propertyName);

            return pi?.GetValue(report, null)?.ToString();
        }

        /// <summary>
        /// Возвращает значение для выражения формата "DateTime:{format}"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Дату в формате описанной в дескрипторе</returns>
        private static string GetDateTime(string expression)
        {
            return DateTime.Now.ToString(GetDescriptorValue(expression));
        }

        /// <summary>
        /// Значение указанное в выражении формата "Свойство:Значение"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetDescriptorValue(string expression)
        {
            var startIndex = expression.IndexOf(ValueSeparator, StringComparison.Ordinal) + 1;
            return expression.Substring(startIndex, expression.Length - startIndex);
        }

        private static string SelectValue(string queryFormat, string value)
        {
            if (string.IsNullOrEmpty(queryFormat) || string.IsNullOrEmpty(value))
                return string.Empty;

            using (var context = new ReportDbContext())
            {
                var query = string.Format(queryFormat, value);
                return context.Database.SqlQuery<string>(query).FirstOrDefault();
            }
        }
    }
}