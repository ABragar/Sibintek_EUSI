using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WordTemplates
{
    public class TemplateContent

    {
        public Dictionary<string, TemplateValue> Values { get; } = new Dictionary<string, TemplateValue>();
        public Dictionary<string, List<TemplateContent>> Items { get; } = new Dictionary<string, List<TemplateContent>>();

    }
}