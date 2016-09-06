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
        public SyntaxFunctionTable()
        {
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
            yield return node.Name;
        }

        public IEnumerable<string> BaseNodeName(IFunctionParameters parameters)
        {
            yield return node.Base.Name;
        }

        public IEnumerable<string> FieldType(IFunctionParameters parameters)
        {
            foreach (Field field in node.Fields)
                yield return field.Type;
        }

        public IEnumerable<string> FieldName(IFunctionParameters parameters)
        {
            foreach (Field field in node.Fields)
                yield return field.Name;
        }

        public IEnumerable<string> TypeComment(IFunctionParameters parameters)
        {
            var indent = parameters.GetValue<int>("indent");

            foreach (XmlElement element in node.TypeComment.Body)
            {
                string[] lines = element.OuterXml.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
                    yield return $"{indent}/// {line.TrimStart()}{Environment.NewLine}";
            }
        }
    }
}
