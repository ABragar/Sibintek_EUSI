using System;
using System.Collections.Generic;


namespace Base.DAL
{
    public interface IBulkRepository<T> : IRepository<T> where T : BaseObject
    {
//        void BulkInsertAll(DataTable table, Type type, Dictionary<string, string> colsNameMapping, ref int count);
//
//        void UpdateOlderDictObject(string tableName);
    }
}
