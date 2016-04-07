using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class FindMainIdentsVisitor : WalkingVisitorNew
    {
        public ISet<ident> vars = new HashSet<ident>();
        public override void visit(ident id)
        {
            vars.Add(id);
        }
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }
    }
}
