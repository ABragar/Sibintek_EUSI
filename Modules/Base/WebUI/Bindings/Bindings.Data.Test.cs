using Base.Security.Service;
using Common.Data;
using Common.Data.Entities.Test;
using Common.Data.Entities.Test.Map;
using Common.Data.Service.Abstract;
using Common.Data.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class DataTestBindings
    {
        public static void Bind(Container container)
        {
            container.Register<TestInitializer>();

            container.Register<ITestObject3Service, TestObject3Service>();
            container.Register<ITestObject3CategoryService, TestObject3CategoryService>();

            container.Register<ITestObjectService, TestObjectService>();
            container.Register<ITestObjectEntryService, TestObjectEntryService>();
            container.Register<ITestObjectNestedEntryService, TestObjectNestedEntryService>();

            //Test Map Objects
            container.Register<ITestBaseMapObjectService<TestBaseMapObject>, TestBaseMapObjectService<TestBaseMapObject>>();
            container.Register<ITestBaseMapObjectService<TestMarkerMapObject>, TestBaseMapObjectService<TestMarkerMapObject>>();
            container.Register<ITestBaseMapObjectService<TestPathMapObject>, TestBaseMapObjectService<TestPathMapObject>>();

            //Test Profile
            container.Register<ICrudProfileService<TestBaseProfile>, CrudProfileService<TestBaseProfile>>();

            //TestFieldWizard
            //container.Register<ITestFieldWizardService, TestFieldWizardService>();
        }
    }
}