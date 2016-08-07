using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public delegate void ProcessDelegate(string parsed);

    public class Parser
    {
        protected internal ParserState state;

        protected internal char CurrentChar => state.Current;

        protected internal char Advance() => state.Advance();

        public bool AtEnd => state.IsEnd;

        public bool IsSucceed => Mode != ParserMode.Error;

        public string Error => state.Error;

        internal ParserMode Mode
        {
            get { return state.Mode; }
            set { state.Mode = value; }
        }

        public Parser(string input)
        {
            state = new ParserState(input);
        }

        internal Parser(ParserState state)
        {
            this.state = state;
        }
    }
}
