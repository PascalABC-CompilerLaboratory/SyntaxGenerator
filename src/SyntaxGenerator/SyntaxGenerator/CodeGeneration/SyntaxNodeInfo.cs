using SyntaxGenerator.SyntaxNodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyntaxGenerator.CodeGeneration
{
    /// <summary>
    /// Обертка над <see cref="SyntaxNode"/>, содержащая дополнительную информацию
    /// </summary>
    public class SyntaxNodeInfo
    {
        /// <summary>
        /// Узел, оберткой для которого служит данный класс
        /// </summary>
        private SyntaxNode _node;

        /// <summary>
        /// Информация обо всех узлах
        /// </summary>
        private SyntaxInfo _syntaxInfo;

        /// <summary>
        /// Информация о базовом классе
        /// </summary>
        private SyntaxNodeInfo _baseNodeInfo;

        public string Name => _node.Name;

        public SyntaxNodeInfo Base => _baseNodeInfo;

        public IEnumerable<Field> Fields => _node.Fields;

        public Comment TypeComment => _node.TypeComment;

        public SyntaxInfo SyntaxInfo => _syntaxInfo;

        public SyntaxNodeInfo(SyntaxNode node, SyntaxNodeInfo baseNode, SyntaxInfo info)
        {
            _node = node;
            _baseNodeInfo = baseNode;
            _syntaxInfo = info;
        }

        /// <summary>
        /// Возвращает список узлов в дереве наследования от текущего узла к корню
        /// </summary>
        /// <param name="node">Конечный узел в иерархии</param>
        /// <param name="fromRootToSelf">Список формируется от корня к заданному узлу</param>
        /// <param name="includeSelf">Включать заданный узел в список</param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> GetInheritanceChain(bool fromRootToSelf = false, bool includeSelf = true)
        {
            var chain = GetInheritanceChainImpl();
            if (!includeSelf)
                chain = chain.Skip(1);
            if (fromRootToSelf)
                chain = chain.Reverse();

            return chain;
        }

        /// <summary>
        /// Возвращает список узлов в дереве наследования в порядке от текущего узла к корню
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SyntaxNode> GetInheritanceChainImpl()
        {
            var node = this;
            while (node != null)
            {
                yield return node._node;
                node = node._baseNodeInfo;
            }
        }
    }
}
