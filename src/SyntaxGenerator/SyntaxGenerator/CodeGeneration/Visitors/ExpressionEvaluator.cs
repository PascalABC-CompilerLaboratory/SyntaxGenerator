using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;
using SyntaxGenerator.Visitors;
using System.Text.RegularExpressions;

namespace SyntaxGenerator.CodeGeneration
{
    public class ExpressionEvaluator : IVisitor<IEvaluatedExpression>
    {
        /// <summary>
        /// Значения выражений
        /// </summary>
        protected IFunctionTable functionTable;

        public ExpressionEvaluator(IFunctionTable funcTable)
        {
            this.functionTable = funcTable;
        }

        public IEvaluatedExpression VisitAssignment(Assignment assignment)
        {
            throw new NotImplementedException();
        }

        public IEvaluatedExpression VisitCSharpCode(CSharpCode code)
        {
            throw new NotImplementedException();
        }

        public IEvaluatedExpression VisitFormatString(FormatString formatString)
        {
            IEnumerable<string> values = VisitFormatStringImpl(formatString);

            return new TextExpression(formatString.Separator == null ?
                    values :
                    Enumerable.Repeat(string.Join(formatString.Separator, values), 1));
        }

        public IEnumerable<string> VisitFormatStringImpl(FormatString formatString)
        {
            if (formatString.Arguments.Count == 0)
            {
                yield return formatString.Format;
                yield break;
            }

            /*
            // Списки аргументов, по каждому из списков будет построена строка
            // и добавлена в результат
            List<List<string>> argumentsList = new List<List<string>>();

            var evaluator = new ExpressionEvaluator(functionTable);

            // Количество строк в результате, минимальное из всех представленных
            int resultCount = 0;
            // Обрабатываем список первых аргументов
            foreach (string firstArg in formatString.Arguments[0].Accept(evaluator).Values)
            {
                argumentsList.Add(new List<string>());
                argumentsList.Last().Add(firstArg);
                resultCount++;
            }

            // Обрабатываем списки остальных аргументов
            for (int argNum = 1; argNum < formatString.Arguments.Count; ++argNum)
            {
                evaluator = new ExpressionEvaluator(functionTable);

                var argCount = 0;
                foreach (string arg in formatString.Arguments[argNum].Accept(evaluator).Values)
                {
                    // Если количество превышает текущее минимальное, прекращаем добавление аргументов
                    if (++argCount > resultCount)
                        break;
                    argumentsList[argCount - 1].Add(arg);
                }
                // Обновляем минимальное количество
                resultCount = argCount;
            }

            // Возвращаем строки с подставленными значениями
            if (resultCount == 0)
                yield break;

            for (int i = 0; i < resultCount; ++i)
                yield return string.Format(formatString.Format, argumentsList[i].ToArray());
            */

            List<IEnumerator<string>> argumentsList = new List<IEnumerator<string>>();
            var evaluator = new ExpressionEvaluator(functionTable);

            for (int i = 0; i < formatString.Arguments.Count; i++)
            {
                argumentsList.Add((formatString.Arguments[i].Accept(evaluator) as TextExpression).Values.GetEnumerator());
                if (!argumentsList.Last().MoveNext())
                    yield break;
            }

            bool lastResult = false;
            while (!lastResult)
            {
                 yield return 
                    Regex.Replace(formatString.Format, @"{\d+}",
                    match =>
                    {
                        var ind = int.Parse(string.Concat(match.Value.Skip(1).TakeWhile(char.IsDigit)));
                        var elem = argumentsList[ind].Current;
                        if (argumentsList[ind].MoveNext() == false)
                            lastResult = true;
                        return elem; 
                    })
                    .Replace("{{", "{")
                    .Replace("}}", "}");
            }
        }

        public IEvaluatedExpression VisitFunctionCall(FunctionCall funcCall)
        {
            if (functionTable.FunctionNames.Contains(funcCall.Name))
            {
                var result = functionTable.CallFunction(funcCall.Name, new FunctionParameters(funcCall.Parameters));
                return new TextExpression(funcCall.Separator == null ?
                    result :
                    Enumerable.Repeat(string.Join(funcCall.Separator, result), 1));
            }
            else
                return new
                    ConditionExpression(functionTable.CheckCondition(funcCall.Name, new FunctionParameters(funcCall.Parameters)));
        }

        public IEvaluatedExpression VisitIfStatement(IfStatement ifStatement)
        {
            throw new NotImplementedException();
        }

        public IEvaluatedExpression VisitSetStatement(SetStatement setStatement)
        {
            throw new NotImplementedException();
        }

        public IEvaluatedExpression VisitTemplate(Template template)
        {
            throw new NotImplementedException();
        }
    }
}
