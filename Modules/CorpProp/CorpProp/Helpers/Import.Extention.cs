using Base.Utils.Common;
using CorpProp.Entities.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers.Import.Extentions
{
    public static class ImportExtention
    {
        public static void AddError(
              this ICollection<ImportErrorLog> logs
            , Exception ex           
            )
        {
            logs.Add(new ImportErrorLog()
            {
                MessageDate = DateTime.Now,
                ErrorText = ex.ToStringWithInner(),
                ErrorType = GetErrorTypeName(ErrorType.System)
            });
        }

        public static void AddError(
             this ICollection<ImportErrorLog> logs
           , string text
           )
        {
            logs.Add(new ImportErrorLog()
            {
                MessageDate = DateTime.Now,
                ErrorText = text,
                ErrorType = GetErrorTypeName(ErrorType.System)
            });
        }

        public static void AddError(
             this ICollection<ImportErrorLog> logs
            , int? rowNumber
            , int? columnNumber
            , string propetyName
            , ErrorType errType
            , string sheet = ""
            )
        {
            logs.Add(new ImportErrorLog()
            {
                MessageDate = DateTime.Now,
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                PropetyName = propetyName,
                ErrorText = GetErrorTypeName(errType),
                ErrorType = GetErrorTypeName(errType),
                Sheet = sheet
            });
        }

        public static void AddError(
             this ICollection<ImportErrorLog> logs
            , int? rowNumber
            , int? columnNumber
            , string propetyName
            , string errorText
            , ErrorType errType
            , string sheet = ""
            )
        {
            logs.Add(new ImportErrorLog()
            {
                MessageDate = DateTime.Now,
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                PropetyName = propetyName,
                ErrorText = errorText,
                ErrorType = GetErrorTypeName(errType),
                Sheet = sheet
            });
        }

        public static void AddRequiredError(this ICollection<ImportErrorLog> logs,
            int? rowNumber, int? columnNumber, string propertyName, string sheet = "")
        {
            logs.AddError(rowNumber, columnNumber, propertyName, ErrorType.Required, sheet);
        }

        public static void AddInCorrectFormatError(this ICollection<ImportErrorLog> logs,
            int? rowNumber, int? columnNumber, string propertyName, string sheet = "")
        {
            logs.AddError(rowNumber, columnNumber, propertyName, ErrorType.Type, sheet);
        }

        public static void AddBreaksBusinessRulesError(this ICollection<ImportErrorLog> logs,
            int? rowNumber, int? columnNumber, string propertyName, string errorText, string sheet = "")
        {
            logs.AddError(rowNumber, columnNumber, propertyName, errorText, ErrorType.BreaksBusinessRules, sheet);
        }

        public static string GetErrorTypeName(ErrorType errType)
        {
            string str =  ErrorTypeName.System;
            switch (errType)
            {
                case ErrorType.System:
                    str = ErrorTypeName.System;
                    break;
                case ErrorType.Type:
                    str = ErrorTypeName.Type;
                    break;
                case ErrorType.Contact:
                    str = ErrorTypeName.Contact;
                    break;
                case ErrorType.Required:
                    str = ErrorTypeName.Required;
                    break;
                case ErrorType.ObjectNotFound:
                    str = ErrorTypeName.ObjectNotFound;
                    break;
                case ErrorType.DictObjectNotFound:
                    str = ErrorTypeName.DictObjectNotFound;
                    break;
                case ErrorType.XML:
                    str = ErrorTypeName.XML;
                    break;
                case ErrorType.BreaksBusinessRules:
                    str = ErrorTypeName.BreaksBusinessRules;
                    break;
                default:
                    str = ErrorTypeName.System;
                    break;
            }
            return str;
        }


        public static void AddError(
            this ICollection<ImportErrorLog> logs
            , int? rowNumber
            , int? columnNumber
            , string propetyName
            , string errorText
            , ErrorType errType
            , string eusiNumber
            , string inventoryNumber           
            , string sheet = ""
        )
        {
            logs.Add(new ImportErrorLog()
            {
                MessageDate = DateTime.Now,
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                PropetyName = propetyName,
                ErrorText = errorText,
                ErrorType = GetErrorTypeName(errType),
                Sheet = sheet,
                InventoryNumber = inventoryNumber,
                EusiNumber = eusiNumber
            });
        }
    }
      
       


}
