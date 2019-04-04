using System;
using Base.UI;
using Base.UI.Presets;
using Kendo.Mvc.UI.Fluent;
using WebUI.Models;

namespace WebUI.BoundsRegister
{
    public class BoundBuilder
    {
        private ColumnBoundConfig _config;

        public BoundBuilder(ColumnBoundConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Инициализация на основании пресета и атрибутов колонки
        /// </summary>
        public BoundBuilder InitDefault()
        {
            _config.IsInitColumn = true;
            return this;
        }

        /// <summary>
        /// Инициализация BaseObject свойства (Обязательно передавать "true" для любого BaseObject свойства)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public BoundBuilder InitBaseObjectBound(bool val)
        {
            _config.IsInitBaseObjectBound = val;
            return this;
        }

        /// <summary>
        /// Инициализация BaseObject-коллекции (Обязательно передавать "true" для любой BaseObject-коллекции)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public BoundBuilder InitBaseObjectCollectionBound(bool val)
        {
            _config.IsInitBaseObjectCollectionBound = val;
            return this;
        }

        /// <summary>
        /// Биндинг колонки с кастомным типом (Например, Enum-свойства биндятся как int)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BoundBuilder CustomBoundType(Type type)
        {
            _config.CustomBoundType = type;
            return this;
        }

        /// <summary>
        /// Делегат для баунда колонки
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public BoundBuilder Create(
            Action<GridBoundColumnBuilder<dynamic>, ColumnPreset, ColumnViewModel, StandartGridView> action)
        {
            _config.Delegate = (builder, preset, column, grid) =>
            {
                action(builder, preset, column, grid);
                object htmlAttr;
                builder.Column.HtmlAttributes.TryGetValue("class", out htmlAttr);
                if (preset.OneLine)
                {
                    if (htmlAttr == null)
                    {
                        string oneLineCss = "nowrapHiddenText";
                        builder.Column.HtmlAttributes.Add("class", oneLineCss);
                    }
                    else
                    {
                        string oneLineCss = (string)htmlAttr;
                        oneLineCss += " nowrapHiddenText";
                        builder.Column.HtmlAttributes["class"] = oneLineCss;
                    }
                }
            };
            return this;
        }
    }
}