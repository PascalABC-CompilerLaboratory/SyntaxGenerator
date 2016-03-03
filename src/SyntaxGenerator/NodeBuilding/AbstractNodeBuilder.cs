using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyntaxGenerator.NodeBuilding
{
    public abstract class AbstractNodeBuilder
    {
        public abstract void AppendNode(StreamWriter writer);
    }
}
