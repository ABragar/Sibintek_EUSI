using Base;
using Base.DAL;
using Base.Entities;
using Base.UI;
using BaseCatalog.Entities;

namespace BaseCatalog
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Measure>()
              .Title("Единицы измерения")
              .DetailView(x => x.Title("Единица измерения"));

            context.CreateVmConfig<MeasureCategory>()
                .Title("Единицы измерения - Категория")
                .DetailView(x => x.Title("Единицы измерения - Категория"));


            context.DataInitializer("BaseCatalog", "0.1", () =>
            {
                initMeasure(context.UnitOfWork);
            });
        }

        private void initMeasure(IUnitOfWork unitOfWork)
        {

            var repositoryCategory = unitOfWork.GetRepository<MeasureCategory>();
            var repositoryItem = unitOfWork.GetRepository<Measure>();

            //---->
            var category = new MeasureCategory()
            {
                Name = "Единицы длины"
            };

            repositoryCategory.Create(category);

            unitOfWork.SaveChanges();

            repositoryItem.Create(new Measure()
            {
                CategoryID = category.ID,
                Code = "006",
                Symbol = "м",
                Description = "Метр",
            });

            //---->
            category = new MeasureCategory()
            {
                Name = "Единицы площади"
            };

            repositoryCategory.Create(category);

            unitOfWork.SaveChanges();

            repositoryItem.Create(new Measure()
            {
                CategoryID = category.ID,
                Code = "055",
                Symbol = "м2",
                Description = "Квадратный метр",
            });

            //---->
            category = new MeasureCategory()
            {
                Name = "Единицы объема"
            };

            repositoryCategory.Create(category);

            unitOfWork.SaveChanges();

            repositoryItem.Create(new Measure()
            {
                CategoryID = category.ID,
                Code = "113",
                Symbol = "м3",
                Description = "Кубический метр",
            });

            //---->
            category = new MeasureCategory()
            {
                Name = "Единицы массы"
            };

            repositoryCategory.Create(category);

            unitOfWork.SaveChanges();

            repositoryItem.Create(new Measure()
            {
                CategoryID = category.ID,
                Code = "166",
                Symbol = "кг",
                Description = "Килограмм",
            });

            //---->
            category = new MeasureCategory()
            {
                Name = "Экономические единицы"
            };

            repositoryCategory.Create(category);

            unitOfWork.SaveChanges();

            repositoryItem.Create(new Measure()
            {
                CategoryID = category.ID,
                Code = "796",
                Symbol = "шт",
                Description = "Штука",
            });

            unitOfWork.SaveChanges();

        }
    }
}
