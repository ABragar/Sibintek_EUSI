using Base;
using Base.Service;
using Base.UI;
using CorpProp.Services.Base;
using EUSI.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Model
{
    public static class ConsolidationAndReportMonitoringEventTypeModel
    {

        public static void Init(IInitializerContext context)
        {
            context.CreateVmConfig<ConsolidationAndReportMonitoringEventType>(nameof(ConsolidationAndReportMonitoringEventType))
                  .Service<IBaseObjectService<ConsolidationAndReportMonitoringEventType>>()
                  .Title("События выполнения контроля для БЕ")
                  .IsReadOnly()
                  .DetailView
                  (dv => dv.Title("Событие выполнения контроля для БЕ")
                  .Editors
                    ( eds => eds.Clear()
                     .Add(ed => ed.PlanDay)
                  ))
            .ListView(lv => lv.Title("Событие выполнения контроля для БЕ")
                .Columns(cols => cols.Clear()
                .Add(ed => ed.PlanDay)
                 ));
        }



    }

}

