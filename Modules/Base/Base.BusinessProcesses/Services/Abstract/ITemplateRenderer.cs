using System.Collections.Generic;
using Base.BusinessProcesses.Entities;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface ITemplateRenderer
    {
        string Render(string template, IBPObject obj, IDictionary<string, string> additional = null);
    }
}