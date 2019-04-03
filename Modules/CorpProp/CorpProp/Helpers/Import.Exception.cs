using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{
    public class ImportException : Exception
    {
        public ImportException()
        {
        }

        public ImportException(string message) : base(message)
        {
        }
    }

    public enum ErrorType
    {
        System,
        Type,
        Required,
        ObjectNotFound,
        DictObjectNotFound,
        XML,
        Contact,
        BreaksBusinessRules
    }

    public static class ErrorTypeName
    {
        public static string System = "Системная ошибка";
        public static string Type = "Формат поля не соответствует утвержденному формату";
        public static string Required = "Отсутствует значение в обязательном поле";
        public static string ObjectNotFound = "Значение справочника отсутствует в АИС КС";
        public static string DictObjectNotFound = "Значение идентификатора отсутствует в АИС КС";
        public static string XML = "Несоответсвие схеме";
        public static string Contact = "Проверьте заполнение контактных данных";
        public static string BreaksBusinessRules = "Не соответствует бизнес правилам.";
        public static string InvalidFileNameFormat = "Файл не соответствует шаблону: некорректный формат наименования файла (<КОД_ГГГГ_MM_ДД_Наименование>).";
        public static string DuplicateRowErr = "Файл содержит дубликаты строк по связке ключевых полей Инвентарный номер и Балансодержатель.";
    }
}
