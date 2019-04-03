using System;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using Base.DAL;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.DefaultData;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;

namespace CorpProp.Analyze.DefaultData
{
    public class FillDataStrategy : IFillDataStrategy<DefaultDataHolder>
    {
        public void FillData(IDefaultDataHelper helper, IUnitOfWork uow, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                if (data.BudgetLines != null) helper.CreateDictItem<BudgetLine>(uow, data.BudgetLines);

                if (data.FinancialIndicators != null) helper.CreateDictItem<FinancialIndicator>(uow, data.FinancialIndicators);

                if (data.NSIs != null) helper.CreateNSI(uow, data.NSIs);

                uow.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public void FillContext(IDefaultDataHelper helper, DbContext context, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
    }
}