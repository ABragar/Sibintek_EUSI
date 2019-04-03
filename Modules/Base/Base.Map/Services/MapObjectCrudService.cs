using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Map.Helpers;
using Base.Map.MapObjects;
using Base.Service.Crud;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Map.Services
{
    public class MapObjectCrudService : IMapObjectCrudService
    {
        private static readonly IReadOnlyCollection<GeoObject> EmptyResult = new GeoObject[0];


        private readonly IUnitOfWorkFactory _unit_of_work_factory;
        private readonly IViewModelConfigService _view_model_config_service;

        public MapObjectCrudService(IUnitOfWorkFactory unit_of_work_factory, IViewModelConfigService view_model_config_service)
        {
            this._unit_of_work_factory = unit_of_work_factory;
            this._view_model_config_service = view_model_config_service;
        }



        public IReadOnlyCollection<GeoObject> DeleteGeoObjects(string mnemonic, IReadOnlyCollection<GeoObject> geoObjects)
        {
            if (geoObjects == null || !geoObjects.Any())
            {
                return EmptyResult;
            }

            var service = GetCrudService(mnemonic);
            var deletedObjects = new List<IGeoObject>();

            using (var uofw = _unit_of_work_factory.CreateTransaction())
            {
                foreach (var geoObject in geoObjects)
                {
                    var objectToDelete = service.Get(uofw, (int)geoObject.ID) as IGeoObject;

                    if (objectToDelete == null)
                    {
                        continue;
                    }

                    service.Delete(uofw, objectToDelete as BaseObject);
                    deletedObjects.Add(objectToDelete);
                }

                uofw.Commit();
            }

            return SelectGeoObjects(deletedObjects);
        }

        public IReadOnlyCollection<GeoObject> UpdateGeoObjects(string mnemonic, IReadOnlyCollection<GeoObject> geoObjects)
        {
            if (geoObjects == null || !geoObjects.Any())
            {
                return EmptyResult;
            }

            var service = GetCrudService(mnemonic);

            var savedObjects = new List<IGeoObject>();

            using (var uofw = _unit_of_work_factory.CreateTransaction())
            {
                foreach (var geoObject in geoObjects)
                {
                    var objectToUpdate = service.Get(uofw, (int)geoObject.ID) as IGeoObject;

                    if (objectToUpdate == null)
                    {
                        continue;
                    }

                    objectToUpdate.Location.Disposition = geoObject.Geometry;
                    service.Update(uofw, objectToUpdate as BaseObject);
                    savedObjects.Add(objectToUpdate);
                }

                uofw.Commit();
            }

            return SelectGeoObjects(savedObjects);
        }


        private static IReadOnlyCollection<GeoObject> SelectGeoObjects(IEnumerable<IGeoObject> models)
        {
            return models.Select(model => new GeoObject
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                Geometry = model.Location.Disposition
            }).ToList();
        }

        private IBaseObjectCrudService GetCrudService(string mnemonic)
        {
            return ViewModelConfigHelper.GetGeoObjectCrudService(_view_model_config_service, mnemonic);
        }
        private ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return ViewModelConfigHelper.GetViewModelConfig(_view_model_config_service, mnemonic);
        }


    }
}