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
        public static IEvaluatedExpression Evaluate(this IExpression expression, IFunctionTable funcTable)
        {
            return expression.Accept(new ExpressionEvaluator(funcTable));
        }

        public static bool CheckCondition(this IfStatement statement, IFunctionTable funcTable)
        {
            return funcTable.CheckCondition(
                statement.Condition.Name,
                new FunctionParameters(statement.Condition.Parameters));
        }
    }
}
