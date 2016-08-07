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
        public List<Expression> Arguments { get; set; } = new List<Expression>();

        public FormatString() { }

        public FormatString(string format, IEnumerable<Expression> args)
        {
            Format = format;
            Arguments = new List<Expression>(args);
        } 
    }
}
