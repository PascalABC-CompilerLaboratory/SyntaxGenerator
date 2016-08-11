using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    internal class Identifier : Expression
    {
        public string Value { get; set; }

        public Identifier() { }

        public Identifier(string value)
        {
            Value = value;
        }
    }
}
