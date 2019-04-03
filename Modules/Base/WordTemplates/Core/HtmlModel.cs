using System.Collections.Generic;

namespace WordTemplates.Core
{
    public class HtmlModel
    {

        public string Id { get; } = Generator.Generate();

        public string Name { get; }

        public ContentControlType Type { get; }

        private readonly Dictionary<object, HtmlModel> _models = new Dictionary<object, HtmlModel>();

        public IEnumerable<HtmlModel> Models => _models.Values;


        public HtmlModel(ContentControlInfo info)
        {
            Name = info?.Name ?? "";
            Type = info?.Type ?? ContentControlType.Unknown;
        }

        public HtmlModel Add(ContentControlInfo info)
        {
            return _models.GetOrAdd(new { info.Name, info.Type }, () => new HtmlModel(info));
        }




    }
}