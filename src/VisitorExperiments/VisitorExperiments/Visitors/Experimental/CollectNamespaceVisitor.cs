using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;
using VisitorExperiments.Utility;
using System.Diagnostics;

namespace SyntaxVisitors
{
    public delegate void OnNamespaceChangedDelegate(Namespace newNamespace);

    /// <summary>
    /// Provides current namespace while traversing a syntax tree
    /// </summary>
    public class CollectNamespaceVisitor : CollectUpperNodesVisitor
    {
        protected OnNamespaceChangedDelegate OnNamespaceChanged;

        private Namespace currentNamespace;

        protected Namespace CurrentNamespace
        {
            get { return currentNamespace; }

            private set
            {
                currentNamespace = value;
                OnNamespaceChanged?.Invoke(currentNamespace);
            }
        }

        public CollectNamespaceVisitor()
        {
            CurrentNamespace = new Namespace("Root");
            OnNamespaceChanged = NamespaceChanged;
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);

            if (st is procedure_definition)
            {
                var procedureDefinition = st as procedure_definition;

                string procedureName = 
                    (procedureDefinition.proc_header as function_header)?.name.meth_name.name ??
                    (procedureDefinition.proc_header as procedure_header)?.name.meth_name.name;

                Debug.Assert(procedureName != null);

                CurrentNamespace = new Namespace(CurrentNamespace, procedureName);
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is procedure_definition)
                CurrentNamespace = CurrentNamespace.ParentNamespace;

            base.Exit(st);
        }

        public virtual void NamespaceChanged(Namespace newNamespace)
        {

        }
    }
}
