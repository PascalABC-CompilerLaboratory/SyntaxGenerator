using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.Visitors;

namespace SyntaxGenerator.TemplateNodes
{
    public class Assignment : IStatement
    {
        public string VariableName { get; set; }

        public IExpression Value { get; set; }

        public Assignment() { }

        public Assignment(string variable, IExpression value)
        {
            VariableName = variable;
            Value = value;
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
