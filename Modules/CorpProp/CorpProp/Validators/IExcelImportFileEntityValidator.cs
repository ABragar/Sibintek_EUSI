using CorpProp.Entities.Import;
using System.Data;

namespace CorpProp.Validators
{
    public interface IExcelImportFileEntityValidator
    {
        void Validate(DataTable dataTable, ref ImportHistory history);
    }
}
