﻿using SyntaxGenerator.TemplateNodes;
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
        static internal T ReadExpression<T>(this T parser, Action<IExpression> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            IExpression expression = null;
            parser
                .Optional()
                    .ReadInterpolatedString(str => expression = str)
                .Or()
                    .ReadFunctionCall(funcCall => expression = funcCall)
                .Merge()
                .Optional()
                    .ReadJoinParameter(separator => expression.Separator = separator)
                .Or().Merge();

            if (parser.IsSucceed)
                processor(expression);

            return parser;
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
        static internal T ReadInterpolatedString<T>(this T parser, Action<FormatString> stringProcessor)
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
                // Читаем, пока не встретим выражение, экранирование или конец строки
                var optional = parser
                    .BeginAccumulation()
                    .ReadWhile(Not(AnyOf('"', '\\', '{', '}')))
                    .EndAccumulation(part => format.Append(part))
                    .Optional();

                // Читаем экранированные символы
                if (optional.ReadString(@"\{").IsSucceed)
                    format.Append('{');
                else
                if (optional.Or().ReadString(@"\}").IsSucceed)
                    format.Append('}');
                else
                if (optional.Or().ReadEscapeCharacter(ch => format.Append(ch)).IsSucceed)
                    ;
                else
                // Если это конец строки, то завершаем чтение
                if (optional.Or().ReadChar('"').IsSucceed)
                    endOfString = true;
                else
                {
                    // пытаемся прочесть {Expression}
                    IExpression expression = null;
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

        /// <summary>
        /// Читает 'join &lt;string&gt;'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor">Обработчик разделителя</param>
        /// <returns></returns>
        static internal T ReadJoinParameter<T>(this T parser, Action<string> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                .ReadString(Lexems.JoinKeyword)
                .SkipWhiteSpaces()
                .ReadString(processor);
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
        static internal T ReadParameter<T>(this T parser, Action<IParameter> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            string parameterName = null;
            IParameter parameter = null;
            parser
                .ReadIdentifier(name => parameterName = name)
                .SkipWhiteSpaces()
                .ReadString(Lexems.Colon)
                .SkipWhiteSpaces()
                .ReadString(str => parameter = new Parameter<string>(parameterName, str));

            if (parser.IsSucceed)
                processor(parameter);

            return parser;
        }

        /// <summary>
        /// Читает строку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadString<T>(this T parser, Action<string> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            StringBuilder result = new StringBuilder();
            parser.ReadChar('"');
            while (parser.IsSucceed)
            {
                // Читаем, пока не встретим выражение, экранирование или конец строки
                var optional = parser
                    .BeginAccumulation()
                    .ReadWhile(Not(AnyOf('"', '\\')))
                    .EndAccumulation(part => result.Append(part))
                    .Optional();

                // Конец строки
                if (optional.ReadChar('"').IsSucceed)
                {
                    optional.Merge();
                    break;
                }

                // Читаем экранированные символы
                optional.Or().ReadEscapeCharacter(ch => result.Append(ch));

                optional.Merge();
            }

            if (parser.IsSucceed)
                processor(result.ToString());

            return parser;
        }

        static private T ReadEscapeCharacter<T>(this T parser, Action<string> processor)
            where T : Parser
        {
            var optional = parser.Optional();

            if (optional.ReadString(@"\n").IsSucceed)
                processor(Environment.NewLine);
            else
            if (optional.Or().ReadString(@"\t").IsSucceed)
                processor("\t");
            else
            if (optional.Or().ReadString(@"\\").IsSucceed)
                processor("\\");

            return optional.Merge();
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


        /// <summary>
        /// Читает вызов функции
        /// <para/>
        /// FunctionName(Parameters)
        /// <para/>
        /// FunctionName()
        /// <para/>
        /// FunctionName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadFunctionCall<T>(this T parser, Action<FunctionCall> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            // Читаем имя функции
            var functionCall = new FunctionCall();
            parser
                .ReadIdentifier(name => functionCall.Name = name)
                .SkipWhiteSpaces();

            parser
                .Optional()
                    .ReadString(Lexems.OpenRoundBracket)
                    .SkipWhiteSpaces()
                    .ReadFunctionParameters(parameters => functionCall.Parameters = parameters)
                    .SkipWhiteSpaces()
                    .ReadString(Lexems.CloseRoundBracket)
                .Or()
                    .Merge();
            
            if (parser.IsSucceed)
                processor(functionCall);

            return parser;
        }

        /// <summary>
        /// Читает параметры функции
        /// <para/>
        /// [ParameterName: ParameterValue]{,ParameterName: ParameterValue}
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadFunctionParameters<T>(this T parser, Action<List<IParameter>> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            var parameters = new List<IParameter>();

            // Читаем первый параметр
            var hasParameter = false;
            parser
                .Optional()
                    .ReadParameter(parameter => parameters.Add(parameter))
                    .IsSucceed(out hasParameter)
                .Or()
                    .Merge();

            // Если параметра нет - выходим
            if (!hasParameter)
                return parser;
            
            // Читаем остальные параметры
            while (parser.IsSucceed)
            {
                // Пытаемся прочесть параметр
                var optional = parser
                    .Optional()
                        .ReadString(Lexems.Comma)
                        .SkipWhiteSpaces()
                        .ReadParameter(param => parameters.Add(param))
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
                processor(parameters);

            return parser;
        }

        /// <summary>
        /// Читает участок кода на языке генерации
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        static internal T ReadTemplateCode<T>(this T parser, Action<ITemplateCode> processor)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            bool statementParsed = false;
            parser
                .ReadString(Lexems.TemplateOpenSymbol)
                .SkipWhiteSpaces()
                .Optional()
                    .ReadStatement(processor)
                    .IsSucceed(out statementParsed)
                .Or()
                    .ReadExpression(processor)
                .Merge()
                .SkipWhiteSpaces()
                .ReadString(Lexems.TemplateCloseSymbol);

            if (statementParsed)
                parser
                    .ReadWhile(Not(AnyOf('\r', '\n')))
                    .ReadNewLine();

            return parser;
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
        static internal T ReadStatement<T>(this T parser, Action<IStatement> processor)
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
        /// 'set VariableName = Value'
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
        /// 'set VariableName = Value'
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

            parser
                .Optional()
                .ReadTemplateCode(part => template.AddPart(part))
                .Or().Merge();
                
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

        static internal T ReadNewLine<T>(this T parser)
            where T : Parser
        {
            if (!IsNormalState(parser))
                return parser;

            return parser
                .Optional()
                    .ReadString("\r\n")
                .Or()
                    .ReadChar('\r')
                .Or()
                    .ReadChar('\n')
                .Merge();
        }
    }
}
