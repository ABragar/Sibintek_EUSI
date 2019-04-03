using Base.DAL;
using Base.DAL.EF;
using CorpProp.Analyze.Entities.Accounting;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Analyze.Entities.Subject;

namespace CorpProp.Analyze
{
    public class AnalyzeConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)

                #region CorpProp.Analyze.Entities.NSI

                .Entity<BudgetLine>()
                .Entity<FinancialIndicator>()

                #endregion

                .Entity<AnalyzeSociety>(en => en
                        .Save(sav => sav
                            .SaveOneObject(s => s.Owner)
                        ))
                .Entity<FinancialIndicatorItem>(en => en
                        .Save(sav => sav
                            .SaveOneObject(s => s.Owner)
                            .SaveOneObject(s => s.FinancialIndicator)
                        ))
                .Entity<RecordBudgetLine>(en => en
                        .Save(sav => sav
                            .SaveOneObject(s => s.Owner)
                            .SaveOneObject(s => s.BudgetLine)
                        ))
                .Entity<BankAccount>(en => en
                        .Save(sav => sav
                            .SaveOneObject(s => s.Society)
                            .SaveOneObject(s => s.Currency)
                        ));
        }
    }
}