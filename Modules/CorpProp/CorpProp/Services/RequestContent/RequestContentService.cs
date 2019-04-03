using System;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Request;

namespace CorpProp.Services.RequestContent
{
    public class RequestContentService: BaseObjectService<Entities.Request.RequestContent>
    {
        public RequestContentService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override Entities.Request.RequestContent Create(IUnitOfWork unitOfWork, Entities.Request.RequestContent obj)
        {
            var createdRequestContent = base.Create(unitOfWork, obj);
            var responseStateTypeCode = typeof(ResponseRowState).Name;
            var typeData = unitOfWork.GetRepository<TypeData>().Find(type => type.Code == responseStateTypeCode);
            if (typeData == null)
                throw new NotSupportedException($"Не найден код {responseStateTypeCode} в справочнике {typeof(TypeData).Name}");
            RequestColumn column = new RequestColumn()
            {
                RequestContent = createdRequestContent,
                RequestContentID = createdRequestContent.ID,
                Name = "Статус строки ответа",
                TypeData = typeData,
                TypeDataID = typeData.ID,
            };
            unitOfWork.GetRepository<RequestColumn>().Create(column);
            unitOfWork.SaveChanges();
            return createdRequestContent;
        }
    }
}
