using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class Assignment : Statement
    {
        public Identifier VariableName { get; set; }

        public Expression Value { get; set; }

        public Assignment() { }

        public Assignment(Identifier variable, Expression value)
        {
            VariableName = variable;
            Value = value;
        }
    }
}
