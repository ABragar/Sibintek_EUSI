using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.DAL.EF;
using CorpProp.Analyze.Entities.Accounting;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Analyze.Entities.Subject;

namespace Common.Data
{
    class AnalyzeConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)

                #region CorpProp.Analyze.Entities.NSI

                .Entity<BudgetLine>()
                .Entity<FinancialIndicator>()

                #endregion

                .Entity<AnalyzeSociety>()
                .Entity<FinancialIndicatorItem>()
                .Entity<RecordBudgetLine>()
                .Entity<BankAccount>();
        }
    }
}