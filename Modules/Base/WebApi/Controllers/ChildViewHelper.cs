using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    internal static class ChildViewHelper
    {
        internal const string LinkKey = "link";

        internal static void GetLinkParams(string link, out string childParam, out string parentParam, out bool parentChildReverse)
        {
            var parentPrefix = "parent.";
            var parentPrefixIndex = link.IndexOf(parentPrefix, StringComparison.OrdinalIgnoreCase);
            var childPrefix = "child.";
            var childPrefixIndex = link.IndexOf(childPrefix, StringComparison.OrdinalIgnoreCase);
            var setterIndex = link.IndexOf('=');

            if ((parentPrefixIndex <= 0 && childPrefixIndex <= 0) || setterIndex <= 0)
            {
                //link неверен
                throw new Exception($"link \"{link}\" неверен");
            }

            parentChildReverse = parentPrefixIndex > childPrefixIndex;

            childParam = parentChildReverse
                ? link.Substring(childPrefixIndex + childPrefix.Length, setterIndex - (childPrefix.Length + childPrefixIndex))
                : link.Substring(setterIndex + childPrefix.Length + 1, link.Length - (setterIndex + childPrefix.Length + 1)).TrimEnd();
            parentParam = parentChildReverse
                ? link.Substring(setterIndex + parentPrefix.Length + 1, link.Length - (setterIndex + parentPrefix.Length + 1)).TrimEnd()
                : link.Substring(parentPrefixIndex + parentPrefix.Length, setterIndex - (parentPrefixIndex + parentPrefix.Length));

        }

    }
}
