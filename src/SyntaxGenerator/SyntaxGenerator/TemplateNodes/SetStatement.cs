using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class SetStatement : IStatement
    {
        public string VariableName { get; set; }

        public IExpression Value { get; set; }

        public SetStatement() { }

        public SetStatement(string variable, IExpression value)
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
