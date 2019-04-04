using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service.Crud;
using Base.Utils.Common;
using CorpProp.Entities.Document;
using CorpProp.Services.Document;
using WebUI.Extensions;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class CardFolderController: HCategoryController
    {
        private readonly IFileCardService _cardService;

        public CardFolderController(IBaseControllerServiceFacade baseServiceFacade, IFileCardService cardService) : base(baseServiceFacade)
        {
            _cardService = cardService;
        }
        public override JsonNetResult Get(string mnemonic, int id)
        {
            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var serv = this.GetService<ICategoryCrudService>(mnemonic);

                    var config = this.GetViewModelConfig(mnemonic);

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);

                    var category = serv.Get(uofw, id) as HCategory;

                    if (category == null) return new JsonNetResult(null);

                    return new JsonNetResult(
                        new
                        {
                            id = category.ID,
                            Title = pifo.GetValue(category),
                            InnerItemsCount = _cardService.GetAll(uofw)?.Count(card => card.CategoryID == category.ID) ?? 0,
                            (category as ITreeNodeImage)?.Image,
                            (category as ITreeNodeIcon)?.Icon,
                            hasChildren = serv.GetAll(uofw).Where($"it.ParentID = {category.ID}").Any(),
                            isRoot = category.IsRoot
                        });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(
                    new
                    {
                        error = $"Ошибка: {e.Message}"
                    }
                );
            }
        }

        public JsonNetResult GetFileCardsInFolderCount(
            int id,
            string mnemonic)
        {
            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var serv = this.GetService<ICategoryCrudService>(mnemonic);

                    var config = this.GetViewModelConfig(mnemonic);

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);

                    var category = serv.Get(uofw, id) as HCategory;

                    if (category != null)
                    {
                        var result = new JsonNetResult(
                                                       new
                                                       {
                                                           FileCardsInFolderCount = _cardService.GetAll(uofw)?.Count(card => card.CategoryID == category.ID) ?? 0
                                                       });
                        return result;
                    }
                    else
                    {
                        return new JsonNetResult(null);
                    }
                }
            }
            catch 
            {
                return new JsonNetResult(new object[] { });
            }
        }

        [WebApi.Attributes.NoCache]
        public async Task<JsonNetResult> TreeView_Read(int? id, string mnemonic, string searchStr = null, string filter = null)
        {
            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var config = this.GetViewModelConfig(mnemonic);

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);

                    var serv = this.GetService<ICategoryCrudService>(mnemonic);

                    IQueryable query;

                    if (!string.IsNullOrEmpty(searchStr))
                    {
                        query = serv.GetAll(uofw).FullTextSearch(searchStr, this.CacheWrapper).Take(500);
                    }
                    else
                    {
                        query = id == null ? serv.GetRoots(uofw) : serv.GetChildren(uofw, (int)id);
                    }


                    if (!string.IsNullOrEmpty(filter))
                    {
                        query = query.Filter(this, uofw, config, filter);
                    }


                    var list = (await query.ToListAsync()).Cast<HCategory>();

                    var parents = new Dictionary<int, int>();

                    if (string.IsNullOrEmpty(searchStr))
                    {
                        var ids = list.Select(x => x.ID).ToArray();

                        parents =
                            serv.GetAll(uofw)
                                .Where("it.ParentID != null")
                                .Select("it.ParentID").Cast<int>()
                                .Where(x => ids.Contains(x))
                                .Distinct().ToDictionary(x => x);
                    }

                    var res = list.Select(
                                a =>
                                    new
                                    {
                                        id = a.ID,
                                        Title = pifo.GetValue(a),
                                        InnerItemsCount = _cardService.GetAll(uofw)?.Count(card => card.CategoryID == a.ID) ?? 0,
                                        (a as ITreeNodeImage)?.Image,
                                        (a as ITreeNodeIcon)?.Icon,
                                        hasChildren = parents.ContainsKey(a.ID),
                                        isRoot = a.IsRoot
                                    });



                    return new JsonNetResult(res);

                }
            }
            catch (Exception exception)
            {
                return new JsonNetResult(new object[] { });
            }
        }


    }

    
}