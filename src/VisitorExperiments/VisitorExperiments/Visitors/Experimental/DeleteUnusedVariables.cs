using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;
using VisitorExperiments.Utility;

namespace SyntaxVisitors
{
    public class DeleteUnusedVariables : CollectNamespaceVisitor
    {
        Dictionary<Namespace, HashSet<string>> variablesForRemoving = new Dictionary<Namespace, HashSet<string>>();

        public DeleteUnusedVariables()
        {
            variablesForRemoving[CurrentNamespace] = new HashSet<string>();
        }

        public override void NamespaceChanged(Namespace newNamespace)
        {
            if (!variablesForRemoving.ContainsKey(newNamespace))
                variablesForRemoving[newNamespace] = new HashSet<string>();
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);

            // visit only leftmost ident
            if (st is dot_node)
            {
                string variableName = ((st as dot_node).left as ident)?.name;
                if (variableName != null)
                    variablesForRemoving[CurrentNamespace].Add(variableName);

                visitNode = false;
            }

            // add all declared variables and prevent node visiting
            // because of visit(ident)
            if (st is var_def_statement)
            {
                HashSet<string> currentVariablesForRemoving = variablesForRemoving[CurrentNamespace];

                foreach (ident ident in (st as var_def_statement).vars.idents)
                    currentVariablesForRemoving.Add(ident.name);

                visitNode = false;
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is block)
            {
                // Before exiting block, delete unused variables 
                var unused = variablesForRemoving[CurrentNamespace];
                if (unused.Count > 0)
                {
                    LimitedLocalDefDeleter deleteVisitor = new LimitedLocalDefDeleter(unused, 0);
                    st.visit(deleteVisitor);
                }
            }

            base.Exit(st);
        }

        public override void visit(ident _ident)
        {
            // if we are here, then _ident 
            Namespace currentNamespace = CurrentNamespace;

            while (currentNamespace != null)
            {
                HashSet<string> currentVariablesForRemoving = variablesForRemoving[currentNamespace];
                if (currentVariablesForRemoving.Contains(_ident.name))
                {
                    currentVariablesForRemoving.Remove(_ident.name);
                    break;
                }

                currentNamespace = currentNamespace.ParentNamespace;
            }
        }
    }
}
