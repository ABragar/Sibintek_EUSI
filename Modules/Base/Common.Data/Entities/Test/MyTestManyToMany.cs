using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;

namespace Common.Data.Entities.Test
{
    public class MyTestManyToMany : BaseObject
    {
        [DetailView("Наименование")]
        [ListView]
        public string Title { get; set; }

        [DetailView("Int Some Value", Group = "[1]Grupp",TabName = "[1]ТАБ")]
        [ListView]
        public int SomeValue { get; set; }

        [DetailView("WithoutListView", Group = "[1]Grupp", TabName = "[1]ТАБ")]
        public string WithoutListView { get; set; }
        
        [DetailView("Test"), ManyToManyTable]
        public virtual ICollection<TestObject> TestObject { get; set; }
        

        [DetailView("Testprop1",Group = "[3]Grupp")]
        public string Testprop1 { get; set; }

        [DetailView("Testprop2", Group = "[3]Grupp")]
        public string Testprop2 { get; set; }

        [DetailView("Testprop3")]
        public string Testprop3 { get; set; }


        [DetailView("Testprop4", TabName = "[2]ТАБ")]
        public string Testprop4 { get; set; }

        [DetailView("Testprop5", Group = "[5]Grupp", TabName = "[2]ТАБ")]
        public string Testprop5 { get; set; }

        [DetailView("Testprop6", Group = "[5]Grupp", TabName = "[2]ТАБ")]
        public string Testprop6 { get; set; }
    }
}
