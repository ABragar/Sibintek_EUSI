using System;
using System.Collections.Generic;
using System.Linq;
using Base.Extensions;
using Base.Service;
using Base.UI.Service;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class UiInitializerContext
    {
        public UiInitializerContext()
        {
            Configs = new Dictionary<string, ViewModelConfig>(StringComparer.InvariantCultureIgnoreCase);
        }

        public IDictionary<string, ViewModelConfig> Configs { get; }
    }

    public static class InitializerExtensions
    {
        public static IDictionary<string, ViewModelConfig> GetVmConfigs(this IInitializerContext context)
        {
            return context.GetChildContext<UiInitializerContext>().Configs;
        }

        private static ViewModelConfig GetVmConfig(this IInitializerContext context, string mnemonic)
        {
            return context.GetChildContext<UiInitializerContext>().Configs[mnemonic];
        }

        public static IEnumerable<ViewModelConfig> GetAllVmConfigs(this IProcessorContext context)
        {
            return context.GetChildContext<UiInitializerContext>().Configs.Values;
        }

        public static ViewModelConfigBuilder<T> CreateVmConfig<T>(this IInitializerContext context) where T : class
        {
            return context.CreateVmConfig<T>(typeof(T).Name);
        }

        public static ViewModelConfigBuilder<T> CreateVmConfigOnBase<TBase, T>(this IInitializerContext context) where T : class, TBase
        {
            return context.CreateVmConfig<T>(baseMnemonic: typeof(TBase).Name, createMnemonic: typeof(T).Name);
        }

        public static ViewModelConfigBuilder<T> CreateVmConfigOnBase<TBase, T>(this IInitializerContext context, string mnemonic) where T : class, TBase
        {
            return context.CreateVmConfig<T>(baseMnemonic: typeof(TBase).Name, createMnemonic: mnemonic);
        }

        public static ViewModelConfigBuilder<T> CreateVmConfigOnBase<T>(this IInitializerContext context, string baseMnemonic) where T : class
        {
            return context.CreateVmConfig<T>(baseMnemonic: baseMnemonic, createMnemonic: typeof(T).Name);
        }

        public static ViewModelConfigBuilder<T> CreateVmConfigOnBase<T>(this IInitializerContext context, string baseMnemonic, string createMnemonic) where T : class
        {
            return context.CreateVmConfig<T>(baseMnemonic: baseMnemonic, createMnemonic: createMnemonic);
        }

        public static ViewModelConfigBuilder<T> ModifyVmConfig<T>(this IInitializerContext context) where T : class
        {
            return context.ModifyVmConfig<T>(typeof(T).Name);
        }

        public static ViewModelConfigBuilder<T> ModifyVmConfig<T>(this IInitializerContext context, string mnemonic) where T : class
        {
            var config = context.GetVmConfig(mnemonic);

            if (config.TypeEntity != typeof(T))
                throw new InvalidOperationException($"bad type for {mnemonic}");

            return new ViewModelConfigBuilder<T>(config, context);
        }

        public static ViewModelConfig GetVmConfig<T>(this IInitializerContext context, string mnemonic = null)
            where T : class
        {
            if (string.IsNullOrEmpty(mnemonic))
            {
                return context.GetVmConfig<T>();
            }

            var config = context.GetVmConfig(mnemonic);

            if (config.TypeEntity != typeof(T))
                throw new InvalidOperationException($"bad type for {mnemonic}");

            return config;
        }

        public static ViewModelConfig GetVmConfig<T>(this IInitializerContext context) where T : class
        {
            return context.GetVmConfig(typeof(T).Name);
        }

        public static ViewModelConfigBuilder<T> CreateVmConfig<T>(this IInitializerContext context, string mnemonic) where T : class
        {
            return context.CreateVmConfig<T>(baseMnemonic: null, createMnemonic: mnemonic);
        }

        private static ViewModelConfigBuilder<T> CreateVmConfig<T>(this IInitializerContext context, string baseMnemonic, string createMnemonic) where T : class
        {
            var config = ViewModelConfigFactory.CreateDefault(typeof(T), context.GetChildContext<IServiceLocator>());

            var configs = context.GetVmConfigs();

            if (baseMnemonic != null)
            {
                if (!configs.ContainsKey(baseMnemonic))
                    throw new InvalidOperationException($"basic config [{baseMnemonic}] not found");

                var original_config = configs[baseMnemonic];
              
                var baseConfig = original_config.Copy<T>();

                //baseConfig.DetailView.Editors.AddRange(
                //    config.DetailView.Editors.Where(
                //        x => baseConfig.DetailView.Editors.All(c => c.PropertyName != x.PropertyName)));

                //baseConfig.ListView.Columns.AddRange(
                //    config.ListView.Columns.Where(
                //        x => baseConfig.ListView.Columns.All(c => c.PropertyName != x.PropertyName)));

                if (original_config.Preview.CustomSelect == null)
                {
                    baseConfig.Preview.Fields.AddRange(
                        config.Preview.Fields.Where(
                            x => baseConfig.Preview.Fields.All(c => c.PropertyName != x.PropertyName)));
                }

                config = baseConfig;

            }

            config.Mnemonic = createMnemonic;

            configs.Add(createMnemonic, config);

            return new ViewModelConfigBuilder<T>(config, context);

        }
    }
}
