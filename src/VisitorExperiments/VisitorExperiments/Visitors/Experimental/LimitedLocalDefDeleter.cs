using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class LimitedLocalDefDeleter : DeleteLocalDefs
    {
        int maxBlockDepth;
        int currentBlockDepth = 0;

        public LimitedLocalDefDeleter(HashSet<string> idsToDelete, int maxBlockDepth = 1) : base(idsToDelete)
        {
            this.maxBlockDepth = maxBlockDepth;
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);

            if (st is block)
            {
                if (currentBlockDepth < maxBlockDepth)
                    ++currentBlockDepth;
                else
                    visitNode = false;
            }       
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is block)
                --currentBlockDepth;

            base.Exit(st);
        }
    }
}
