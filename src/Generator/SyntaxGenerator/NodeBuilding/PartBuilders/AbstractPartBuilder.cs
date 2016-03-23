using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.NodeInfo;
using System.IO;

namespace SyntaxGenerator.NodeBuilding.PartBuilders
{
    public abstract class AbstractPartBuilder
    {
        protected SyntaxNodeInfo nodeInfo;
        protected int indent;

        public AbstractPartBuilder() { }

        public AbstractPartBuilder(SyntaxNodeInfo node, int indent)
        {
            nodeInfo = node;
            this.indent = indent;
        }

        public abstract void AppendCode(StreamWriter writer);
    }
}
