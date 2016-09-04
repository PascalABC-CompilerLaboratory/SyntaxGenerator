using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    /// <summary>
    /// Параметр функции <see cref="FunctionCall"/>
    /// </summary>
    public interface IParameter
    {
        string Name { get; set; }
    }

    public class Parameter<T> : IParameter
    {
        public string Name { get; set; }

        public T Value { get; set; }

        public Parameter() { }

        public Parameter(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
