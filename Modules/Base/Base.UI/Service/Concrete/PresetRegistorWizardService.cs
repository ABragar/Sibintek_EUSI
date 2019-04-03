using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities;
using Base.Service;
using Base.UI.Presets;
using Base.UI.ViewModal;
using Base.UI.Wizard;

namespace Base.UI.Service
{
    public class PresetRegistorWizardService : BaseWizardService<PresetRegistorWizard, PresetRegistor>,
        IPresetRegistorWizardService
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IPresetService<MenuPreset> _menuPresetService;
        private readonly IPresetService<DashboardPreset> _dashboardPresetService;
        private readonly IPresetService<QuickAccessBarPreset> _quickaccessbarPresetService;


        // private readonly IPresetService<> _presetService;

        public PresetRegistorWizardService(IPresetRegistorService presetRegistorService, IAccessService accessService,
            IPresetService<MenuPreset> menuPresetService, IPresetService<DashboardPreset> dashboardPresetService,
            IPresetService<QuickAccessBarPreset> quickaccessbarPresetService,
            IViewModelConfigService viewModelConfigService) : base(presetRegistorService, accessService)
        {
            _menuPresetService = menuPresetService;
            _dashboardPresetService = dashboardPresetService;
            _quickaccessbarPresetService = quickaccessbarPresetService;
            _viewModelConfigService = viewModelConfigService;
        }


        public override void OnBeforeStart(IUnitOfWork unitOfWork, ViewModelConfig config, PresetRegistorWizard obj)
        {
            string menuPresetMnemonic = nameof(MenuPreset);
            string quickAccessBarPresetMnemonic = nameof(QuickAccessBarPreset);
            string dashboardPresetMnemonic = nameof(DashboardPreset);
            var presetTypes = _viewModelConfigService.GetAll().Where(x => x.Mnemonic == menuPresetMnemonic ||
                                                                          x.Mnemonic == quickAccessBarPresetMnemonic ||
                                                                          x.Mnemonic == dashboardPresetMnemonic);
            if (!obj.PresetTypes.Any())
            {
                int count = 1;
                foreach (var presetType in presetTypes)
                {
                    obj.PresetTypes.Add(new DescriptionLookupVm()
                    {
                        ID = count.ToString(),
                        Title = presetType.Title,
                        Description = presetType.TypeEntity.GetTypeName()
                    });
                    count++;
                }
            }
        }

        public override Task<PresetRegistor> CompleteAsync(IUnitOfWork unitOfWork, PresetRegistorWizard obj)
        {
            var @switch = new Dictionary<string, Action>
            {
                {
                    typeof(MenuPreset).GetTypeName(), () =>
                    {
                        obj.For = "Menu";
                        obj.Preset = _menuPresetService.GetDefaultPreset(obj.For);
                    }
                },
                {
                    typeof(DashboardPreset).GetTypeName(), () =>
                    {
                        obj.For = "Global";
                        obj.Preset = _dashboardPresetService.GetDefaultPreset(obj.For);
                    }
                },
                {
                    typeof(QuickAccessBarPreset).GetTypeName(), () =>
                    {
                        obj.For = "QuickAccessBar";
                        obj.Preset = _quickaccessbarPresetService.GetDefaultPreset(obj.For);
                    }
                },
            };

            if (obj.PresetType != null)
            {
                var type = obj.PresetType.Description;
                if (obj.Type != type || obj.Preset == null || obj.Preset.GetType().GetTypeName() != type)
                {
                    obj.Type = obj.PresetType.Description;

                    @switch[obj.Type]();

                }
            }
            return base.CompleteAsync(unitOfWork, obj);
        }


        //public override Task OnAfterStepChangeAsync(IUnitOfWork unitOfWork,
        //    ViewModelConfig config,
        //    string prevStepName,
        //    PresetRegistorWizard obj)
        //{
        //    var @switch = new Dictionary<string, Action>
        //    {
        //        {
        //            typeof(MenuPreset).GetTypeName(), () =>
        //            {
        //                obj.For = "Menu";
        //                obj.Preset = _menuPresetService.GetDefaultPreset(obj.For);
        //            }
        //        },
        //        {
        //            typeof(DashboardPreset).GetTypeName(), () =>
        //            {
        //                obj.For = "Global";
        //                obj.Preset = new DashboardPreset();
        //            }
        //        },
        //        {
        //            typeof(QuickAccessBarPreset).GetTypeName(), () =>
        //            {
        //                obj.For = "QuickAccessBar";
        //                obj.Preset = _quickaccessbarPresetService.GetDefaultPreset(obj.For);
        //            }
        //        },
        //    };

        //    if (obj.PresetType != null)
        //    {
        //        var type = obj.PresetType.Description;
        //        if (obj.Type != type || obj.Preset == null || obj.Preset.GetType().GetTypeName()!= type)
        //        {
        //            obj.Type = obj.PresetType.Description;

        //            @switch[obj.Type]();

        //        }

        //    }

        //    return base.OnAfterStepChangeAsync(unitOfWork, config, prevStepName, obj);
        //}

    }
}