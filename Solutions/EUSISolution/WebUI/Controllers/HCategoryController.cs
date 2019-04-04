using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Extensions;
using Base.Service.Crud;
using Base.Utils.Common;
using WebUI.Extensions;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class HCategoryController : BaseController
    {
        public HCategoryController(IBaseControllerServiceFacade baseServiceFacade) : base(baseServiceFacade) { }

        public virtual JsonNetResult Get(string mnemonic, int id)
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
              
        public async Task<JsonNetResult> GetChildren(int id, string mnemonic)
        {
            using (var uofw = this.CreateUnitOfWork())
            {
                var serv = this.GetService<ICategoryCrudService>(mnemonic);
                var list = await serv.GetChildren(uofw, id).ToListAsync();

                return new JsonNetResult(list);
            }
        }

        [HttpPost]
        public ActionResult ChangePosition(string mnemonic, int id, int? parentID, int? posChangeID, string typePosChange)
        {
            var serv = this.GetService<ICategoryCrudService>(mnemonic);
            try
            {
                using (var uofw = this.CreateTransactionUnitOfWork())
                {
                    var obj = serv.Get(uofw, id) as HCategory;

                    if (obj == null)
                        return new JsonNetResult(new
                        {
                            error = "Ошибка переноса раздела: раздел не найден"
                        });

                    serv.ChangePosition(uofw, obj, posChangeID, typePosChange);

                    uofw.Commit();

                    return new JsonNetResult(new
                    {
                        message = "Раздел успешно перенесен!"
                    });
                }

            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = $"Ошибка переноса раздела: {e.Message}"
                });
            }
        }
    }
}
