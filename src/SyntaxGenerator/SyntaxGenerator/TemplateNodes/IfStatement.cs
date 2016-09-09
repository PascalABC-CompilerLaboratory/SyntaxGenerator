using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.Visitors;

namespace SyntaxGenerator.TemplateNodes
{
    public class IfStatement : IStatement
    {
        // TODO: Заменить FunctionCall на Condition, убрать из предков IExpression
        public FunctionCall Condition { get; set; }

        public List<ICodePart> Body { get; set; }
         
        public IfStatement() { Body = new List<ICodePart>(); }

        public IfStatement(FunctionCall condition, IEnumerable<ICodePart> body)
        {
            Condition = condition;
            Body = new List<ICodePart>(body);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitIfStatement(this);
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIfStatement(this);
        }
    }
}
