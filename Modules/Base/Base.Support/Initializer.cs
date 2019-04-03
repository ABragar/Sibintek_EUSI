using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.Support.Entities;
using Base.Support.Services.Abstract;
using Base.UI;

namespace Base.Support
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<BaseSupport>()
                .Service<IBaseSupportService<BaseSupport>>()
                .Title("Обратная связь - Базовый тип")
                .DetailView(d => d.Title("Поддержка"))
                .ListView(l => l.Title("Поддержка"));


            context.CreateVmConfig<SupportRequest>()
                .IsReadOnly()
                .Title("Обратная связь - Обращение")
                .Service<IBaseSupportService<SupportRequest>>()
                .DetailView(d => d
                    .Title("Обращение")
                    .Editors(edit => edit
                        .Add<BpHistory>("History", x => x.Title("История сообщении").IsReadOnly().DataType(PropertyDataType.Custom))))
                .ListView(l => l.Title("Обращения"));

            context.CreateVmConfigOnBase<SupportRequest>(nameof(SupportRequest), "ApplySupportRequest")
                .IsReadOnly(false)
                .Title("Обратная связь - Создание обращение")
                .Service<IBaseSupportService<SupportRequest>>()
                .DetailView(d => d
                    .Editors(x => x
                        .Add<BpHistory>("History", el => el.Visible(false))
                        .Add(el => el.Status, el => el.Visible(false))
                ));

            context.CreateVmConfig<SupportFile>()
                .Title("Обратная связь - Прикрепленные файлы")
                .DetailView(x => x.Title("Прикрепленный файл"));
        }
    }
}