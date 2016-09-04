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
        void Visit(Assignment assignment);

        void Visit(CSharpCode code);

        void Visit(FormatString formatString);

        void Visit(FunctionCall funcCall);

        void Visit(SetStatement setStatement);

        void Visit(Template template);
    }
}
