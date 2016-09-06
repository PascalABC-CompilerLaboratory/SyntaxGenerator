using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SyntaxGenerator.SyntaxNodes.Model
{
    public class SyntaxNode
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string BaseName;

        [XmlElement]
        public Comment TypeComment;

        [XmlElement(ElementName = "Field", Type = typeof(Field))]
        public List<Field> Fields;

        public SyntaxNode() { }

        public SyntaxNode(string name, string baseName, IEnumerable<Field> fields = null)
        {
            Name = name;
            BaseName = baseName;
            if (fields != null)
                Fields = new List<Field>(fields);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
