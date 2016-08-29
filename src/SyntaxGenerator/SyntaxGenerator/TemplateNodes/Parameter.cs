using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public abstract class Parameter : AbstractNode { }

    public class Parameter<T> : Parameter
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
