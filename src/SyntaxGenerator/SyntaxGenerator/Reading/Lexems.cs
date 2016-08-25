using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Reading
{
    internal class Lexems
    {
        internal static readonly string TemplateOpenSymbol = "<#";
        internal static readonly string TemplateCloseSymbol = "#>";
        internal static readonly string AssignSymbol = "=";

        internal static readonly string JoinKeyword = "join";
        internal static readonly string SetKeyword = "set";
    }
}
