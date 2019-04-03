using Base.Service;
using Base.UI.Service;
using ExCSS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Base.Utils.Common.Maybe;

namespace WebUI.Concrete
{
    public class IconService : IIconService
    {
        private readonly IPathHelper _pathHelper;

        public IconService(IPathHelper pathHelper)
        {
            this._pathHelper = pathHelper;
        }

        private string GetFontsDirectory() => Path.Combine(_pathHelper.GetContentDirectory(), "fonts");

        private IEnumerable<IconsSet> ParseIcons(string dirName, IEnumerable<string> files)
        {
            string path = Path.Combine(GetFontsDirectory(), dirName);

            var parser = new Parser();

            var list = new List<IconsSet>();

            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(Path.Combine(path, file), Encoding.Default);

                var stylesheet = parser.Parse(fileContent);

                var set = new IconsSet
                {
                    Icons = stylesheet.StyleRules.Select(x => x.Selector.ToString())
                        .Where(x => x.Contains(":before"))
                        .Select(x => x.Replace(":before", string.Empty).Split(',').First())
                        .Select(x => string.Format("{0} {1}",
                            x.Split('.', '-')[1],
                            x.Replace(".", string.Empty)))
                        .ToList(),
                    Title = fileContent.Split('\r', '\n')
                        .FirstOrDefault()
                        .With(x => Regex.Match(x, @"/\*(.*?)\*/").Groups[1].Value)
                };

                list.Add(set);
            }

            return list;
        }

        public IList<IconsSet> GetIcons()
        {
            var glyphicons = Directory.GetFiles(Path.Combine(GetFontsDirectory(), "glyphicons"), "*.css")
                .Where(x => !x.EndsWith(".min.css"));
            var fontello = Directory.GetFiles(Path.Combine(GetFontsDirectory(), "fontello"), "*.css")
                .Where(x => !x.EndsWith(".min.css"));
            var fontAwesome = Directory.GetFiles(Path.Combine(GetFontsDirectory(), "font-awesome"), "*.css")
                .Where(x => !x.EndsWith(".min.css"));
            var material = Directory.GetFiles(Path.Combine(GetFontsDirectory(), "mdi"), "*.css")
                .Where(x => !x.EndsWith(".min.css"));

            return ParseIcons("glyphicons", glyphicons)
                .Concat(ParseIcons("font-awesome", fontAwesome))
                .Concat(ParseIcons("fontello", fontello))
                .Concat(ParseIcons("mdi", material))
                .OrderBy(x => x.Title)
                .ToList();
        }
    }
}