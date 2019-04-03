using System.Linq;
using Base.DAL;

namespace Base.UI.Editors.OneToManyExtensions
{
    public static class OneToManyAssociationParamsExtensions
    {
        public static IQueryable<T> AddFilter<T>(this AssociationParams associationParams, IQueryable<T> q, IUnitOfWork unitOfWork, IViewModelConfigService viewModelConfigService, System.Guid oid) where T : IBaseObject
        {
            if (associationParams.SelectionDialog || !associationParams.Success) return q;
            var config = viewModelConfigService.Get(associationParams.Mnemonic);

            var editor = (OneToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == associationParams.SysName);

            return editor.Filter == null ? q : (IQueryable<T>)editor.Filter(unitOfWork, q, associationParams.Id, oid);
        }

        public static void AddAssociation<T>(this AssociationParams parameters, T entity, IUnitOfWork unitOfWork, IViewModelConfigService viewModelConfigService)
        {
            if(!parameters.Success) return;

            var config = viewModelConfigService.Get(parameters.Mnemonic);

            var editor = (OneToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == parameters.SysName);

            editor.Create?.Invoke(unitOfWork, entity, parameters.Id);
        }

        public static void DeleteAssociation<T>(this AssociationParams parameters, T entity, IUnitOfWork unitOfWork, IViewModelConfigService viewModelConfigService)
        {
            if (!parameters.Success) return;

            var config = viewModelConfigService.Get(parameters.Mnemonic);

            var editor = (OneToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == parameters.SysName);

            editor.Delete?.Invoke(unitOfWork, entity, parameters.Id);
        }
    }
}