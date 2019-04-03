namespace CSharp.WpfDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Telerik.Windows.Controls;

    public partial class Window1 : Window
    {
        static readonly string[] dictionaries = new[]
            {
                "Themes/{0}/System.Windows.xaml",
                "Themes/{0}/Telerik.Windows.Controls.xaml",
                "Themes/{0}/Telerik.Windows.Controls.Input.xaml",
                "Themes/{0}/Telerik.Windows.Controls.Navigation.xaml",
                "Themes/{0}/Telerik.ReportViewer.Wpf.xaml"
            };

        public Window1()
        {
            InitializeComponent();
        }

        void ThemeSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItem = (RadComboBoxItem)((RadComboBox)sender).SelectedItem;
            var themeNameParts = ((string)selectedItem.Content).Split('_');
            var themeName = themeNameParts[0];
            var themeVariation = string.Empty;

            MergeResourceDictionaries(themeName);

            if (themeNameParts.Length > 1)
            {
                switch (themeName)
                {
                    case "VisualStudio2013":
                        VisualStudio2013Palette.LoadPreset((VisualStudio2013Palette.ColorVariation)GetPresetEnum(typeof(VisualStudio2013Palette.ColorVariation), themeNameParts[1]));
                        break;
                    case "Office2013":
                        Office2013Palette.LoadPreset((Office2013Palette.ColorVariation)GetPresetEnum(typeof(Office2013Palette.ColorVariation), themeNameParts[1]));
                        break;
                    case "Green":
                        GreenPalette.LoadPreset((GreenPalette.ColorVariation)GetPresetEnum(typeof(GreenPalette.ColorVariation), themeNameParts[1]));
                        break;
                }

                themeVariation = themeNameParts[1];
            }

            this.SetLayoutBackground(themeName, themeVariation);
        }

        void SetLayoutBackground(string themeName, string themeVariation)
        {
            var grid = this.LayoutRoot as Grid;
            if (null != grid)
            {
                grid.Background = new SolidColorBrush(GetLayoutBackground(themeName, themeVariation, grid));
            }
        }

        object GetPresetEnum(Type enumeration, string colorVariation)
        {
            return Enum.Parse(enumeration, colorVariation);
        }

        static Color GetLayoutBackground(string themeName, string themeVariation, Panel grid)
        {
            switch (themeName)
            {
                case "VisualStudio2013":
                case "Green":
                    if (themeVariation == "Dark")
                        return Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
                    else
                        return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                case "Office2013":
                    if (themeVariation == "White")
                        return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                    else
                        return Color.FromArgb(0xFF, 0xD6, 0xD6, 0xD6);
                case "ExpressionDark":
                    return Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
                case "OfficeBlack":
                    return Color.FromArgb(0xFF, 0xCA, 0xCA, 0xCA);
                case "OfficeBlue":
                    return Color.FromArgb(0xFF, 0xCD, 0xE5, 0xFE);
                case "OfficeSilver":
                    return Color.FromArgb(0xFF, 0xDC, 0xDD, 0xE7);
                default:
                    return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            }
        }

        static void MergeResourceDictionaries(string theme)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            foreach (var dictionary in dictionaries)
            {
                var uri = string.Format(dictionary, theme);
                mergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(uri, UriKind.RelativeOrAbsolute)
                });
            }
        }
    }
}
