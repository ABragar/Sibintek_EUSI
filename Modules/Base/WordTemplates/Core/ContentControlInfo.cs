using System;
using System.Xml.Linq;

namespace WordTemplates.Core
{
    public class ContentControlInfo
    {
        public ContentControlInfo(XElement sdt)
        {
            if (sdt.Name != Word.Sdt)
                throw new ArgumentException(nameof(sdt));

            var sdt_pr = sdt.Element(Word.SdtPr);

            Tag = GetSdtPrTag(sdt_pr);

            Type = GetSdtPrType(sdt_pr);

            Name = GetSdtPrName(sdt_pr) ?? "";
        }

        public string Name { get; }
        public string Tag { get; }
        public ContentControlType Type { get; }

        public static ContentControlType GetSdtPrType(XElement sdt_pr)
        {

            if (sdt_pr != null)
            {
                if (sdt_pr.Element(Word.RepeatingSection) != null)
                    return ContentControlType.Repeat;

                if (sdt_pr.Element(Word.Text) != null)
                    return ContentControlType.Text;
            }

            return ContentControlType.Unknown;

        }

        public static string GetSdtPrTag(XElement sdt_pr)
        {
            return sdt_pr?
                .Element(Word.Tag)?
                .Attribute(Word.Val)?
                .Value;

        }

        public static string GetSdtPrName(XElement sdt_pr)
        {
            return sdt_pr?
                .Element(Word.Alias)?
                .Attribute(Word.Val)?
                .Value;


        }
    }
}