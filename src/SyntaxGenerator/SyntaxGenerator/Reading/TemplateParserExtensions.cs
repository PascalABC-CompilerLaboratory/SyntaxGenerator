using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateParser;
using TemplateParser.Extensions;
using static System.Char;
using static TemplateParser.ParserHelper;

namespace SyntaxGenerator.Reading
{
    static internal class ParserExtensionsForTemplates
    {
        static internal T ReadIdentifier<T>(this T parser)
            where T : Parser
        {
            return parser.ReadChar(IsLetter).ReadWhile(IsLetterOrDigit);
        }

        static internal T ReadExpression<T>(this T parser, Action<Expression> expressionProcessor)
            where T : Parser
        {
            parser
                .Optional()
                    .ReadString(s => expressionProcessor(s))
                .Merge();

            return parser;
        }

        static internal T ReadString<T>(this T parser, Action<FormatString> stringProcessor)
            where T : Parser
        {
            int argumentCounter = 0;
            StringBuilder format = new StringBuilder();
            var formatString = new FormatString();
            bool endOfString = false;

            parser.ReadChar('"');
            while (!endOfString && parser.IsSucceed)
            {
                // Читаем, пока не встретим '{', '}' или конец строки
                var optional = parser
                    .BeginAccumulation()
                    .ReadWhile(Not(AnyOf('"', '\\', '{', '}')))
                    .EndAccumulation(part => format.Append(part))
                    .Optional();

                // '\}' переводим в '}', '\{' в '{'
                if (optional.ReadString(@"\{").IsSucceed)
                    format.Append('{');
                else
                if (optional.Or().ReadString(@"\}").IsSucceed)
                    format.Append('}');
                else
                // Если это конец строки, то завершаем чтение
                if (optional.Or().ReadChar('"').IsSucceed)
                    endOfString = true;
                else
                {
                    // пытаемся прочесть {Expression}
                    Expression expression = null;
                    bool isInterpolation = optional
                        .Or()
                        .ReadChar('{')
                        .ReadWhile(IsWhiteSpace)
                        .ReadExpression(expr => expression = expr)
                        .ReadWhile(IsWhiteSpace)
                        .ReadChar('}')
                        .IsSucceed;

                    if (isInterpolation)
                    {
                        format.Append("{" + (argumentCounter++).ToString() + "}");
                        formatString.Arguments.Add(expression);
                    }
                }

                parser = optional.Merge();
            }

            formatString.Format = format.ToString();
            stringProcessor(formatString);
            return parser;
        }
    }
}
