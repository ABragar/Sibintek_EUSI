using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Mapping;

namespace EUSI.Services.Mapping
{
    public interface IERTypeERReceiptReasonService : IBaseObjectService<ERTypeERReceiptReason>, IExcelImportEntity, ISystemImportEntity
    { }

    public class ERTypeERReceiptReasonService : BaseObjectService<ERTypeERReceiptReason>, IERTypeERReceiptReasonService
    {
        public ERTypeERReceiptReasonService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        public void Import(
            IUnitOfWork uofw, 
            IUnitOfWork histUofw, 
            DataTable table, 
            Dictionary<string, string> colsNameMapping, 
            ref int count, 
            ref ImportHistory history)
        {
            try
            {

                for (int r = 9; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    string err = "";
                    var obj = new ERTypeERReceiptReason();
                    obj.FillObject(uofw, typeof(ERTypeERReceiptReason),
                        row, row.Table.Columns, ref err, ref history, colsNameMapping, IdentyDictByCode());
                    if (obj.ID != 0)
                        uofw.GetRepository<ERTypeERReceiptReason>().Update(obj);
                    else
                        uofw.GetRepository<ERTypeERReceiptReason>().Create(obj);
                    uofw.SaveChanges();
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }
        public bool IdentyDictByCode()
        {
            return false;
        }

        public void CancelImport(IUnitOfWork uofw, ref ImportHistory history)
        {
            throw new NotImplementedException();
        }
    }
}
