namespace CorpProp.Services.Import.BulkMerge
{
    public class SqlColumnDefinition
    {
        public string ColumnName { get; set; }

        public string ColumnDefault { get; set; }

        public bool IsNullable { get; set; }

        public string DataType { get; set; }

        public int CharacterMaximumLenght { get; set; }
    }
}
