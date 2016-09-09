using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Char;

namespace SyntaxGenerator.TemplateNodes
{
    public class CSharpCode : ICodePart
    {
        private string _indent = null;

        public string Code { get; set; }

        public string Indent
        {
            get
            {
                if (_indent == null)
                    _indent = CalculateIndent();
                 return _indent;
            }

            private set { _indent = value; }
        }

        public CSharpCode() { }

        public CSharpCode(string code)
        {
            Code = code;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitCSharpCode(this);
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitCSharpCode(this);
        }

        private string CalculateIndent()
        {
            if (Code == null)
                return null;

            return string.Concat(Code
                .Reverse()
                .TakeWhile(
                c => IsWhiteSpace(c) && c != '\r' && c != '\n'));
        }
    }
}
