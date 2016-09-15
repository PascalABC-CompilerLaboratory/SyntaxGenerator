using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;

namespace SyntaxGenerator.CodeGeneration
{
    public class SingleValueFunctionTable : IFunctionTable
    {
        private string _funcName;

        private IEnumerable<string> _syntaxNodes;

        public SingleValueFunctionTable(string funcName, IEnumerable<string> syntaxNodes)
        {
            _funcName = funcName;
            _syntaxNodes = syntaxNodes;
        }

        public IEnumerable<string> ConditionNames => Enumerable.Empty<string>();

        public IEnumerable<string> FunctionNames
        {
            get
            {
                yield return _funcName;
            }
        }

        public IEnumerable<string> CallFunction(string name, IFunctionParameters parameters)
        {
            if (name == _funcName)
                return _syntaxNodes;
            else
                throw new ArgumentException($"{name}: no such function");

            throw new ArgumentException(nameof(name));
        }

        public void AddExpressionAlias(string name, IExpression expression)
        {
            throw new NotImplementedException();
        }

        public bool CheckCondition(string name, IFunctionParameters parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
