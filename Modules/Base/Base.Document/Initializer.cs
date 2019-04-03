using Base.Document.Entities;
using Base.Document.Services.Abstract;
using Base.UI;

namespace Base.Document
{

    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<BaseDocument>("JournalDocument")
                .Title("Базовый документ")
                .Service<IBaseDocumentService<BaseDocument>>()
                .ListView(l => l.Title("Журнал документов").Columns(cols => cols.Add(c => c.ExtraID, x => x.Visible(true))))
                .DetailView(d => d.Title("Базовый документ"));

            context.CreateVmConfig<Contract>()
                .Title("Договор")
                .Service<IBaseDocumentService<Contract>>()
                .DetailView(d => d.Title("Договор"))
                .ListView(l => l.Title("Договор"));

            context.CreateVmConfig<UnifiedDocument>()
               .Title("Документ")
               .Service<IBaseDocumentService<UnifiedDocument>>()
               .DetailView(d => d.Title("Документ"))
               .ListView(l => l.Title("Документы"));
        }
    }
}
