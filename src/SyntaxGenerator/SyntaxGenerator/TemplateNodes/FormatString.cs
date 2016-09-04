using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class FormatString : Expression
    {
        public string Format { get; set; } = "";

        public List<IExpression> Arguments { get; set; } = new List<IExpression>();

        public FormatString() : base("") { }

        public FormatString(string format, IEnumerable<IExpression> args, string separator = "") :
            base(separator)
        {
            Format = format;
            Arguments = new List<IExpression>(args);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
