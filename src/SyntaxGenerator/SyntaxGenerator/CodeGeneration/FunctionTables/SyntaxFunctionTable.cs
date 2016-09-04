using SyntaxGenerator.SyntaxNodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyntaxGenerator.CodeGeneration
{
    public class SyntaxFunctionTable : FunctionTable
    {
        private SyntaxInfo _syntaxInfo;

        private SyntaxNode CurrentNode => _syntaxInfo.CurrentNode;

        private List<SyntaxNode> Nodes => _syntaxInfo.SyntaxTree.Nodes;

        public SyntaxFunctionTable(SyntaxInfo info)
        {
            _syntaxInfo = info;

            // Здесь задаются имена функций, которые будут использованы в шаблоне
            // и соответствующие им функции
            functions["NodeName"] = NodeName;
            functions["BaseNodeName"] = BaseNodeName;
            functions["FieldType"] = FieldType;
            functions["FieldName"] = FieldName;
            functions["TypeComment"] = TypeComment;
        }
        
        public IEnumerable<string> NodeName(IFunctionParameters parameters)
        { 
            yield return CurrentNode.Name;
        }

        public IEnumerable<string> BaseNodeName(IFunctionParameters parameters)
        {
            yield return CurrentNode.Base;
        }

        public IEnumerable<string> FieldType(IFunctionParameters parameters)
        {
            foreach (Field field in CurrentNode.Fields)
                yield return field.Type;
        }

        public IEnumerable<string> FieldName(IFunctionParameters parameters)
        {
            foreach (Field field in CurrentNode.Fields)
                yield return field.Name;
        }

        public IEnumerable<string> TypeComment(IFunctionParameters parameters)
        {
            foreach (XmlElement element in CurrentNode.TypeComment.Body)
            {
                string[] lines = element.OuterXml.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
                    yield return "/// " + line.TrimStart();
            }
        }
    }
}
