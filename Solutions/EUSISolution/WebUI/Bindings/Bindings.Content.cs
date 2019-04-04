using Base.Content.Service.Abstract;
using Base.Content.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ContentBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Content.Initializer>();
            container.Register<IContentCategoryService, ContentCategoryService>();
            container.Register<IContentItemService, ContentItemService>();
            container.Register<IContentSubscriberService, ContentSubscriberService>();

            container.Register<ICourseCategoryService, CourseCategoryService>();

            container.Register<IExerciseService, ExerciseService>();

            container.Register<IJournalEntryService, JournalEntryService>();
            container.Register<IExerciseResultService, ExerciseResultService>();

            container.Register<IQuestionService, QuestionService>();

            container.Register<IAnswerService , AnswerService >();

            container.Register<ITagService, TagService>();

            container.Register<IQuestionContentCategoryService, QuestionContentCategoryService>();

        }
    }
}