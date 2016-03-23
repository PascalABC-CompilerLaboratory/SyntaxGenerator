using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.NodeInfo;
using SyntaxGenerator.NodeBuilding.PartBuilders;
using System.IO;

namespace SyntaxGenerator.NodeBuilding
{
    public class PartNodeBuilder : AbstractNodeBuilder
    {
        private SyntaxNodeInfo _syntaxNode;
        private List<AbstractPartBuilder> _nodePartBuilders;
        private int _indent;

        public PartNodeBuilder() { }

        public PartNodeBuilder(SyntaxNodeInfo node, int indent = 0)
        {
            _syntaxNode = node;
            _indent = indent;
            _nodePartBuilders = new List<AbstractPartBuilder>();
        }

        public void AddBuilder(AbstractPartBuilder builder)
        {
            _nodePartBuilders.Add(builder);
        }

        public override void AppendNode(StreamWriter writer)
        {
            OpenClass(writer);
            foreach (AbstractPartBuilder builder in _nodePartBuilders)
                builder.AppendCode(writer);
            CloseClass(writer);
        }

        /// <summary>
        /// Записывает заголовок синтаксического узла
        /// </summary>
        private void OpenClass(StreamWriter writer)
        {
            string indentStr = new string(' ', _indent);
            writer.WriteLine(indentStr + "[Serializable]");
            writer.WriteLine(string.Format("{0}public class {1} : {2}", indentStr, _syntaxNode.NodeName, _syntaxNode.BaseNodeName));
            writer.WriteLine(indentStr + "{");
        }

        /// <summary>
        /// Записывает посткод синтаксического узла
        /// </summary>
        /// <param name=""></param>
        private void CloseClass(StreamWriter writer)
        {
            string indentStr = new string(' ', _indent);
            writer.WriteLine(indentStr + "}");
        }
    }
}
