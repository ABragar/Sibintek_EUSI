using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Multimedia.Entities;
using Base.Service;

namespace Base.Multimedia.Service
{
    public class MultimediaObjectService : BaseObjectService<MultimediaObject>, IMultimediaObjectService
    {
        public MultimediaObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<MultimediaObject> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<MultimediaObject> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveManyToMany(x => x.SourceFiles);
        }
    }
}
