using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base.Notification.Service.Abstract;
using WebUI.Helpers;
using Base.Service.Crud;

namespace WebUI.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(IBaseControllerServiceFacade serviceFacade, INotificationService notificationService) : base(serviceFacade)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult> MarkAsRead(IEnumerable<int> ids)
        {
            try
            {
                await _notificationService.MarkAsRead(ids);

                return new JsonNetResult(new { });
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { error = e.Message });
            }
        }

        /// <summary>
        /// Отправка уведомлений
        /// </summary>
        /// <param name="stringIds">Строка вида "1,2,3" или "[1,2,3]" содержащая перечисление ID получателей.</param>
        /// <param name="json">JSON-строка вида {mnemonic: value1, id: value2}, где value1 - мнемоника передаваемого объекта, value2 - ID передаваемого объекта.</param>
        /// <param name="title">Заголовок оповещения.</param>
        /// <param name="descr">Описание оповещения.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult CreateNotification (string stringIds, string json, string title, string descr)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    int[] userIds = stringIds.Replace("[", "").Replace("]", "").Split(',').Select(int.Parse).ToArray();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (string.IsNullOrEmpty(obj["mnemonic"]) || string.IsNullOrEmpty(obj["id"]))
                        throw new Exception("Mnemonic or id in null");

                    var serv = GetService<IBaseObjectCrudService>(obj["mnemonic"]);
                    var baseObject = serv.Get(uofw, int.Parse(obj["id"]));
                    var linkedObj = GetLinkedObj(baseObject);
                    linkedObj.Mnemonic = linkedObj.Mnemonic ?? obj["mnemonic"];
                    _notificationService.CreateNotification(uofw, userIds, linkedObj, title, descr);
                    return new JsonNetResult(new { });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { error = e.Message });
            }
        }

        private Base.Entities.Complex.LinkBaseObject GetLinkedObj(Base.BaseObject obj)
        {
            var ret = new Base.Entities.Complex.LinkBaseObject(obj);
            return ret;
        }
    }
}