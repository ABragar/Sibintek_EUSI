﻿using CorpProp.Analyze.Services.Accounting;
using CorpProp.Analyze.Services.Subject;
using CorpProp.Services.Subject;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace WebUI.Bindings
{
    public class AnalyzeBindings
    {
        public static void Bind(Container container)
        {
            container.Register<CorpProp.Analyze.Initializer>();

            container.Register<IBankAccountService, BankAccountService>();
            container.Register<IFinancialIndicatorItemService, FinancialIndicatorItemService>();
            container.Register<IRecordBudgetLineService, RecordBudgetLineService>();
            container.Register<IAnalyzeSocietyService, AnalyzeSocietyService>();
        }
    }
}