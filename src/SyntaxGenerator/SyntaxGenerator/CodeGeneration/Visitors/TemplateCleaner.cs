using SyntaxGenerator.TemplateNodes;
using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Char;

namespace SyntaxGenerator.CodeGeneration.Visitors
{
    public class TemplateCleaner : IVisitor
    {
        private Template _template;
        bool deleteNode = false;

        public TemplateCleaner(Template template)
        {
            _template = template;
        }

        public static Template Visit(Template template)
        {
            var visitor = new TemplateCleaner(template);
            template.Accept(visitor);
            return visitor._template;
        }

        public void VisitFormatString(FormatString formatString)
        {
            throw new NotImplementedException();
        }

        public void VisitSetStatement(SetStatement setStatement)
        {
            throw new NotImplementedException();
        }

        public void VisitIfStatement(IfStatement ifStatement)
        {
            throw new NotImplementedException();
        }

        public void VisitTemplate(Template template)
        {
            // Убираем лишние строки в CSharpCode после операторов, исключая if
            for (int i = 0; i < template.Parts.Count - 1; i++)
            {
                if (template.Parts[i] is IStatement && 
                    template.Parts[i].GetType() != typeof(IfStatement) &&
                    template.Parts[i + 1] is CSharpCode)
                {
                    VisitCSharpCode(template.Parts[i + 1] as CSharpCode);
                    if (deleteNode)
                    {
                        template.Parts.RemoveAt(i + 1);
                        deleteNode = false;
                    }
                }
            }
        }

        public void VisitFunctionalCall(FunctionCall funcCall)
        {
            throw new NotImplementedException();
        }

        public void VisitCSharpCode(CSharpCode code)
        {
            if (code.Code.All(IsWhiteSpace))
            {
                deleteNode = true;
                return;
            }

            code.Code = string.Concat(code.Code
                .SkipWhile(c => c != '\r' && c != '\n')
                .SkipWhile(c => c == '\r' || c == '\n'));
        }

        public void VisitAssignment(Assignment assignment)
        {
            throw new NotImplementedException();
        }
    }
}
