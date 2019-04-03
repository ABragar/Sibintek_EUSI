namespace Common.Data.BaseImport.Projections
{
    public class PermissionImportProjection
    {
        public string RoleId { get; set; }
        public string Mnemonic { get; set; }
        public string Read { get; set; }
        public string Write { get; set; }
        public string Create { get; set; }
        public string Delete { get; set; }
    }
}
