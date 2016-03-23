using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyntaxGenerator.NodeBuilding
{
    public class NodesCollectionBuilder
    {
        string usings = "using System;" + Environment.NewLine +
                        "using System.Collections;" + Environment.NewLine +
                        "using System.Collections.Generic;" + Environment.NewLine;

        private List<AbstractNodeBuilder> _nodeBuilders;

        public void AppendNodes(StreamWriter writer)
        {
            foreach (AbstractNodeBuilder builder in _nodeBuilders)
                builder.AppendNode(writer);
        }

        private void AddPreCode(StreamWriter writer)
        {

        }
    }
}
