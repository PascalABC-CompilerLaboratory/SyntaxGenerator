using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    /// <summary>
    /// Таблицы функций
    /// </summary>
    public class TableStorage
    {
        /// <summary>
        /// Позволяет получить таблицу, подходящую для генерации заданного узла
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Таблица функций</returns>
        public static IFunctionTable GetSyntaxTable(SyntaxNodeInfo node)
        {
            return new SyntaxFunctionTable(node);
        }

        public static IFunctionTable GetSyntaxTreeFileTable(IEnumerable<string> syntaxNodes)
        {
            return new SyntaxTreeFileFunctionTable(syntaxNodes);
        }
    }
}
