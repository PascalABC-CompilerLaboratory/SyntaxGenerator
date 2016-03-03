using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.NodeInfo
{
    public class SyntaxNodeInfo
    {
        private string _baseNodeName;
        private string _nodeName;
        private List<FieldInfo> _fields;
        
        public SyntaxNodeInfo(string nodeName, string baseNodeName)
        {
            _baseNodeName = baseNodeName;
            _nodeName = nodeName;
            _fields = new List<FieldInfo>();
        }

        public string BaseNodeName
        {
            get { return _baseNodeName; }
        }

        public string NodeName
        {
            get { return _nodeName; }
        }

        public IReadOnlyList<FieldInfo> Fields
        {
            get { return _fields; }
        }

        public void AddField(FieldInfo field)
        {
            _fields.Add(field);
        }
    }

    public class FieldInfo
    {
        private string _type;
        private string _modifiers;
        private string _name;

        public FieldInfo(string modifiers, string type, string name)
        {
            _type = type;
            _modifiers = modifiers;
            _name = name;
        }

        public string Type
        {
            get { return _type; }
        }

        public string Modifiers
        {
            get { return _modifiers; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
