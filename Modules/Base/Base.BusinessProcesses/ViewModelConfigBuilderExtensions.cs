using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Base.BusinessProcesses.Entities;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.BusinessProcesses
{

    public static class ViewModelConfigBuilderExtensions
    {
        public static ViewModelConfigBuilder<T> AddBusinessProcessesToolbar<T>(this ViewModelConfigBuilder<T> builder)
            where T : class, IBPObject
        {
            builder.SetAdditional(Flag);
            builder.Config.AddBusinessProcessesToolbarIfNeeded();

            return builder;
        }

        public static void RemoveBusinessProcessesToolbar<T>(this ViewModelConfigBuilder<T> builder)
            where T : class, IBPObject
        {
            builder.SetAdditional(Flag);
        }

        private class IgnoreToolbarFlag
        {
        };

        private static readonly IgnoreToolbarFlag Flag = new IgnoreToolbarFlag();

        internal static void AddBusinessProcessesToolbarIfNeeded(this ViewModelConfig config)
        {

            if (typeof (IBPObject).IsAssignableFrom(config.TypeEntity) && config.GetAdditional<IgnoreToolbarFlag>() == null)
            {
                config.DetailView.Toolbars.Add(new Toolbar()
                {
                    AjaxAction =
                        new AjaxAction
                        {
                            Name = "GetToolbar",
                            Controller = "BusinessProcesses",
                            Params =
                                new Dictionary<string, string>() {{"mnemonic", config.Mnemonic}, {"objectID", "[ID]"}}
                        },
                    Title = "Действия",
                    IsAjax = true,
                });
            }
        }



    }

}