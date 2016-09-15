using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public class TextExpression : IEvaluatedExpression
    {
        public IEnumerable<string> Values { get; }

        public TextExpression(IEnumerable<string> values)
        {
            Values = values;
        }

        //public string GetResult(string separator, string indent)
        //{
        //    var newRes = Values.AddIndent(indent);
        //    if (separator == null)
        //        return string.Join(_separator == null ? "" : _separator, newRes);
        //    else
        //        return string.Join(separator, newRes);
        //}
    }
}
