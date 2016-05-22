using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class AssertDeleter : BaseChangeVisitor
    {
        public override void visit(procedure_call _procedure_call)
        {
            var methodCall = _procedure_call.func_name as method_call;
            if (methodCall != null && 
                string.Compare(methodCall.SimpleName, "Assert", false) == 0 &&
                methodCall.ParametersCount == 1)
            {
                UpperNodeAs<statement_list>()?.Remove(_procedure_call);
            }
        }
    }
}
