using System;
using System.Linq;
using System.Web.Script.Serialization;
using Base.DAL;
using CorpProp.Entities.Request;
using CorpProp.Services.Response.Fasade;

namespace WebUI.Models.CorpProp.Response.Config
{

    public class ResponseGridConfigPropertyFabric
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResponseGridConfigPropertyFabric(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public ResponseGridConfigProperty Create(IRequestColumn column)
        {
            var result = new ResponseGridConfigProperty()
                         {
                             ID = column.ID,
                             ColumnConstrains = RequestColumnConstrain.CreateFrom(column),
                             Title = GetTitle(column),
                             Mnemonic = column.TypeData.Code,
                             PropertyName = GetPropertyName(column),
                             Type = ResponseTypeDataFacade.GetColumnType(column.TypeData)
                         };
            result.ColumnConstrains.HasColumnItems = ColumnItems(_unitOfWork, column);
            return result;
        }

        public ResponseGridConfigProperty Create(IRequestColumn column, string propertyName)
        {
            var result = new ResponseGridConfigProperty()
            {
                ID = column.ID,
                ColumnConstrains = RequestColumnConstrain.CreateFrom(column),
                Title = GetTitle(column),
                Mnemonic = column.TypeData.Code,
                PropertyName = propertyName,
                Type = ResponseTypeDataFacade.GetColumnType(column.TypeData)
            };

            return result;
        }


        private string GetTitle(IRequestColumn column)
        {
            return column.Name;
        }

        bool ColumnItems(IUnitOfWork unitOfWork, IRequestColumn column)
        {
            var firstRequestColumnItem = unitOfWork.GetRepository<RequestColumnItems>()?.Filter(items => items.RequestColumnID == column.ID).FirstOrDefault();
            return firstRequestColumnItem != null;
        }

        private string GetPropertyName(IRequestColumn column)
        {
            return RequestQuery.ColumnPropertyAliasNameMaker(column);
        }
    }

    public class ResponseGridConfigProperty
    {
        public RequestColumnConstrain ColumnConstrains { get; set; }

        public int ID { get; set; }

        public string PropertyName { get; set; }

        public bool Visible { get; set; } = true;

        public string Title { get; set; }

        [ScriptIgnore]
        public Type Type { get; set; }

        public string Mnemonic { get; set; }
    }
}