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
        void VisitAssignment(Assignment assignment);

        void VisitCSharpCode(CSharpCode code);

        void VisitFormatString(FormatString formatString);

        void VisitFunctionalCall(FunctionCall funcCall);

        void VisitSetStatement(SetStatement setStatement);

        void VisitTemplate(Template template);

        void VisitIfStatement(IfStatement ifStatement);
    }
}
