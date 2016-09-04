using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Visitors
{
    public interface IVisitor<T>
    {
        T Visit(Assignment assignment);

        T Visit(CSharpCode code);

        T Visit(FormatString formatString);

        T Visit(FunctionCall funcCall);

        T Visit(SetStatement setStatement);

        T Visit(Template template);
    }
}
