using System.Collections.Generic;

namespace WebUI.Areas.Account.Models.Shared
{
    public class ActionResultModel
    {
        public string GoBackUrl { get; set; }
        public bool Success { get; set; }

        public IReadOnlyCollection<string> Messages { get; set; }

        public static ActionResultModel ExternalInfoNotFoundResult(string go_back_url)
        {
            return new ActionResultModel
            {
                GoBackUrl = go_back_url,
                Success = false,
                Messages = new[] {"Не удалось получить информацию о методе входа"}
            };

        }

        public static ActionResultModel MessageSendResult(string go_back_url)
        {
            return new ActionResultModel
            {
                GoBackUrl = go_back_url,
                Success = true,
                Messages = new[] { "На адрес электронной почты выслано письмо с дальнейшими инструкциями" }
            };

        }


        public static ActionResultModel NotAllowedResult(string go_back_url)
        {
            return new ActionResultModel
            {
                GoBackUrl = go_back_url,
                Success = false,
                Messages = new[] { "Действие не разрешено" }
            };

        }

        public static ActionResultModel Error(string go_back_url)
        {
            return new ActionResultModel
            {
                GoBackUrl = go_back_url,
                Success = false,
                Messages = new[] { "Ошибка сервера" }
            };

        }
    }
}