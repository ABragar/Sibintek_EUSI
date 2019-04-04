namespace WebUI.Models.ExportImport
{
    public class ImportItemModel
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public bool Enabled { get; set; }
        public FormImport FormImport { get; set; }
    }

    public enum FormImport
    {
        Roles,
        PresetMenu
    }
}