using System;
using System.Collections.Generic;
using System.Linq;
using Base.Enums;
using Base.UI.ViewModal;

namespace Base.UI.Extensions
{
    public static class ViewModelConfigExtensions
    {
        public static ViewModelConfigDto ToDto(this ViewModelConfig config, Func<string, ViewModelConfig> getConfig)
        {
            var res = new ViewModelConfigDto
            {
                Mnemonic = config.Mnemonic,
                IsReadOnly = config.IsReadOnly || !Ambient.AppContext.SecurityUser.IsPermission(config.TypeEntity, TypePermission.Write),
                TypeEntity = config.TypeEntity.GetTypeName(),
                Icon = config.Icon,
                LookupProperty = config.LookupProperty,
                Title = config.DetailView.Title ?? config.Title,
                SystemProperties = config.DetailView.Props.Where(x => x.IsSystemPropery).Select(x => x.PropertyName).ToArray(),
                ListView = new ListViewDto()
                {
                    Title = config.ListView.Title ?? config.Title,
                    Columns = config.ListView.Columns.Select(c => new ColumnDto()
                    {
                        PropertyName = c.PropertyName,
                        Hidden = c.Hidden,
                        DataType = c.PropertyDataTypeName,
                        Type = c.ColumnType.IsEnum || c.ColumnType.IsBaseObject() ? c.ColumnType.GetTypeName() : null
                    })
                },
                DetailView = new DetailViewDto()
                {
                    Title = config.DetailView.Title ?? config.Title,
                    Width = config.DetailView.Width,
                    Height = config.DetailView.Height,
                    IsMaximaze = config.DetailView.IsMaximaze,
                    WizardName = config.DetailView.WizardName
                },
                Ext = new Dictionary<string, object>() { },
                Preview = config.Preview.Enable
            };

            var listViewCategorizedItem = config.ListView as ListViewCategorizedItem;

            if (listViewCategorizedItem != null)
            {
                res.Ext.Add("MnemonicCategory", listViewCategorizedItem?.MnemonicCategory);
            }

            if (config.Relations.Any())
            {
                //NOTE: getConfig(mnemonic) - есть мнемоника
                //      getConfig(mnemonic).Relations.Count == 1 - нет наследников (в Relations только его мнемоника)
                //      mnemonic != config.Mnemonic - не являеться текущей мнемоникой
                res.Ext.Add("Relations", config.Relations
                   .Where(mnemonic => getConfig(mnemonic) != null)
                   .ToDictionary(mnemonic => mnemonic, mnemonic =>
                   {
                        var rconfig = getConfig(mnemonic);

                        return new
                        {
                            rconfig.Mnemonic,
                            rconfig.Icon,
                            rconfig.IsReadOnly,
                            Title = rconfig.DetailView.Title ?? rconfig.Title
                        };
                   }));
            }

            return res;
        }
    }
}