using Base;
using Common.Data.Entities.Test;
using WebUI.Service;

namespace WebUI.BoundsRegister
{
    public static class DataBounds
    {
        public static void Init(IColumnBoundRegisterService boundRegisterService)
        {
            string downloadFileTemplate = "<a href='#= pbaAPI.getHrefFile(data.FileID) #' target=\"_blank\" " +
                                  "data-uid='#= data.uid #' " +
                                  "class='fa fa-upload list-group-item #: data.ID > 0 ? \"\" : \"list-group-item-new\"  #'>" +
                                  "  Скачать</a>";
            boundRegisterService
                    //.Register<FileData, System.Guid>(x => x.FileID)
                    .Register<FileData, string>(x => x.Extension)
                    .Create((builder, preset, column, grid) =>
                    {
                        builder
                            .ClientTemplate(downloadFileTemplate);
                    });
            #region Примеры
            /*
            boundRegisterService
                .Register<TestObject, double>(x => x.Double)
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate("#=data.Double# Кастомный темплейт")
                        .Width(preset.Width ?? 250)
                        .HtmlAttributes(new { style = "color: red;" });
                });

            boundRegisterService
                .Register<TestObject, User>(x => x.Creator)
                .InitDefault()
                .InitBaseObjectBound(true)
                .Create((builder, preset, column, grid) =>
                {
                    builder.Width(preset.Width ?? 50);
                });
            */

            boundRegisterService
                .Register<TestObject, double>(x => x.Double)
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate("#=data.Double# Кастомный темплейт")
                        .Width(preset.Width ?? 250)
                        .HtmlAttributes(new { style = "color: red;" });
                });

            boundRegisterService
                .Register<TestObject, double>("TestObject2", x => x.Double)
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .HtmlAttributes(new { style = "background-color: blue;" });
                });

            #endregion
        }
    }
}