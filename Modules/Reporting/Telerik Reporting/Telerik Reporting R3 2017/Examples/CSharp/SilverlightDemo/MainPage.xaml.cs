namespace CSharp.SilverlightDemo
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using Telerik.Windows.Controls;

    public partial class MainPage : UserControl
    {
        static readonly string[] dictionaries = new[] 
        {
            "/CSharp.SilverlightDemo;component/Themes/{0}/System.Windows.xaml",
            "/CSharp.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.xaml",
            "/CSharp.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.Input.xaml",
            "/CSharp.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.Navigation.xaml",
            "/CSharp.SilverlightDemo;component/Themes/{0}/Telerik.ReportViewer.Silverlight.xaml" 
        };

        public MainPage()
        {
            InitializeComponent();
        }

        void ThemeSelector_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
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
            return Enum.Parse(enumeration, colorVariation, false);
        }

        static Color GetLayoutBackground(string themeName, string themeVariation, Panel grid)
        {
            switch (themeName)
            {
                case "VisualStudio2013":
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
                var entry = string.Format(dictionary, theme);
                var resourceInfo = App.GetResourceStream(new Uri(entry, UriKind.RelativeOrAbsolute));
                var resourceReader = new StreamReader(resourceInfo.Stream);
                var xaml = resourceReader.ReadToEnd();
                var resourceTheme = XamlReader.Load(xaml) as ResourceDictionary;

                mergedDictionaries.Add(resourceTheme);
            }
        }
    }
}
