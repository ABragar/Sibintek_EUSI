using System.Runtime.CompilerServices;
using WordTemplates;

namespace Base.Word.Services.Concrete
{
    public static class TemplateContentExtensions
    {
        private const ulong HashConst = 3074457345618258791ul;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong GenerateHash(string str, ulong start)
        {
            if (str != null)
            {
                for (var i = 0; i < str.Length; i++)
                {
                    start += str[i];
                    start *= HashConst;
                }
            }
            return start;
        }

        internal static ulong GenerateHash(this TemplateContent content, ulong start = HashConst)
        {

            foreach (var value in content.Values.Values)
            {

                start = GenerateHash(value.Title, start);
                start = GenerateHash(value.Url, start);
            }

            foreach (var item in content.Items.Values)
            {
                foreach (var value in item)
                {
                    start = value.GenerateHash(start);
                }

            }

            return start;
        }
    }
}