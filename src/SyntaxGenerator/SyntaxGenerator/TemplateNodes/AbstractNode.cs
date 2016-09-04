using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public interface IAbstractNode
    {
        void Accept(IVisitor visitor);

        T Accept<T>(IVisitor<T> visitor);
    }
}
