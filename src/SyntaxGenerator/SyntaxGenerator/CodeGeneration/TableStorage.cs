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
        /// Возвращает таблицу функций для синтаксического узла
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Таблица функций</returns>
        public static IFunctionTable GetSyntaxTable(SyntaxNodeInfo node)
            => new SyntaxFunctionTable(node);

        public static IFunctionTable GetSyntaxTreeFileTable(IEnumerable<string> syntaxNodesCode)
            => new SingleValueFunctionTable("SyntaxTreeNodes", syntaxNodesCode);

        public static IFunctionTable GetSyntaxStreamTable(SyntaxNodeInfo node)
            => new SyntaxStreamFunctionTable(node);

        public static IFunctionTable GetSyntaxWriterFileTable(IEnumerable<string> writerNodesCode)
            => new SingleValueFunctionTable("WriterNodesCode", writerNodesCode);

        public static IFunctionTable GetSyntaxReaderFileTable(SyntaxInfo syntaxInfo, IEnumerable<string> readerNodesCode)
            => new SyntaxReaderFileFunctionTable(syntaxInfo, readerNodesCode);
    }
}
