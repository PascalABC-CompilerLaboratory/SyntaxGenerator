using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser.Extensions
{
    public static class ParserExtensions
    {
        /// <summary>
        /// Считывает символ, удовлетворяющий предикату
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T ReadChar<T>(this T parser, Predicate<char> predicate)
            where T : Parser
        {
            if (parser.Mode == ParserMode.IgnoreInstructions || parser.Mode == ParserMode.Error)
                return parser;

            if (parser.state.IsEnd || !predicate(parser.CurrentChar))
                parser.state.Failure();
            else
                parser.Advance();

            return parser;
        }

        /// <summary>
        /// Считывает символ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static T ReadChar<T>(this T parser, char c)
            where T : Parser
        => parser.ReadChar(ch => ch == c);

        /// <summary>
        /// Считывает символы, пока они удовлетворяют предикату
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T ReadWhile<T>(this T parser, Predicate<char> predicate)
            where T : Parser
        {
            if (parser.Mode == ParserMode.IgnoreInstructions || parser.Mode == ParserMode.Error)
                return parser;

            // Считываем символы пока они удовлетворяют предикату или пока не достигнут конец
            if (predicate(parser.CurrentChar))
                while (!parser.state.IsEnd && predicate(parser.Advance())) { }

            return parser;
        }

        /// <summary>
        /// Начинает накапливать пройденные символы строки. 
        /// Доступ к накопленным символам осуществляется с помощью <see cref="Process(ProcessDelegate)"/>
        /// </summary>
        /// <returns></returns>
        public static T BeginAccumulation<T>(this T parser)
            where T : Parser
        {
            parser.state.BeginAccumulation();
            return parser;
        }

        /// <summary>
        /// Останавливает накопление и позволяет обработать ранее накопленный текст, 
        /// после чего удаляет его
        /// </summary>
        /// <param name="processor">Обработчик текста</param>
        public static T EndAccumulation<T>(this T parser, ProcessDelegate processor)
            where T : Parser
        {
            processor(parser.state.Accumulated);
            parser.state.ClearAccumulator();
            return parser;
        }

        /// <summary>
        /// Считывает строку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        public static T ReadString<T>(this T parser, string @string)
            where T : Parser
        {
            foreach (char ch in @string)
                parser.ReadChar(c => c == ch);

            return parser;
        }

        /// <summary>
        /// Считывает символы вплоть до вхождения переданной строки. 
        /// Если строки нет - парсер доходит до конца
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        public static T ReadUntil<T>(this T parser, string @string)
            where T : Parser
        {
            ParserState _firstMatchState;
            do
            {
                // Доходим до вхождения первого символа
                parser.ReadWhile(c => c != @string[0]);

                // Копируем состояние
                _firstMatchState = (ParserState)parser.state.Clone();

                // Если прочли нужную строку - возвращаемся в ее начало и заканчиваем работу метода
                if (parser.ReadString(@string).IsSucceed)
                {
                    parser.state = _firstMatchState;
                    return parser;
                }
                else 
                {
                    // продолжаем работу со следующего после первого совпадения символа
                    parser.state = _firstMatchState;
                    parser.ReadChar(c => c == @string[0]);
                }
            } while (!parser.AtEnd); // Повторяем пока не дойдем до конца строки


            return parser;
        }

        /// <summary>
        /// Позволяет узнать, завершились ли предыдущие операции успехом
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static T IsSucceed<T>(this T parser, out bool success)
            where T : Parser
        {
            success = parser.IsSucceed;
            return parser;
        }

        public static OptionalParser<T> Optional<T>(this T parser)
            where T : Parser 
            => new OptionalParser<T>(parser);
    }
}
