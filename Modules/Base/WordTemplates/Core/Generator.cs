using System;

namespace WordTemplates.Core
{
    public static class Generator
    {
        public static string Generate()
        {
            return "id"+Guid.NewGuid().ToString("N");

        }
    }
}