using WebUI.Controllers;
using WebUI.Models.CorpProp.Response.Config;

namespace WebUI.Models.CorpProp.Response
{
    public class ResponseGridModel: StandartGridView
    {
        void Init()
        {
            //Mnemonic = "RequestDynamicType";
            //После фикса 
            //Preset = new GridPreset();
        }
        public ResponseGridModel(IBaseController controller, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame) : base(controller, mnemonic, dialogID, type)
        {
            Init();
        }

        public ResponseGridModel(BaseViewModel baseViewModel, string mnemonic, string dialogID, TypeDialog type = TypeDialog.Frame) : base(baseViewModel, mnemonic, dialogID, type)
        {
            Init();
        }

        public ResponseGridModel(DialogViewModel dialogViewModel) : base(dialogViewModel)
        {
            Init();
        }

        public ResponseGridModel(IBaseController controller) : base(controller)
        {
            Init();
        }

        public int? ResponseID { get; set; }
        public int? RequestID { get; set; }
        public ResponseGridConfig Config { get; set; }
    }
}