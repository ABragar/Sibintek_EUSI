using Base.Word.Services.Abstract;
using Base.Word.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class WordBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Word.Initializer>();
            container.Register<IWordService, WordService>(Lifestyle.Singleton);
            container.Register<IPrintingService,PrintingService>(Lifestyle.Singleton);
            container.Register<IPrintingSettingsService, PrintingSettingsService>();
        }
    }
}