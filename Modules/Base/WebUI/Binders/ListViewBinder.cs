using System;
using System.Web.Mvc;
using Base.UI.ViewModal;

namespace WebUI.Binders
{
    public class ListViewBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var entityType = typeof(ListView);

            var val = bindingContext.ValueProvider.GetValue("model.ListView.Type");

            if (val != null)
            {
                var lvType =
                    (ListViewType)
                        Enum.Parse(typeof(ListViewType), val.AttemptedValue);

                switch (lvType)
                {
                    case ListViewType.GridCategorizedItem:
                        entityType = typeof(ListViewCategorizedItem);
                        break;

                    case ListViewType.Tree:
                        entityType = typeof(TreeView);
                        break;

                    case ListViewType.Scheduler:
                        entityType = typeof(SchedulerView);
                        break;

                    case ListViewType.TreeListView:
                        entityType = typeof(TreeListView);
                        break;

                    case ListViewType.Gantt:
                        entityType = typeof(GanttView);
                        break;
                    case ListViewType.Pivot:
                        entityType = typeof(PivotListView);
                        break;
                }
            }

            object obj = Activator.CreateInstance(entityType);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => obj, entityType);

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}