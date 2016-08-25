using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class Separator : Parameter
    {
        public Expression Value { get; set; }

        public Separator() { }

        public Separator(Expression separator)
        {
            Value = separator;
        }
    }
}
