using Base.Content.Entities;
using Base.Content.Service.Abstract;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Content
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<ContentItem>()
                .Service<IContentItemService>()
                .Icon("glyphicon glyphicon-notes", "#F0AD4E")
                .Title("Контент")
                .DetailView(x => x.Title("Контент"))
                .ListView(x => x.Title("Контент").HiddenActions(new[] { LvAction.AllCategorizedItems, LvAction.Settings, }));

            context.CreateVmConfig<ContentCategory>()
                .Service<IContentCategoryService>()
                .Title("Контент - Категории")
                .DetailView(x => x.Title("Категория").Editors(eds => eds.Add(e => e.ID, e => e.Visible(true))));

            context.CreateVmConfig<ContentSubscriber>()
                .Service<IContentSubscriberService>()
                .Title("Контент - Категории - Подписчики")
                .DetailView(x => x.Title("Подписчик"))
                .ListView(x => x.Title("Подписчики"));

            context.CreateVmConfig<Tag>()
                .Service<ITagService>()
                .Title("Контент - Тэги")
                .DetailView(x => x.Title("Тэг"))
                .ListView(x => x.Title("Тэги"));

            context.CreateVmConfig<CourseCategory>()
                .Service<ICourseCategoryService>()
                .Title("Учебные курсы")
                .DetailView(x => x.Title("Учебный курс/урок"))
                .ListView(x => x.Title("Учебные курсы/уроки"));

            context.CreateVmConfig<Exercise>()
                .Service<IExerciseService>()
                .Title("Учебные курсы - Задания")
                .DetailView(x => x.Title("Учебное задание"))
                .ListView(x => x.Title("Учебные задания"));

            context.CreateVmConfig<JournalEntry>()
                .Service<IJournalEntryService>()
                .Title("Учебные курсы - Статистика по обучению")
                .DetailView(x => x.Title("Запись"))
                .ListView(x => x.Title("Статистика по обучению"));

            context.CreateVmConfig<ExerciseResult>()
                .Service<IExerciseResultService>()
                .Title("Учебные курсы - Журнал прогресса - Результаты")
                .DetailView(x => x.Title("Результат выполнения учебного задания"))
                .ListView(x => x.Title("Результаты выполнения учебных заданий"));

            context.CreateVmConfig<Question>()
                .Service<IQuestionService>()
                .Title("ЧАВО - Вопросы")
                .DetailView(x => x.Title("Вопрос"))
                .ListView(x => x.Title("Вопросы"));

            context.CreateVmConfig<Answer>()
                .Service<IAnswerService>()
                .Title("ЧАВО - Ответы")
                .DetailView(x => x.Title("Ответ"))
                .ListView(x => x.Title("Ответы"))
                .LookupProperty(x => x.Text(e => e.Value));

            context.CreateVmConfig<TagCategory>()
                .Title("Категории тэгов")
                .DetailView(x => x.Title("Категория"))
                .ListView(x => x.Title("Категория"));

            context.CreateVmConfig<ContentItem>("News")
                .Title("Новости")
                .Service<IContentItemService>()
                .DetailView(x => x.Title("Новость"));

            context.CreateVmConfig<ContentItem>("PublicNews")
                .Title("Новости")
                .Service<IContentItemService>()
                .DetailView(x => x.Title("Новость")
                .Editors(eds => eds
                        .Add(e => e.ImagePreview, e => e.Visible(true))
                        .Add(e => e.Value, e => e.Visible(false))
                        .Add(e => e.Top, e => e.Visible(false))
                        .Add(e => e.ContentItemStatus, e => e.Visible(false))
                        .Add(e => e.Src, e => e.EditorTemplate("Url"))
                        .Add(e => e.Created, e => e.Visible(false))
                        .Add(e => e.Tags, e => e.Visible(false))
                        .Add(e => e.Title, e => e.Visible(true))
                        .Add(e => e.Description, e => e.Visible(true))
                        .Add(e => e.Date, e => e.Visible(true))
                        .Add(e => e.Content, e => e.Visible(true))
                    )
            );
        }
    }
}
