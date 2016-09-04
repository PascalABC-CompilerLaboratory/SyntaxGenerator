using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public abstract class Expression : IExpression
    {
        public string Separator { get; set; }

        public Expression(string separator)
        {
            Separator = separator;
        }

        public abstract void Accept(IVisitor visitor);
        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
