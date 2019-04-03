using System;
using System.Collections.Generic;
using System.Drawing.Design;

namespace WordTemplates
{
    [Serializable]
    public class Template
    {

        public HashSet<string> Values { get; } = new HashSet<string>();

        public Dictionary<string, Template> Items { get; } = new Dictionary<string, Template>();
    }












  
}




