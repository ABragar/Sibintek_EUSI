using System;
using System.Collections.Generic;
using WebUI.Controllers;

namespace WebUI.Models
{
    public class CustomGridConfig
    {
        public List<CustomGridConfigProperty> Columns { get; set; } = new List<CustomGridConfigProperty>();
    }

    public class CustomGridConfigProperty
    {
        public string Title { get; set; }
        public Type Type { get; set; }
    }

    public class CustomGridModel : StandartGridView
    {
        public int? ObjectID { get; set; }
        public CustomGridConfig Config { get; set; }

        public CustomGridModel(IBaseController controller, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame) : base(controller, mnemonic, dialogID, type)
        {
        }

        public CustomGridModel(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame) : base(baseViewModel, mnemonic, dialogID, type)
        {
        }

        public CustomGridModel(DialogViewModel dialogViewModel) : base(dialogViewModel)
        {
        }

        public CustomGridModel(IBaseController controller) : base(controller)
        {
        }
    }
}