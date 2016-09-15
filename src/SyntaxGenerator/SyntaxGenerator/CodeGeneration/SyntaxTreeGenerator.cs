using SyntaxGenerator.SyntaxNodes.Model;
using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    /// <summary>
    /// Генератор кода синтаксических узлов
    /// </summary>
    public class SyntaxTreeGenerator
    {
        TemplateStorage _templates;

        private delegate IFunctionTable TableProvider(SyntaxNodeInfo node);

        public SyntaxTreeGenerator(TemplateStorage templates)
        {
            _templates = templates;
        }

        private IList<string> GenerateCodeForEachNode(SyntaxTree tree, Template template, TableProvider tableProvider)
        {
            SyntaxInfo syntaxInfo = new SyntaxInfo(tree);

            List<string> code = new List<string>();
            foreach (SyntaxNodeInfo node in syntaxInfo.Nodes)
            {
                var functionTable = tableProvider(node);
                var codeGenerator = new CodeGenerator(functionTable);
                template.Accept(codeGenerator);
                code.Add(codeGenerator.GetCode());
            }

            return code;
        }

        public string GenerateTree(SyntaxTree tree)
        {
            IList<string> nodesCode = GenerateCodeForEachNode(tree, _templates["SyntaxNode"], TableStorage.GetSyntaxTable);
            var fileTable = TableStorage.GetSyntaxTreeFileTable(nodesCode);
            var fileGenerator = new CodeGenerator(fileTable);
            _templates["SyntaxTreeFile"].Accept(fileGenerator);

            return fileGenerator.GetCode();
        }

        public string GenerateWriter(SyntaxTree tree)
        {
            IList<string> writerBodyCode = GenerateCodeForEachNode(tree, _templates["SyntaxWriterBody"], TableStorage.GetSyntaxStreamTable);
            var fileTable = TableStorage.GetSyntaxWriterFileTable(writerBodyCode);
            var fileGenerator = new CodeGenerator(fileTable);
            _templates["SyntaxWriterFile"].Accept(fileGenerator);

            return fileGenerator.GetCode();
        }

        public string GenerateReader(SyntaxTree tree)
        {
            IList<string> readerBodyCode = GenerateCodeForEachNode(tree, _templates["SyntaxReaderBody"], TableStorage.GetSyntaxStreamTable);
            var fileTable = TableStorage.GetSyntaxReaderFileTable(new SyntaxInfo(tree), readerBodyCode);
            var fileGenerator = new CodeGenerator(fileTable);
            _templates["SyntaxReaderFile"].Accept(fileGenerator);

            return fileGenerator.GetCode();
        }
    }
}
