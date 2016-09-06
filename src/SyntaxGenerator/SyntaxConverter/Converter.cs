using NodeGenerator;
using SyntaxGenerator.SyntaxNodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyntaxConverter
{
    public class Converter
    {
        public static SyntaxTree FromBinaryFormat(IEnumerable<node_info> nodes)
        {
            List<SyntaxNode> newNodes = new List<SyntaxNode>();
            foreach (var node in nodes)
            {
                List<Field> fields = new List<Field>();
                foreach (var fieldInfo in node.subnodes)
                        fields.Add(new Field(
                            fieldInfo.field_name,
                            fieldInfo.field_type_name));

                newNodes.Add(
                    new SyntaxNode(
                        node.node_name,
                        node.base_class?.node_name,
                        fields));
            }

            return new SyntaxTree(newNodes);
        }
    }
}
