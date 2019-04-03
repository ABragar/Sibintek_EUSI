using Base;
using Base.Content;
using Base.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.MultiEdit;

namespace WebUI.Controllers
{
    public class MultiEditController : BaseController
    {
        public MultiEditController(IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {
        }
        
        public ActionResult Toolbar()
        {
            return PartialView();
        }

        public ActionResult GetPartialView(string mnemonic, string property)
        {
            var commonEditorViewModel = UiFasade.GetCommonEditor(mnemonic);

            var editors = commonEditorViewModel.Editors
                .Where(x => x.Visible && !x.IsReadOnly && x.PropertyName != null && x.PropertyType != typeof(FileData) && x.PropertyType != typeof(ICollection<>) && x.PropertyType != typeof(Content));

            var tabs = TabVm.GetTabs(editors);

            return PartialView(new MultiEditViewModel { Property = property, Tabs = tabs });
        }
    }
}