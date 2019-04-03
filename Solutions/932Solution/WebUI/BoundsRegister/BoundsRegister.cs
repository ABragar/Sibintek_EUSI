using System.Web.Mvc;
using WebUI.Service;

namespace WebUI.BoundsRegister
{
    public static class BoundsRegister
    {
        private static IColumnBoundRegisterService _boundRegisterService;

        public static void Init()
        {
            _boundRegisterService = DependencyResolver.Current.GetService<IColumnBoundRegisterService>();

            BaseBounds.Init(_boundRegisterService);
            DataBounds.Init(_boundRegisterService);
        }

        internal static string CreateClientFooterTemplate(string columnName)
        {
            var template = "";
            template += string.Format("#= data.{0} != null && data.{0}.count != null ? 'Всего - ' %2B kendo.toString(data.{0}.count, 'n0') : '' #",
                columnName);

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.average != null ? 'Среднее - ' %2B kendo.toString(data.{0}.average, 'n') : '' #", columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.average != null ? '<br/>Среднее - ' %2B kendo.toString(data.{0}.average, 'n') : '' #", columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.max != null ? 'Макс.. - ' %2B kendo.toString(data.{0}.max, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.max != null ? '<br/>Макс.. - ' %2B kendo.toString(data.{0}.max, 'n') : '' #",
                columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.min != null ? 'Мин. - ' %2B kendo.toString(data.{0}.min, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.min != null ? '<br/>Мин. - ' %2B kendo.toString(data.{0}.min, 'n') : '' #",
                columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.sum != null ? '&\\#931; ' %2B kendo.toString(data.{0}.sum, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.sum != null ? '<br/>&\\#931; ' %2B kendo.toString(data.{0}.sum, 'n') : '' #",
                columnName);
            }



            template = "<div>" + template + "</div>";
            return template;
        }

        internal static string CreateClientGroupFooterTemplate(string columnName)
        {
            var template = "";
            template += string.Format("#= data.{0} != null && data.{0}.count != null ? 'Всего - ' + kendo.toString(data.{0}.count, 'n0') : '' #",
                columnName);

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.average != null ? 'Среднее - ' + kendo.toString(data.{0}.average, 'n') : '' #", columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.average != null ? '<br/>Среднее - ' + kendo.toString(data.{0}.average, 'n') : '' #", columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.max != null ? 'Макс.. - ' + kendo.toString(data.{0}.max, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.max != null ? '<br/>Макс.. - ' + kendo.toString(data.{0}.max, 'n') : '' #",
                columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.min != null ? 'Мин. - ' + kendo.toString(data.{0}.min, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.min != null ? '<br/>Мин. - ' + kendo.toString(data.{0}.min, 'n') : '' #",
                columnName);
            }

            if (template == "")
            {
                template += string.Format("#= data.{0} != null && data.{0}.sum != null ? '&\\#931; ' + kendo.toString(data.{0}.sum, 'n') : '' #",
                columnName);
            }
            else
            {
                template += string.Format("#= data.{0} != null && data.{0}.sum != null ? '<br/>&\\#931; ' + kendo.toString(data.{0}.sum, 'n') : '' #",
                columnName);
            }


            template = "<div>" + template + "</div>";

            return template;
        }
    }
}