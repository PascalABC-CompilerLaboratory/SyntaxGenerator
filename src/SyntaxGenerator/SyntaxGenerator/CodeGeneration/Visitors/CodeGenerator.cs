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
        /// Аккумулятор результата
        /// </summary>
        protected StringBuilder generatedCode = new StringBuilder();

        /// <summary>
        /// Хранит отступ для выражений
        /// </summary>
        protected string currentIndent;

        public CodeGenerator(IFunctionTable funcTable)
        {
            functionTable = funcTable;
        }

        public string GetCode()
        {
            var code = generatedCode.ToString();

            var lines = code.Lines();
            List<string> result = new List<string>();
            var ignoreWhitespace = false;
            foreach (var line in lines)
                if (!ignoreWhitespace)
                {
                    result.Add(line);
                    if (line.All(char.IsWhiteSpace))
                        ignoreWhitespace = true;
                }
                else
                {
                    if (!line.All(char.IsWhiteSpace))
                    {
                        result.Add(line);
                        ignoreWhitespace = false;
                    }
                }
            return string.Join(Environment.NewLine, result);
        }

        public void VisitFormatString(FormatString formatString)
        {
            var str = string.Join(formatString.Separator, (formatString.Evaluate(functionTable) as TextExpression).Values);
            var result = str.AddIndent(currentIndent);

            generatedCode.Append(result);
        }

        public void VisitFunctionalCall(FunctionCall funcCall)
        {
            var funcResult = functionTable.CallFunction(funcCall.Name, new FunctionParameters(funcCall.Parameters));
            var result = string.Join(funcCall.Separator, funcResult).AddIndent(currentIndent);

            generatedCode.Append(result);
        }

        public void VisitSetStatement(SetStatement setStatement)
        {
            
        }

        public void VisitTemplate(Template template)
        {
            foreach (var codePart in template.Parts)
                codePart.Accept(this);
        }

        public void VisitAssignment(Assignment assignment)
        {
            functionTable.AddExpressionAlias(assignment.VariableName, assignment.Value);
        }

        public void VisitCSharpCode(CSharpCode csharpCode)
        {
            currentIndent = csharpCode.Indent;
            string code = csharpCode.Code;
            code = code.Substring(0, code.Length - currentIndent.Length);
            generatedCode.Append(code);
        }

        public void VisitIfStatement(IfStatement ifStatement)
        {
            if (ifStatement.CheckCondition(functionTable))
                foreach (ICodePart part in ifStatement.Body)
                    part.Accept(this);
        }
    }
}
