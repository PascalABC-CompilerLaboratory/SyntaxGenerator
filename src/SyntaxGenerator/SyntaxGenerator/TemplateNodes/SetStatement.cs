using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class SetStatement : Statement
    {
        public Identifier VariableName { get; set; }

        public Expression Value { get; set; }

        public SetStatement() { }

        public SetStatement(Identifier variable, Expression value)
        {
            VariableName = variable;
            Value = value;
        }
    }
}
