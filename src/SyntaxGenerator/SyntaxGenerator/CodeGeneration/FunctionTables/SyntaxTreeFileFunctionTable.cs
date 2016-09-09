using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;

namespace SyntaxGenerator.CodeGeneration
{
    public class SyntaxTreeFileFunctionTable : IFunctionTable
    {
        private static readonly string syntaxTreeCode = "SyntaxTreeNodes";

        private IEnumerable<string> _syntaxNodes;

        public SyntaxTreeFileFunctionTable(IEnumerable<string> syntaxNodes)
        {
            _syntaxNodes = syntaxNodes;
        }

        public IEnumerable<string> ConditionNames
        {
            get
            {
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> FunctionNames
        {
            get
            {
                yield return syntaxTreeCode;
            }
        }

        public IEnumerable<string> CallFunction(string name, IFunctionParameters parameters)
        {
            if (name == syntaxTreeCode)
                return _syntaxNodes;

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
