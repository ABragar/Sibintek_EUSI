using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Contact.Entities;
using Base.Contact.Service.Concrete;
using Base.Event.Entities;
using Base.Event.Service;
using Base.Service.Crud;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Event;

namespace WebUI.Controllers
{
    public class EventController : BaseController
    {
        private readonly IContactService _contactService;
        private readonly ICallService _callService;


        public EventController(IBaseControllerServiceFacade serviceFacade, IContactService contactService, ICallService callService) : base(serviceFacade)
        {
            _contactService = contactService;
            _callService = callService;
        }

        public ActionResult NewCall(bool outcall)
        {
            return PartialView("NewCall", outcall);
        }

        public JsonNetResult CreateCall(string phone, string code = "+7", bool isOutCall = false)
        {
            using (var uow = CreateUnitOfWork())
            {
                var call = _callService.GetNewCall(uow, phone, code, isOutCall);

                return new JsonNetResult(call);
            }
        }


        public ActionResult CheckPhoneNumber(string number, string code)
        {
            using (var uow = CreateUnitOfWork())
            {
                var contact = _contactService.GetcontactByPhone(uow, number, code);

                return new JsonNetResult(contact);
            }
        }

        public ActionResult CreateCallContact(string phone, string code = "+7")
        {
            var model = new CreateCallModel { Phone = phone, Code = code };

            return PartialView("_CallContact", model);
        }


        [HttpPost]
        public ActionResult CreateContact(string contactName, string phone, string code = "+7")
        {
            using (var uow = CreateUnitOfWork())
            {
                try
                {
                    var contact = _contactService.CreateContact(uow, SourceContact.Call, contactName, phone, null, code);

                    return new JsonNetResult(contact);
                }
                catch (Exception error)
                {
                    return new JsonNetResult(new { error = error.Message });
                }
            }
        }

        [HttpPost]
        public ActionResult AddPhoneToContact(int id, string phone, string code = "+7")
        {
            using (var uow = CreateUnitOfWork())
            {
                var contact = _contactService.AddPhone(uow, id, phone, code);
                return new JsonNetResult(contact);
            }
        }
    }
}