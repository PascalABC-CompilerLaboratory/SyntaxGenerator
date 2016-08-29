using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public static class TemplateNodeExtensions
    {
        public static void Accept<T>(this T node, IVisitor visitor)
            where T : AbstractNode
        {
            visitor.Visit(node);
        }
    }
}
