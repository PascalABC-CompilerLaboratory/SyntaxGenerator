using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;
using SyntaxGenerator.Visitors;

namespace SyntaxGenerator.CodeGeneration
{
    public class CodeGenerator : IVisitor
    {
        /// <summary>
        /// Значения выражений
        /// </summary>
        protected IFunctionTable functionTable;

        /// <summary>
        /// Сохраненные выражения
        /// </summary>
        protected Dictionary<string, IExpression> expressionAliases;

        /// <summary>
        /// Аккумулятор результата
        /// </summary>
        protected StringBuilder generatedCode;
        
        public void Visit(FormatString formatString)
        {
            var str = string.Join(formatString.Separator, formatString.Evaluate(functionTable));
            generatedCode.Append(str);
        }

        public void Visit(FunctionCall funcCall)
        {
            var str = string.Join(funcCall.Separator, funcCall.Evaluate(functionTable));
            generatedCode.Append(str);
        }

        public void Visit(SetStatement setStatement)
        {
            
        }

        public void Visit(Template template)
        {
            foreach (var codePart in template.Parts)
                codePart.Accept(this);
        }

        public void Visit(Assignment assignment)
        {
            expressionAliases[assignment.VariableName] = assignment.Value;
        }

        public void Visit(CSharpCode csharpCode)
        {
            generatedCode.Append(csharpCode.Code);
        }
    }
}
