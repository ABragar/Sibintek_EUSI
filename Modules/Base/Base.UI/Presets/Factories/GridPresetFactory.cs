using System;

namespace Base.UI.Presets.Factories
{
    public class GridPresetFactory : DefaultPresetFactory<GridPreset>
    {
        private readonly IViewModelConfigService _viewModelConfigService;

        public GridPresetFactory(IViewModelConfigService viewModelConfigService)
        {
            _viewModelConfigService = viewModelConfigService;
        }

        public override GridPreset Create(string ownerName)
        {
            if (ownerName == null)
                throw new NullReferenceException(nameof(ownerName));

            var config = _viewModelConfigService.Get(ownerName);

            if (config == null)
                throw new NullReferenceException(nameof(ownerName));

            var columns = _viewModelConfigService.GetColumns(ownerName);

            var preset = base.Create(ownerName);

            preset.PageSize = config.ListView.DataSource.PageSize;
            preset.Groupable = config.ListView.DataSource.Groups.Groupable;

            columns.ForEach(col =>
            {
                var column = new ColumnPreset
                {
                    Name = col.PropertyName,
                    Title = col.Title,
                    Visible = col.Visible,
                    Width = col.Width,
                    SortOrder = col.SortOrder,
                    ID = preset.Columns.Count + 1,
                    OneLine = col.OneLine
                };

                preset.Columns.Add(column);
            });


            return preset;
        }
    }
}
