namespace Data.BaseImport.Projections
{
    public class MenuElementImportProjection
    {
        public string ID { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Mnemonic { get; set; }
        public string Url { get; set; }
    }
}
