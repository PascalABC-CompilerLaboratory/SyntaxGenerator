using SyntaxGenerator.SyntaxNodes.Model;
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

        public SyntaxTreeGenerator(TemplateStorage templates)
        {
            _templates = templates;
        }

        public string Generate(SyntaxTree tree)
        {
            SyntaxInfo syntaxInfo = new SyntaxInfo(tree);

            List<string> nodesCode = new List<string>();
            foreach (SyntaxNodeInfo node in syntaxInfo.Nodes)
            {
                var functionTable = TableStorage.GetSyntaxTable(node);
                var codeGenerator = new CodeGenerator(functionTable);
                _templates["SyntaxNode"].Accept(codeGenerator);
                nodesCode.Add(codeGenerator.GetCode());
            }

            var fileTable = TableStorage.GetSyntaxTreeFileTable(nodesCode);
            var fileGenerator = new CodeGenerator(fileTable);
            _templates["SyntaxTreeFile"].Accept(fileGenerator);

            return fileGenerator.GetCode();
        }
    }
}
