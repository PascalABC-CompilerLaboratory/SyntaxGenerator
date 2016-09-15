using SyntaxGenerator.SyntaxNodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyntaxGenerator.CodeGeneration
{
    public class SyntaxFunctionTable : AbstractSyntaxFunctionTable
    {
        public SyntaxFunctionTable(SyntaxNodeInfo node) : base(node)
        {
            // Здесь задаются имена функций, которые будут использованы в шаблоне
            // и соответствующие им функции
            functions["NodeName"] = NodeName;
            functions["BaseNodeName"] = BaseNodeName;
            functions["FieldType"] = FieldType;
            functions["FieldName"] = FieldName;
            functions["TypeComment"] = TypeComment;
            functions["SimpleFieldsCount"] = SimpleFieldsCount;
            functions["IntSequence"] = IntSequence;
            functions["InheritanceModifier"] = InheritanceModifier;
            functions["ListElementType"] = ListElementType;

            // Аналогично задаются условия
            conditions["HasInheritedFields"] = HasInheritedFields;
            conditions["HasFields"] = HasFields;
            conditions["NotSyntaxTreeNode"] = NotSyntaxTreeNode;
            conditions["SyntaxTreeNode"] = SyntaxTreeNode;
            conditions["HasSingleList"] = HasSingleList;
        }

        #region Functions
        public IEnumerable<string> NodeName(IFunctionParameters parameters)
        { 
            yield return node.Name;
        }

        public IEnumerable<string> BaseNodeName(IFunctionParameters parameters)
        {
            if (node.Base == null)
                yield break;

            yield return node.Base.Name;
        }

        public IEnumerable<string> FieldType(IFunctionParameters parameters)
        {
            var includeParents = parameters.GetValue<bool>("includeParents");
            var syntaxOnly = parameters.GetValue<bool>("syntaxOnly");
            var kind = parameters.GetValue<string>("kind");

            foreach (Field field in FilterFields(node, kind, includeParents, syntaxOnly))
                yield return field.Type;
        }

        public IEnumerable<string> FieldName(IFunctionParameters parameters)
        {
            var includeParents = parameters.GetValue<bool>("includeParents");
            var syntaxOnly = parameters.GetValue<bool>("syntaxOnly");
            var kind = parameters.GetValue<string>("kind");

            foreach (Field field in FilterFields(node, kind, includeParents, syntaxOnly))
                yield return field.Name;
        }

        public IEnumerable<string> ListElementType(IFunctionParameters parameters)
        {
            var includeParents = parameters.GetValue<bool>("includeParents");
            var syntaxOnly = parameters.GetValue<bool>("syntaxOnly");

            foreach (Field field in FilterFields(node, "List", includeParents, syntaxOnly))
                yield return field.ListElementType;
        }

        private IEnumerable<Field> FilterFields(SyntaxNodeInfo node, string kind, bool includeParents, bool syntaxOnly)
        {
            Predicate<Field> FieldSyntaxFilter = (Field field) =>
            {
                if (field.IsList)
                    return syntaxOnly ? node.SyntaxInfo.HasSyntaxNode(field.ListElementType) : true;
                else
                    return syntaxOnly ? node.SyntaxInfo.HasSyntaxNode(field.Type) : true;
            };

            Predicate<SyntaxNode> NodeFilter = 
                (SyntaxNode syntaxNode) =>
                // Исключаем из посещения узлы syntax_tree_node и declaration, 
                // если информация запрашивается не для этих узлов
                (syntaxNode.Name != "syntax_tree_node" || node.Name == "syntax_tree_node") && 
                (syntaxNode.Name != "declaration" || node.Name == "declaration");

            switch (kind)
            {
                case "Simple":
                    return node.GetFields(
                        includeParents,
                        NodeFilter,
                        field => FieldSyntaxFilter(field) && !field.IsList);
                case "List":
                    return node.GetFields(
                        includeParents,
                        NodeFilter,
                        field => FieldSyntaxFilter(field) && field.IsList);
                case "Any":
                    return node.GetFields(
                        includeParents,
                        NodeFilter,
                        FieldSyntaxFilter);
                default:
                    throw new ArgumentException(nameof(kind));
            }
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

        public IEnumerable<string> InheritanceModifier(IFunctionParameters parameters)
        {
            if (node.Name == "syntax_tree_node")
                yield return "virtual";
            else
                yield return "override";
        }

        public IEnumerable<string> SimpleFieldsCount(IFunctionParameters parameters)
        {
            yield return FilterFields(node, kind: "Simple", includeParents: true, syntaxOnly: true).Count().ToString();
        }

        public IEnumerable<string> IntSequence(IFunctionParameters parameters)
        {
            var from = parameters.GetValue<int>("from");
            return Enumerable.Range(from, int.MaxValue).Select(num => num.ToString());
        }
        #endregion

        #region Conditions
        public bool HasInheritedFields(IFunctionParameters parameters)
        {
            return FilterFields(node, kind: "Any", includeParents: true, syntaxOnly: false).Count() > node.Fields.Count();
        }

        public bool SyntaxTreeNode(IFunctionParameters parameters)
        {
            return node.Name == "syntax_tree_node" ? true : false;
        }

        public bool NotSyntaxTreeNode(IFunctionParameters parameters)
        {
            return node.Name == "syntax_tree_node" ? false : true;
        }

        public bool HasSingleList(IFunctionParameters parameters)
        {
            return node.Fields.Count(field => field.IsList) == 1;
        }

        public bool HasFields(IFunctionParameters parameters)
        {
            var includeParents = parameters.GetValue<bool>("includeParents");
            var syntaxOnly = parameters.GetValue<bool>("syntaxOnly");
            var kind = parameters.GetValue<string>("kind");

            return FilterFields(node, kind, includeParents, syntaxOnly).Count() > 0;
        }
        #endregion
    }
}
