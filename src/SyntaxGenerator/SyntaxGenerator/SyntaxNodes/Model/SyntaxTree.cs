using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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

        public SyntaxTree() { }

        public SyntaxTree(IEnumerable<SyntaxNode> nodes)
        {
            Nodes = new List<SyntaxNode>(nodes);
        }

        public static SyntaxTree Deserialize(string path)
        {
            SyntaxTree tree = new XmlSerializer(typeof(SyntaxTree)).
                Deserialize(File.OpenRead(path)) as SyntaxTree;
            return tree;
        }
    }
}
