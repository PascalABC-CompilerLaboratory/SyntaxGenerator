using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SyntaxGenerator.SyntaxNodes.Model
{
    [XmlRoot]
    public class SyntaxTree
    {
        [XmlElement(ElementName = "SyntaxNode", Type = typeof(SyntaxNode))]
        public List<SyntaxNode> Nodes;

        public SyntaxTree(IEnumerable<SyntaxNode> nodes)
        {
            Nodes = new List<SyntaxNode>(nodes);
        }
    }
}
