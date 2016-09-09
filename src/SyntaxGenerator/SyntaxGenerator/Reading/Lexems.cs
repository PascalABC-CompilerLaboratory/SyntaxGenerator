using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Reading
{
    public class Lexems
    {
        public static readonly string TemplateOpenSymbol = "<#";
        public static readonly string TemplateCloseSymbol = "#>";
        public static readonly string AssignSymbol = "=";
        public static readonly string Comma = ",";
        public static readonly string OpenRoundBracket = "(";
        public static readonly string CloseRoundBracket = ")";
        public static readonly string Colon = ":";

        public static readonly string SetKeyword = "set";
        public static readonly string JoinKeyword = "join";
        public static readonly string IfKeyword = "if";
        public static readonly string TrueKeyword = "true";
        public static readonly string FalseKeyword = "false";
    }
}
