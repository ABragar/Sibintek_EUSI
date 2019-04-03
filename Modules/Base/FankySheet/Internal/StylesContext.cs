using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Tuple = System.Tuple;

namespace FankySheet.Internal
{
    public class StylesContext
    {
        public class StyleContextItem
        {
            public StyleContextItem(uint style_index, uint style_id)
            {
                StyleIndex = style_index;
                StyleId = style_id;
            }

            public uint StyleIndex { get; }

            public uint StyleId { get; }
        }

        private readonly Stylesheet _stylesheet;

        private uint _current_id = 164;

        public StylesContext(Stylesheet stylesheet)
        {
            _stylesheet = stylesheet;
        }

        public int Count => _styles.Count;


        public StyleContextItem GetOrCreateStyle<TStyle>(Guid uid, Func<uint, TStyle> create_style_func)
            where TStyle : OpenXmlElement
        {
            StyleContextItem item;

            if (_styles.TryGetValue(uid, out item))
            {
                return item;
            }



            var style = create_style_func(++_current_id);

            var index = AddStyle(style);

            item = new StyleContextItem(index, _current_id);

            _styles.Add(uid, item);

            return item;
        }

        private uint AddStyle<TStyle>(TStyle style) where TStyle : OpenXmlElement
        {

            var container = GetContainer<TStyle>();

            container.Item1.AppendChild(style);

            return container.Item2.Value++;
        }

        private Tuple<OpenXmlCompositeElement, UInt32Value> GetContainer<TStyle>() where TStyle : OpenXmlElement
        {
            Tuple<OpenXmlCompositeElement, UInt32Value> container;

            if (!_containers.TryGetValue(typeof(TStyle), out container))
            {
                Func<StylesContext, Tuple<OpenXmlCompositeElement, UInt32Value>> func;

                if (!_create_containers.TryGetValue(typeof(TStyle), out func))
                {
                    throw new NotSupportedException();
                }

                container = func(this);
                _containers.Add(typeof(TStyle), container);
            }

            return container;
        }


        private static readonly Dictionary<Type, Func<StylesContext, Tuple<OpenXmlCompositeElement, UInt32Value>>> _create_containers = new Dictionary<Type, Func<StylesContext, Tuple<OpenXmlCompositeElement, UInt32Value>>>()
        {
             { typeof(NumberingFormat), c=> CreateTuple(c._stylesheet.NumberingFormats = new NumberingFormats { Count = 0 } ,x=>x.Count)},
             { typeof(Font), c=> CreateTuple(c._stylesheet.Fonts = new Fonts { Count = 0 } ,x=>x.Count)},
             { typeof(Fill), c=> CreateTuple(c._stylesheet.Fills = new Fills() { Count = 0 } ,x=>x.Count)},
             { typeof(Border), c=> CreateTuple(c._stylesheet.Borders = new Borders() { Count = 0 } ,x=>x.Count)},
             { typeof(CellStyle), c=> CreateTuple(c._stylesheet.CellStyles = new CellStyles() { Count = 0 } ,x=>x.Count)},
             { typeof(CellFormat), InitCellFormats }

        };

        private static Tuple<OpenXmlCompositeElement, UInt32Value> CreateTuple<T>(T item, Func<T, UInt32Value> item2)
            where T : OpenXmlCompositeElement
        {
            return Tuple.Create<OpenXmlCompositeElement, UInt32Value>(item, item2(item));
        }

        private static Tuple<OpenXmlCompositeElement, UInt32Value> InitCellFormats(StylesContext context)
        {
            context.AddStyle(new Font());

            context.AddStyle(new Fill());

            context.AddStyle(new Border());

            context.AddStyle(new CellStyle() { FormatId = 0 });

            context._stylesheet.CellStyleFormats = new CellStyleFormats(new CellFormat()) { Count = 1 };

            return CreateTuple(context._stylesheet.CellFormats = new CellFormats(new CellFormat { FormatId = 0 }) { Count = 1 }, x => x.Count);

        }


        private readonly Dictionary<Type, Tuple<OpenXmlCompositeElement, UInt32Value>> _containers = new Dictionary<Type, Tuple<OpenXmlCompositeElement, UInt32Value>>();

        private readonly Dictionary<Guid, StyleContextItem> _styles = new Dictionary<Guid, StyleContextItem>();


    }
}