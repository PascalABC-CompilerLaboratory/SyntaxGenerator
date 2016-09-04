using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;
using SyntaxGenerator.Visitors;

namespace SyntaxGenerator.CodeGeneration
{
    public class ExpressionEvaluator : IVisitor<IEnumerable<string>>
    {
        /// <summary>
        /// Значения выражений
        /// </summary>
        protected IFunctionTable functionTable;

        public ExpressionEvaluator(IFunctionTable funcTable)
        {
            this.functionTable = funcTable;
        }

        public IEnumerable<string> Visit(FormatString formatString)
        {
            if (formatString.Arguments.Count == 0)
            {
                yield return formatString.Format;
                yield break;
            }

            // Списки аргументов, по каждому из списков будет построена строка
            // и добавлена в результат
            List<List<string>> argumentsList = new List<List<string>>();

            var evaluator = new ExpressionEvaluator(functionTable);
            formatString.Arguments[0].Accept(evaluator);

            // Количество строк в результате, минимальное из всех представленных
            int resultCount = 0;
            // Обрабатываем список первых аргументов
            foreach (string firstArg in formatString.Arguments[0].Accept(evaluator))
            {
                argumentsList.Add(new List<string>());
                argumentsList[0].Add(firstArg);
                resultCount++;
            }

            // Обрабатываем списки остальных аргументов
            for (int argNum = 1; argNum < formatString.Arguments.Count; ++argNum)
            {
                evaluator = new ExpressionEvaluator(functionTable);
                formatString.Arguments[argNum].Accept(evaluator);

                var argCount = 0;
                foreach (string arg in formatString.Arguments[argNum].Accept(evaluator))
                {
                    // Если количество превышает текущее минимальное, прекращаем добавление аргументов
                    if (++argCount > resultCount)
                        break;
                    argumentsList[argNum].Add(arg);
                }
                // Обновляем минимальное количество
                resultCount = argCount;
            }

            // Возвращаем строки с подставленными значениями
            for (int i = 0; i < resultCount; ++i)
                yield return string.Format(formatString.Format, argumentsList[i]);
        }

        public IEnumerable<string> Visit(CSharpCode code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Visit(FunctionCall funcCall)
        {
            return functionTable.CallFunction(
                funcCall.Name,
                new FunctionParameters(funcCall.Parameters));
        }

        public IEnumerable<string> Visit(SetStatement setStatement)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Visit(Template template)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Visit(Assignment assignment)
        {
            throw new NotImplementedException();
        }
    }
}
