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
        /// <summary>
        /// Все узлы
        /// </summary>
        public SyntaxTree SyntaxTree { get; }

        /// <summary>
        /// Узел, для которго в данный момент генерируется код
        /// </summary>
        public SyntaxNode CurrentNode { get; set; }

        public SyntaxInfo(SyntaxTree tree)
        {
            SyntaxTree = tree;
        }
    }
}
