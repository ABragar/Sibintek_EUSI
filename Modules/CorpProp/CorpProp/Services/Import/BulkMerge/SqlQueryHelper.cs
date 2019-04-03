namespace CorpProp.Services.Import.BulkMerge
{
    public static class SqlQueryHelper
    {
        public static string AddCommaAhead(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            return $",{source}";
        }
    }
}
