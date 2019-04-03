using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Estate
{
    public class CreateCadastralQueryBuilder
    {
        public string GetUpdateQuery(IUpdateEstateHistoryStrategy strategy)
        {
            StringBuilder mergeSqlScript = new StringBuilder();
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetValuesSpecification()})");
            return mergeSqlScript.ToString();
        }

        public string GetCreateHistory(Entities.Estate.Estate estate)
        {
            return string.Empty;
        }
    }

    public interface IUpdateEstateHistoryStrategy
    {
    }
}