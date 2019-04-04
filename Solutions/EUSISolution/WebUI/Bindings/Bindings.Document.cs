using Base.CRM.Entities;
using Base.CRM.Services.Concrete;
using Base.Document.Entities;
using Base.Document.Services.Abstract;
using Base.Document.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class DocumentBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Document.Initializer>();
            container.Register<IBaseDocumentService<BaseDocument>, BaseDocumentService<BaseDocument>>();
            container.Register<IBaseDocumentService<UnifiedDocument>, UnifiedDocumentService>();
            container.Register<IBaseDocumentService<Deal>, DealService<Deal>>();
            container.Register<IBaseDocumentService<Contract>, ContractService>();
        }
    }
}