using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Social.Entities.Components;
using Base.Social.Service.Abstract.Components;

namespace Base.Social.Service.Concrete.Components
{
    public class CommentsService:BaseObjectService<Сomments>,ICommentsService
    {
        public CommentsService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}
