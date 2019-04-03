using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.DAL;
using Base.Service.Log;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using Data.BaseImport.Exceptions;
using Data.BaseImport.Projections;
using Data.BaseImport.Services.Abstract;
using LinqToExcel;

namespace Data.BaseImport.Services.Concrete
{
    public class PresetMenuBaseImportService : BaseImportService, IPresetMenuBaseImportService
    {
        private readonly IPresetRegistorService _presetRegistorService;

        private bool _fullImport = true;
        private Dictionary<string, MenuElement> _registryMenuItems;
        private MenuPreset _preset;
        public PresetMenuBaseImportService(IPresetRegistorService presetRegistorService, IUnitOfWorkFactory unitOfWorkFactory,
            ILogService logService) : base(unitOfWorkFactory, logService)
        {
            _presetRegistorService = presetRegistorService;
        }

        public override void Import(string pathFile)
        {
            string[] worksheetNames;
            using (ExcelQueryFactory excelFile = new ExcelQueryFactory(pathFile))
            {
                worksheetNames = excelFile.GetWorksheetNames().ToArray();
            }

            foreach (string worksheetName in worksheetNames)
            {
                try
                {
                    _registryMenuItems = new Dictionary<string, MenuElement>();
                    _preset = new MenuPreset();
                    ExecuteProjection<MenuElementImportProjection>(AddMenuElement, pathFile, worksheetName);
                    using (var unitOfWork = _unitOfWorkFactory.Create())
                    {
                        _presetRegistorService.Create(unitOfWork, new PresetRegistor()
                        {
                            Title = worksheetName,
                            For = "Menu",
                            Type = typeof(MenuPreset).GetTypeName(),
                            Preset = _preset
                        });
                    }
                }
                catch (Exception e)
                {
                    _fullImport = false;
                    _logService.Log($"Ошибка при обработке листа '{worksheetName}': {e.Message}");
                }
            }

            if(!_fullImport)
                throw new FailImportException("Произошла одна или несколько ошибок при работе импорта");
        }

        private void AddMenuElement(MenuElementImportProjection projection)
        {
            try
            {
                if (_registryMenuItems.ContainsKey(projection.ID))
                    throw new Exception("В excel файле определено несколько одинаковых ID элементов меню");

                MenuElement menuItem = new MenuElement(projection.Title, projection.Mnemonic, projection.Icon) { URL = projection.Url };
                if (string.IsNullOrEmpty(projection.ParentId))
                    _preset.MenuElements.Add(menuItem);
                else
                    _registryMenuItems[projection.ParentId].Children.Add(menuItem);

                _registryMenuItems.Add(projection.ID, menuItem);
            }
            catch (Exception e)
            {
                _fullImport = false;
                throw new Exception(e.Message);
            }
        }
    }
}
