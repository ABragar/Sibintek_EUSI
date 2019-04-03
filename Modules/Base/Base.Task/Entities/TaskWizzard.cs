using System.Collections.Generic;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Security;
using Base.UI.Wizard;

namespace Base.Task.Entities
{
    public class TaskWizzard : DecoratedWizardObject<Task>
    {
        public TaskWizzard()
        {
            Period = new Period();
        }

        //[DetailView(Name = "Категория", Required = true)]
        //public BaseTaskCategory Category
        //{
        //    get { return (BaseTaskCategory)DecoratedObject.GetPropertyValue(x => x.BaseTaskCategory); }
        //    set { DecoratedObject.SetPropertyValue(x => x.BaseTaskCategory, value); }
        //}



        [SystemProperty, DetailView(Visible = false)]
        public int CategoryID { get; set; }


        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [ListView(Order = 4)]
        [DetailView(Name = "Период", Required = true)]
        public Period Period { get; set; }

        public override Task GetObject()
        {
            return new Task()
            {
                CategoryID = CategoryID,
                Name = Title,
                Period = Period,
                Description = Description,
                Files = Files,
                AssignedTo = AssignedTo,
                AssignedFrom = AssignedFrom
            };
        }
        [DetailView(Name = "Описание", Required = true)]
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        public string Description { get; set; }


        [DetailView("Файлы")]
        public virtual ICollection<BaseTaskFile> Files { get; set; }


        [DetailView(Name = "Исполнитель")]
        public virtual User AssignedTo { get; set; }


        [DetailView(Name = "Автор")]
        public virtual User AssignedFrom { get; set; }

    }
}