using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SyntaxGenerator.SyntaxNodes.Model
{
    public class Field
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Type;

        public Field() { }

        public Field(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
