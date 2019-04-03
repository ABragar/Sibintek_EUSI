using Base.DAL;
using Base.DAL.EF;
using Base.Macros.Entities.Rules;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Filter;
using Base.UI.RegisterMnemonics.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class UiConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<PresetRegistor>()
                .Entity<UiEnum>()
                .Entity<UiEnumValue>()
                .Entity<MnemonicItem>()
                .Entity<SystemMnemonicItem>()
                .Entity<ClientMnemonicItem>()
                .Entity<MnemonicEx>()
                .Entity<TitleEx>()
                .Entity<ListViewFilterEx>()
                .Entity<DeatilViewEx>(e => e.Save(s => s.SaveOneToMany(o => o.Editors)))
                .Entity<EditorEx>()
                .Entity<ListViewEx>(e => e.Save(s => s.SaveOneToMany(o => o.Columns)))
                .Entity<ColumnEx>()

                //FILTER
                .Entity<MnemonicFilter>()
                .Entity<GlobalMnemonicFilter>()
                .Entity<UsersMnemonicFilter>()

                //DetailView
                .Entity<DvSetting>()
                .Entity<DvSettingForMnemonic>()
                .Entity<DvSettingForType>()
                .Entity<EditorVmSetting>(e => e.Save(s => s.SaveOneToMany(x => x.VisibleRules)))
                //.Entity<FieldRoleVisible>()
                //.Entity<FieldRoleHidden>()
                //.Entity<FieldRoleEnable>()
                //.Entity<FieldRoleReadOnly>()
                .Entity<RuleVisible>()
                .Entity<RuleHidden>()
                .Entity<RuleEnable>()
                .Entity<ReadOnlyEnable>()
                .Entity<BRuleEnable>()
                .Entity<RuleForType>()
                .Entity<BRuleHidden>()
                .Entity<BRuleVisible>()
                .Entity<BRuleReadOnly>()
                .Entity<RuleItem>();
        }
    }
}