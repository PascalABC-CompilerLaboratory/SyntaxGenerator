using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace VisitorExperiments.Utility
{
    public static class StatementListExtensions
    {
        public static void ForEach<TNode>(this IEnumerable<TNode> nodes, Action<TNode> action)
            where TNode : syntax_tree_node
        {
            foreach (var node in nodes)
                action(node);
        }
    }
}
