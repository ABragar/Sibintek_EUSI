using System.Collections.Generic;

namespace WebUI.Models.Print
{
    public class DataSourceModel
    {
        public string Name { get; set; }
        public IEnumerable<DataSourceModel> Childrens { get; set; }
    }
}