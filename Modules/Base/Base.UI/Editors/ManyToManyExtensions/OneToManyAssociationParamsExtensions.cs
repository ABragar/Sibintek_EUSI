using System.Linq;
using System.Linq.Dynamic;
using Base.DAL;
using Base.Service.Crud;


namespace Base.UI.Editors.ManyToManyExtensions
{
    public static class ManyToManyAssociationParamsExtensions
    {
        public static IQueryable AddFilter<T>(this AssociationParams associationParams, IQueryable<T> q, IUnitOfWork unitOfWork, IViewModelConfigService viewModelConfigService) where T : IBaseObject
        {            
            if (associationParams.SelectionDialog || !associationParams.Success) return q;
            var config = viewModelConfigService.Get(associationParams.Mnemonic);
            var editor = (ManyToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == associationParams.SysName);

            var junctionConfig = viewModelConfigService.Get(editor.ManyToManyType);

            var serv = junctionConfig.GetService<IBaseObjectCrudService>();

            if (editor.AssociationType == ManyToManyAssociationType.Left)
            {
                var junction = serv.GetAll(unitOfWork).Where($"ObjLeftId == @0", associationParams.Id);
                return q.Join(junction, "ID", "ObjRigthId", "outer");
            }
            else
            {
                var junction = serv.GetAll(unitOfWork).Where($"ObjRigthId == @0", associationParams.Id);
                return q.Join(junction, "ID", "ObjLeftId", "outer");
            }
        }
    }
}