using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Char;

namespace SyntaxGenerator.SyntaxNodes.Model
{
    public class Field
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Type;

        public bool IsList => Type.StartsWith("List<");

        public string ListElementType => IsList ? 
            string.Concat(Type.Skip(5).TakeWhile(c => c != '>')) : 
            "";

        public Field() { }

        public Field(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
