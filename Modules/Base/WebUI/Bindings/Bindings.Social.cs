using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base.Security.Service;
using Base.Social.Service.Abstract.Components;
using Base.Social.Service.Concrete.Components;
using Data;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class SocialBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Social.Initializer>();
            container.Register<IVoitingService, VoitingService>();
            container.Register<ICommentsService, CommentsService>();
        }
    }
}