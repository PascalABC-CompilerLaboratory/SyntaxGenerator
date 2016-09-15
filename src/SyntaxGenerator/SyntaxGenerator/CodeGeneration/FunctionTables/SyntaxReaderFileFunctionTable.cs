using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;

namespace SyntaxGenerator.CodeGeneration
{
    public class SyntaxReaderFileFunctionTable : IFunctionTable
    {
        private SyntaxInfo _syntaxInfo;

        /// <summary>
        /// Сопоставляет имя функции из текста шаблона делегату функции
        /// </summary>
        protected Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>> functions =
            new Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>>();

        public SyntaxReaderFileFunctionTable(SyntaxInfo syntaxInfo, IEnumerable<string> readerNodesCode)
        {
            _syntaxInfo = syntaxInfo;

            functions["ReaderNodesCode"] = p => readerNodesCode;
            functions["NodeName"] = p => _syntaxInfo.Nodes.Select(node => node.Name);
            functions["IntSequence"] = p => Enumerable.Range(0, int.MaxValue).Select(n => n.ToString());
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
                return functions.Keys;
            }
        }

        public void AddExpressionAlias(string name, IExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> CallFunction(string name, IFunctionParameters parameters = null)
        {
            return functions[name](parameters);
        }

        public bool CheckCondition(string name, IFunctionParameters parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
