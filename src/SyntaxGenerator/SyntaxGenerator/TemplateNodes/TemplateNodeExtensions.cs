using SyntaxGenerator.CodeGeneration;
using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public static class TemplateNodeExtensions
    {
        public static IEnumerable<string> Evaluate(this IExpression expression, IFunctionTable funcTable)
        {
            return expression.Accept(new ExpressionEvaluator(funcTable));
        }
    }
}
