using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public abstract class FunctionTable : IFunctionTable
    {
        /// <summary>
        /// Информация о текущем узле
        /// </summary>
        protected SyntaxNodeInfo node;

        /// <summary>
        /// Сопоставляет имя функции из текста шаблона делегату функции
        /// </summary>
        protected Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>> functions = 
            new Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>>();

        public virtual IEnumerable<string> CallFunction(string name, IFunctionParameters parameters)
            => functions[name](parameters);

        public void SetCurrentNode(SyntaxNodeInfo node) => this.node = node;
    }
}
