using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Base.Contact.Entities;
using Base.Contact.Service.Concrete;
using Base.Mail.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebUI.Helpers;
using System.Threading.Tasks;
using Base.MailAdmin.Entities;
using Base.MailAdmin.Services;
using Microsoft.AspNet.SignalR;


namespace WebUI.Controllers
{
    public class MailAdminController : BaseController
    {
        private readonly IMailAccountService _mailAccountService;

        public MailAdminController(IBaseControllerServiceFacade serviceFacade, IMailAccountService mailAccountService)
            : base(serviceFacade)
        {
            _mailAccountService = mailAccountService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAccountByUserId(int userId)
        {
            try
            {
                return new JsonNetResult(await _mailAccountService.GetByIserId(userId));
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { error = e.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveAccount(MailAccount model)
        {
            try
            {
                return new JsonNetResult(await _mailAccountService.Save(model));
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { error = e.Message });
            }
        }
    }
}