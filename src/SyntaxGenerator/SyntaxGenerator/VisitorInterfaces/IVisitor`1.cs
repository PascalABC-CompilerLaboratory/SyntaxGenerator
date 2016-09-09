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
        T VisitAssignment(Assignment assignment);

        T VisitCSharpCode(CSharpCode code);

        T VisitFormatString(FormatString formatString);

        T VisitFunctionCall(FunctionCall funcCall);

        T VisitSetStatement(SetStatement setStatement);

        T VisitTemplate(Template template);

        T VisitIfStatement(IfStatement ifStatement);
    }
}
