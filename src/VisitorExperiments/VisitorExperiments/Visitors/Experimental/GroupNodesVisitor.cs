using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class GroupNodesVisitor : BaseChangeVisitor
    {
        public override void Exit(syntax_tree_node st)
        {
            // filter
            if (!(st is procedure_definition))
            {
                base.Exit(st);
                return;
            }

            declarations procedureDefs = UpperNodeAs<declarations>();

            int curNameIdx = 0, firstMismatchIdx = 0;
            int definitionCount = procedureDefs.subnodes_count - procedureDefs.subnodes_without_list_elements_count;

            while (curNameIdx < definitionCount)
            {
                string curName = ProcedureName(procedureDefs.defs[curNameIdx]);
                // find first procedure name mistmatch
                while (firstMismatchIdx < definitionCount && ProcedureName(procedureDefs[firstMismatchIdx]) == curName)
                    ++firstMismatchIdx;

                // index of procedure definition with the same name, after mismatch
                int finderIdx = firstMismatchIdx + 1;

                while (finderIdx < definitionCount)
                {
                    // names match, swap finder and firstMismatch, refresh current
                    if (ProcedureName(procedureDefs[finderIdx]) == curName)
                    {
                        Swap(procedureDefs.defs, finderIdx, firstMismatchIdx);
                        curNameIdx = firstMismatchIdx + 1;
                        firstMismatchIdx = curNameIdx + 1;
                        break;
                    }
                    else
                        ++finderIdx;
                }

                // if the same procedure name was not found, then refresh current name index
                if (finderIdx >= definitionCount)
                    curNameIdx = firstMismatchIdx;
            }

            base.Exit(st);
        }

        private string ProcedureName(syntax_tree_node procedureDef)
        {
            return (procedureDef as procedure_definition).proc_header.name.meth_name.name;
        }
        
        private void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}
