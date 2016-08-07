using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public class OptionalParser<T> : Parser
        where T : Parser
    {
        private readonly T _originalParser;

        protected bool HadError => _originalParser.state.Mode == ParserMode.Error;

        protected ParserState OriginalState => _originalParser.state;

        internal OptionalParser(T parser) : base((ParserState)parser.state.Clone())
        {
            _originalParser = parser;
        }

        /// <summary>
        /// Возвращает парсер в начальное состояние, если предыдущая попытка парсинга не удалась
        /// </summary>
        /// <returns></returns>
        public OptionalParser<T> Or()
        {
            if (Mode == ParserMode.Normal)
                Mode = ParserMode.IgnoreInstructions;
            else
            if (!HadError && Mode == ParserMode.Error)
                state = (ParserState)OriginalState.Clone();

            return this;
        }

        /// <summary>
        /// Завершает работу опционального парсера
        /// </summary>
        /// <returns></returns>
        public T Merge()
        {
            if (!HadError)
            {
                if (Mode == ParserMode.IgnoreInstructions)
                    Mode = ParserMode.Normal;

                _originalParser.state = state;
            }

            return _originalParser;
        }

        //// Переопределение методов для изменения возвращаемого типа

        //public new OptionalParser ReadChar(Predicate<char> predicate) => base.ReadChar(predicate) as OptionalParser;

        //public new OptionalParser ReadWhile(Predicate<char> predicate) => base.ReadWhile(predicate) as OptionalParser;

        //public new OptionalParser Accumulate() => base.Accumulate() as OptionalParser;

        //public new OptionalParser Process(ProcessDelegate processor) => base.Process(processor) as OptionalParser;
    }
}
