using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public class OptionalParser : Parser
    {
        private readonly ParserState _originalState;

        private bool HadError => _originalState.Mode == ParserMode.Error;

        internal OptionalParser(ParserState state) : base(state)
        {
            _originalState = (ParserState)state.Clone();
        }

        /// <summary>
        /// Возвращает парсер в начальное состояние, если предыдущая попытка парсинга не удалась
        /// </summary>
        /// <returns></returns>
        public OptionalParser Or()
        {
            if (Mode == ParserMode.Normal)
                Mode = ParserMode.IgnoreInstructions;
            else
            if (!HadError && Mode == ParserMode.Error)
                state = (ParserState)_originalState.Clone();

            return this;
        }

        /// <summary>
        /// Завершает работу опционального парсера
        /// </summary>
        /// <returns></returns>
        public Parser Merge()
        {
            if (!HadError && Mode == ParserMode.IgnoreInstructions)
                state.Mode = _originalState.Mode;

            return new Parser(state);
        }

        //// Переопределение методов для изменения возвращаемого типа

        //public new OptionalParser ReadChar(Predicate<char> predicate) => base.ReadChar(predicate) as OptionalParser;

        //public new OptionalParser ReadWhile(Predicate<char> predicate) => base.ReadWhile(predicate) as OptionalParser;

        //public new OptionalParser Accumulate() => base.Accumulate() as OptionalParser;

        //public new OptionalParser Process(ProcessDelegate processor) => base.Process(processor) as OptionalParser;
    }
}
