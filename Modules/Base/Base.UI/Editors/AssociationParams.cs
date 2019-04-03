namespace Base.UI
{
    public class AssociationParams
    {
        public string Mnemonic { get; set; }
        public int Id { get; set; }
        public string SysName { get; set; }
        public bool Success => !string.IsNullOrEmpty(Mnemonic) && !string.IsNullOrEmpty(SysName) && Id != 0;
        public string Oid { get; set; }
        public string Date { get; set; }
        public bool SelectionDialog { get; set; }
    }
}