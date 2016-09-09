using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public static class StringExtensions
    {
        public static string AddIndent(this string str, string indent)
        {
            if (indent == null || indent == "")
                return str;

            return string.Join(Environment.NewLine, str
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(s => indent + s));
        }

        public static IEnumerable<string> AddIndent(this IEnumerable<string> strings, string indent)
        {
            foreach (string s in strings)
                yield return s.AddIndent(indent);
        }

        public static string[] Lines(this string str)
        {
            return str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
