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
        static internal T ReadIdentifier<T>(this T parser, Action<string> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                    .BeginAccumulation()
                    .ReadChar(IsLetter)
                    .ReadWhile(IsLetterOrDigit)
                    .EndAccumulation(processor);
        }

        #region Expressions

        /// <summary>
        /// Читает выражение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadExpression<T>(this T parser, Action<Expression> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                    .Optional()
                        .ReadString(processor)
                    .Or()
                        .ReadQualifiedIdentifier(processor)
                    .Or()
                        .ReadIdentifier(processor)
                    .Merge();
        }

        static internal T ReadIdentifier<T>(this T parser, Action<Identifier> processor)
            where T : Parser
            => parser.ReadIdentifier((string s) => processor(new Identifier(s)));

        /// <summary>
        /// Читает идентификатор вида &lt;Identifier&gt;.&lt;Identifier&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor">Обработчик идентификатора</param>
        /// <returns></returns>
        static internal T ReadQualifiedIdentifier<T>(this T parser, Action<QualifiedIdentifier> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            Identifier qualifier = null;
            return parser
                .ReadIdentifier(id => qualifier = id)
                .ReadChar('.')
                .ReadIdentifier(id => processor(new QualifiedIdentifier(qualifier, id)));
        }

        /// <summary>
        /// Читает строку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="stringProcessor">Обработчик строки</param>
        /// <returns></returns>
        /// <remarks> 
        /// В строке могут быть использованы значения выражений. 
        /// Выражения берутся в фигурные скобки, например, "A = {ValueA}". 
        /// Фигурные скобки экранируются символом '\',
        /// например, "object A \{ get; set; \}".
        /// </remarks>
        static internal T ReadString<T>(this T parser, Action<FormatString> stringProcessor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

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
                        .SkipWhiteSpaces()
                        .ReadExpression(expr => expression = expr)
                        .SkipWhiteSpaces()
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

            if (parser.IsSucceed)
            {
                formatString.Format = format.ToString();
                stringProcessor(formatString);
            }

            return parser;
        }

        #endregion

        #region Parameters

        /// <summary>
        /// Читает параметр
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadParameter<T>(this T parser, Action<Parameter> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                    .Optional()
                        .ReadJoinParameter(processor)
                    .Merge();
        }

        /// <summary>
        /// Читает 'join &lt;expression&gt;'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor">Обработчик разделителя</param>
        /// <returns></returns>
        static internal T ReadJoinParameter<T>(this T parser, Action<Separator> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                .ReadString(Lexems.JoinKeyword)
                .SkipWhiteSpaces()
                .ReadExpression(expr => processor(new Separator(expr)));
        }

        #endregion

        #region CodeParts

        static internal T ReadCSharpCode<T>(this T parser, Action<CSharpCode> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                .BeginAccumulation()
                .ReadUntil(Lexems.TemplateOpenSymbol)
                .EndAccumulation(s => processor(new CSharpCode(s)));
        }

        static internal T ReadParametrizedExpression<T>(this T parser, Action<ParameterizedExpression> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            var expressionWithParams = new ParameterizedExpression();
            parser
                .ReadExpression(expr => expressionWithParams.Expression = expr)
                .SkipWhiteSpaces();
            
            while (parser.IsSucceed)
            {
                // Пытаемся прочесть параметр
                var optional = parser
                    .Optional()
                        .ReadParameter(param => expressionWithParams.AddParameter(param))
                        .SkipWhiteSpaces();

                // Если удается, сохраняем позицию и продолжаем работу
                if (optional.IsSucceed)
                    optional.Merge();
                // Иначе - возвращаемся к началу и прекращаем чтение
                else
                {
                    optional.Or().Merge();
                    break;
                }
            }

            if (parser.IsSucceed)
                processor(expressionWithParams);

            return parser;
        }

        static internal T ReadTemplateCode<T>(this T parser, Action<TemplateCode> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                .ReadString(Lexems.TemplateOpenSymbol)
                .SkipWhiteSpaces()
                .Optional()
                    .ReadParametrizedExpression(processor)
                .Or()
                    .ReadStatement(processor)
                .Merge()
                .SkipWhiteSpaces()
                .ReadString(Lexems.TemplateCloseSymbol);
        }

        #endregion

        #region Statements

        /// <summary>
        /// Читает оператор 
        /// </summary> 
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadStatement<T>(this T parser, Action<Statement> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                    .Optional()
                        .ReadAssignment(processor)
                    .Or()
                        .ReadSetStatement(processor)
                    .Merge();
        }

        /// <summary>
        /// Читает присваивание 
        /// <para/>
        /// 'set &lt;VariableName&gt; = &lt;Value&gt;'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadAssignment<T>(this T parser, Action<Assignment> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            var assignment = new Assignment();
            parser
                .ReadIdentifier(varName => assignment.VariableName = varName)
                .SkipWhiteSpaces()
                .ReadString(Lexems.AssignSymbol)
                .SkipWhiteSpaces()
                .ReadExpression(value => assignment.Value = value);

            if (parser.IsSucceed)
                processor(assignment);

            return parser;
        }

        /// <summary>
        /// Читает оператор установки значения зарезервированной переменной
        /// <para/>
        /// 'set &lt;VariableName&gt; = &lt;Value&gt;'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadSetStatement<T>(this T parser, Action<SetStatement> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            var setStatement = new SetStatement();
            parser
                .ReadString(Lexems.SetKeyword)
                .SkipWhiteSpaces()
                .ReadIdentifier(varName => setStatement.VariableName = varName)
                .SkipWhiteSpaces()
                .ReadString(Lexems.AssignSymbol)
                .SkipWhiteSpaces()
                .ReadExpression(value => setStatement.Value = value);

            if (parser.IsSucceed)
                processor(setStatement);

            return parser;
        }

        #endregion

        static internal T ReadTemplate<T>(this T parser, Action<Template> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            var template = new Template();

            // Читаем тип файла
            parser
                .SkipWhiteSpaces()
                .ReadString(Lexems.TemplateOpenSymbol)
                .SkipWhiteSpaces()
                .ReadSetStatement(fileType => template.AddPart(fileType))
                .SkipWhiteSpaces()
                .ReadString(Lexems.TemplateCloseSymbol)
                .SkipWhiteSpaces();
                
            // Разбиваем файл на части кода C# и языка генерации
            while (!parser.AtEnd && parser.IsSucceed)
            {
                parser.ReadCSharpCode(part => template.AddPart(part));

                if (!parser.AtEnd)
                    parser.ReadTemplateCode(part => template.AddPart(part));
            }

            if (parser.IsSucceed)
                processor(template);

            return parser;
        }
    }
}
