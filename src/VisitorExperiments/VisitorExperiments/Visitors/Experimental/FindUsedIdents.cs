using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class FindUsedIdents : BaseEnterExitVisitor
    {
        HashSet<string> usedIdents = new HashSet<string>();

        public HashSet<string> UsedIdents { get { return usedIdents; } }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);

            // Skip variable definitions
            if (st is variable_definitions || st is var_def_statement)
                visitNode = false;

            // Take from the dot node only leftmost ident
            if (st is dot_node)
            {
                dot_node dotNode = st as dot_node;
                if (dotNode.left is ident)
                {
                    usedIdents.Add((dotNode.left as ident).name);
                    visitNode = false;
                }
            }
        }

        public override void visit(ident _ident)
        {
            usedIdents.Add(_ident.name);
        }
    }
}
