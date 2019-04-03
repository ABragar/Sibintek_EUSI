using Base;
using Base.ComplexKeyObjects.Common;
using Base.ComplexKeyObjects.Superb;
using Base.Extensions;
using Base.UI;
using Base.UI.Dashboard;
using System.Linq;
using WebUI.Models.ContentWidgets;

namespace WebUI
{
    public class WebUIInitilizer : IModuleInitializer
    {
        private readonly IDashboardService _dashboardService;


        public WebUIInitilizer(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<LayoutVm>("Content_Layout");
            context.CreateVmConfig<TextAreaVm>("Content_TextArea");
            context.CreateVmConfig<NavigationVm>("Content_Navigation");
            context.CreateVmConfig<TabsVm>("Content_Tabs");
            context.CreateVmConfig<QuoteVm>("Content_Quote");
            context.CreateVmConfig<InternalImageVm>("Content_Image_Internal");
            context.CreateVmConfig<ExternalImageVm>("Content_Image_External");
            context.CreateVmConfig<InternalVideoVm>("Content_Video_Internal");
            context.CreateVmConfig<InternalFileVm>("Content_File_Internal");
            context.CreateVmConfig<ExternalVideoVm>("Content_Video_External");
            context.CreateVmConfig<TestWidget<CheckboxTestItem>>("TestWidget_Checkboxes");
            context.CreateVmConfig<CheckboxTestItem>("TestWidget_CheckboxTestItem");

            _dashboardService.Register("Calendar")
                .Title("Календарь");

            _dashboardService.Register("Tasks")
                .Title("Задачи");
        }
    }
}