using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class CSharpCode : ICodePart
    {
        public string Code { get; set; }

        public CSharpCode() { }

        public CSharpCode(string code)
        {
            Code = code;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
