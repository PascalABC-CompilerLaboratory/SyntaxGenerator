using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Visitors
{
    public interface IVisitor
    {
        void Visit(AbstractNode abstractNode);

        void Visit(Assignment assignment);

        void Visit(FormatString formatString);

        void Visit(Identifier identifier);

        void Visit(FunctionCall funcCall);

        void Visit(QualifiedIdentifier identifier);

        void Visit(SetStatement setStatement);

        void Visit(Template template);
    }
}
