using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.SyntaxNodes.Model;

namespace SyntaxGenerator.CodeGeneration
{
    /// <summary>
    /// Предоставляет информацию об узлах, необходимую для генерации кода
    /// </summary>
    public class SyntaxInfo
    {
        private List<SyntaxNodeInfo> _nodesInfo = new List<SyntaxNodeInfo>();

        /// <summary>
        /// Имена узлов
        /// </summary>
        private HashSet<string> _nodeNames = new HashSet<string>();

        /// <summary>
        /// Список узлов
        /// </summary>
        public IList<SyntaxNodeInfo> Nodes => _nodesInfo;
            
        private SyntaxNodeInfo BuildSyntaxNodeInfo(SyntaxNode node, SyntaxTree tree)
        {
            return new SyntaxNodeInfo(
                    node,
                    node.BaseName == null ?
                    null :
                    BuildSyntaxNodeInfo(tree.Nodes.Find(baseNode => baseNode.Name == node.BaseName), tree),
                    this);
        }

        public SyntaxInfo(SyntaxTree tree)
        {
            foreach (SyntaxNode node in tree.Nodes)
            {
                _nodeNames.Add(node.Name);
                _nodesInfo.Add(BuildSyntaxNodeInfo(node, tree));
            }
        }

        /// <summary>
        /// Позволяет выяснить, существует ли синтаксический узел с заданным названием
        /// </summary>
        /// <param name="type">Название типа</param>
        /// <returns>Является ли синтаксическим узлом</returns>
        public bool HasSyntaxNode(string type) => _nodeNames.Contains(type);
    }
}
