using System.Collections.Generic;
using Base.Social.Entities.Components;
using Base.UI.ViewModal;

namespace Base.Social.Extensions
{
    public static class ViewModelConfigBuilderData
    {
        internal static void AddStaticticToolbarIfNeeded(this ViewModelConfig config)
        {

            if (typeof(IVoitingState).IsAssignableFrom(config.TypeEntity))
            {
                config.DetailView.Toolbars.Add(new Toolbar()
                {
                    AjaxAction =
                        new AjaxAction
                        {
                            Name = "GetVoitingToolbar",
                            Controller = "Social",
                            Params =
                                new Dictionary<string, string>() { { "mnemonic", config.Mnemonic }, { "objectID", "[ID]" } }
                        },
                    Title = "Действия",
                    IsAjax = true,
                });
            }

            if (typeof(ICommentsState).IsAssignableFrom(config.TypeEntity))
            {
                config.DetailView.Toolbars.Add(new Toolbar()
                {
                    AjaxAction =
                        new AjaxAction
                        {
                            Name = "GetCommentToolbar",
                            Controller = "Social",
                            Params =
                                new Dictionary<string, string>() { { "mnemonic", config.Mnemonic }, { "objectID", "[ID]" } }
                        },
                    Title = "Действия",
                    IsAjax = true,
                });
            }
        }
    }
}
