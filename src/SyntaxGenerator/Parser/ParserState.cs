using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public enum ParserMode { Normal, IgnoreInstructions, Error };

    public class ParserState : ICloneable
    {
        private int _position = 0;

        /// <summary>
        /// Позиция первого символа накапливаемой строки в <see cref="FullInput"/>
        /// </summary>
        private int _accumulatorStartPosition = -1;

        public ParserMode Mode { get; set; } = ParserMode.Normal;

        public string FullInput { get; }

        public int Position
        {
            get { return _position; }
            set
            {
                if (value < 0 || value > FullInput.Length)
                    throw new ArgumentOutOfRangeException(nameof(Position));
                else
                    _position = value;
            }
        }

        public char Current => FullInput[_position];

        public int Row { get; private set; } = 1;

        public int Column { get; private set; } = 1;

        public bool IsEnd => Position == FullInput.Length;

        public string Accumulated => _accumulatorStartPosition == -1 ?
            "" :
            FullInput.Substring(
            _accumulatorStartPosition,
            _position - _accumulatorStartPosition);

        public string Error { get; private set; }

        public ParserState(string input)
        {
            FullInput = input;
        }

        private ParserState(string input, 
            int position, 
            int row, 
            int column, 
            ParserMode mode, 
            int accumulatorStartPos)
        {
            FullInput = input;
            _position = position;
            Row = row;
            Column = column;
            Mode = mode;
            _accumulatorStartPosition = accumulatorStartPos;
        }

        public char Advance()
        {
            if (Mode == ParserMode.Error)
                return FullInput[Position];

            // This is not good for '\r' new line
            if (Current == '\n')
            {
                ++Row;
                Column = 1;
            }
            else
                ++Column;

            Position++;
            return IsEnd ? '\0' : FullInput[Position];
        }

        public void Failure()
        {
            Mode = ParserMode.Error;
            // TODO: Save error
            //Error = IsEnd ? 
            //    "Attempt to read beyond end of string" :
            //    $"Unexpected character '{Current}' at {Row}:{Column}";
        }

        public void Failure(string message)
        {
            Mode = ParserMode.Error;
            Error = message;
        }

        public void BeginAccumulation() => _accumulatorStartPosition = _position;

        public void ClearAccumulator() => _accumulatorStartPosition = -1;

        /// <summary>
        /// Копирует текущий объект (глубокое копирование)
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new ParserState(
                FullInput,
                _position,
                Row,
                Column,
                Mode,
                _accumulatorStartPosition);
        }
    }
}
