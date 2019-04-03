using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Forum.Entities;
using Base.Forum.Service;
using Base.UI;

namespace Base.Forum
{

    public class Initializer : IModuleInitializer
    {

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<ForumSection>()
                .Service<IForumSectionService>()
                .Title("Форум - Раздел")
                .ListView(x => x.Title("Раздел"))
                .DetailView(x => x.Title("Раздел"));

            context.CreateVmConfig<ForumTopic>()
                .Service<IForumTopicService>()
                .Title("Форум - Тема")
                .ListView(x => x.Title("Тема"))
                .DetailView(x => x.Title("Тема"));

            context.CreateVmConfig<ForumPost>()
                .Service<IForumPostService>()
                .Title("Форум - Сообщение")
                .ListView(x => x.Title("Сообщение"))
                .DetailView(x => x.Title("Сообщение"));
        }
    }
}
